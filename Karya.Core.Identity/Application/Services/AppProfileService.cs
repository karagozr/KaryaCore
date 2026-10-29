using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Identity.Extensions;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Karya.Core.Identity.Application.Services;

public interface IAppProfileService
{
    Task<BaseResult<AppProfileDto>> GetBasicAsync();
    Task<BaseResult<AppProfileUserDto>> GetAsync();

    Task<BaseResult<AppProfileDto>> UpdateAsync(Dictionary<string, object> updateData);

    Task<BaseResult<LoadResult>> GetClaimsAsync(DataSourceLoadOptionsBase loadOptions);

    Task<BaseResult<AppProfileClaimDto>> AddClaimAsync(AppProfileClaimDto dto);

    Task<BaseResult<AppProfileClaimDto>> GetClaimAsync(string key);

    Task<BaseResult<AppProfileClaimDto>> UpdateClaimAsync(string key, Dictionary<string, object> updateData);

    Task<BaseResult> DeleteClaimAsync(string key);
}

public class AppProfileService : IAppProfileService
{
    protected readonly UserManager<AppUser> UserManager;
    protected readonly AppTenantService TenantService;
    protected readonly ICurrentUser CurrentUser;
    protected readonly UserProfileOptions Options;

    public AppProfileService(UserManager<AppUser> userManager, AppTenantService tenantService, ICurrentUser currentUser, IOptions<CoreIdentityOptions> options)
    {
        UserManager = userManager;
        TenantService = tenantService;
        CurrentUser = currentUser;
        Options = options.Value.Profile;
    }

    /// Profil üzerinden yönetilebilen claim tipleri.
    protected virtual IReadOnlyCollection<string> AllowedClaimTypes => Options.ClaimTypes;

    public virtual async Task<BaseResult<AppProfileDto>> GetBasicAsync()
    {
        var user = await FindCurrentUserAsync();
        if (user is null)
            return BaseResult<AppProfileDto>.Error("404", "User not found.", null);

        return BaseResult<AppProfileDto>.Success("200", null, await BuildProfileAsync(user));
    }

    public virtual async Task<BaseResult<AppProfileDto>> UpdateAsync(Dictionary<string, object> updateData)
    {
        var user = await FindCurrentUserAsync();
        if (user is null)
            return BaseResult<AppProfileDto>.Error("404", "User not found.", null);

        var allowed = AllowedClaimTypes;
        

        var existing = await UserManager.GetClaimsAsync(user);

        foreach (var (key, value) in updateData)
        {
            var type = allowed.First(a => string.Equals(a, key, StringComparison.OrdinalIgnoreCase));
            var current = existing.Where(c => c.Type == type).ToList();

            if (current.Count > 0)
            {
                var removeResult = await UserManager.RemoveClaimsAsync(user, current);
                if (!removeResult.Succeeded)
                    return ToError(removeResult);
            }

            if (!string.IsNullOrWhiteSpace(value?.ToString()))
            {
                var addResult = await UserManager.AddClaimAsync(user, new Claim(type, value.ToString()));
                if (!addResult.Succeeded)
                    return ToError(addResult);
            }
        }

        return BaseResult<AppProfileDto>.Success("200", null, await BuildProfileAsync(user));
    }

    protected virtual async Task<AppUser?> FindCurrentUserAsync()
    {
        if (string.IsNullOrWhiteSpace(CurrentUser.UserId))
            return null;

        return await UserManager.FindByIdAsync(CurrentUser.UserId);
    }

    private async Task<AppTenant?> GetCurrentTenantAsync(AppUser user)
    {
        var tenant = await TenantService.Query().FirstOrDefaultAsync(x => x.Id == (string.IsNullOrWhiteSpace(CurrentUser.TenantId) ? user.TenantId : CurrentUser.TenantId));
        if (tenant is null) throw new InvalidOperationException($"Tenant with ID '{user.TenantId}' not found.");
        return tenant;
    }

    protected virtual async Task<AppProfileDto> BuildProfileAsync(AppUser user)
    {
        var allowed = AllowedClaimTypes;
        var claims = await UserManager.GetClaimsAsync(user);
       
        return new AppProfileDto
        {
            User = new AppBasicProfileUserDto
            {
                FirstName= user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                Username = user.UserName,
                TenantId = string.IsNullOrWhiteSpace(CurrentUser.TenantId) ? user.TenantId : CurrentUser.TenantId,
                TenantName = (await GetCurrentTenantAsync(user))?.Name

            },
            Claims = allowed.ToDictionary(t => t, t => claims.FirstOrDefault(c => c.Type == t)?.Value)
        };
    }


    private static BaseResult<AppProfileDto> ToError(IdentityResult result) =>
        BaseResult<AppProfileDto>.Error("400", "Profile update failed.", null,
            result.Errors.ToDictionary(e => e.Code, e => e.Description));

    public async Task<BaseResult<AppProfileUserDto>> GetAsync()
    {
        var user = await FindCurrentUserAsync();
        if (user is null)
            return BaseResult<AppProfileUserDto>.Error("404", "User not found.", null);

        return BaseResult<AppProfileUserDto>.Success("200", null, new AppProfileUserDto
        {
            Id = user.Id,
            Username = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            TenantId = string.IsNullOrWhiteSpace(CurrentUser.TenantId) ? user.TenantId : CurrentUser.TenantId,
            TenantName = (await GetCurrentTenantAsync(user))?.Name,
            IsSystemAdmin = user.IsSystemAdmin,
            ErpPersonId = user.ErpPersonId,
            ErpUsername = user.ErpUsername,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsActive = user.IsActive
        });
    }

    public virtual async Task<BaseResult<LoadResult>> GetClaimsAsync(DataSourceLoadOptionsBase loadOptions)
    {
        var user = await FindCurrentUserAsync();
        if (user is null)
            return BaseResult<LoadResult>.Error("404", "User not found.", null);

        var allowed = AllowedClaimTypes;
        var claims = await UserManager.GetClaimsAsync(user);

        var query = claims
            .Where(c => allowed.Contains(c.Type))
            .Select(c => new AppProfileClaimDto { Key = c.Type, Value = c.Value })
            .AsQueryable();

        var res = DataSourceLoader.Load(query, loadOptions);

        return BaseResult<LoadResult>.Success("200", null, res);
    }

    public virtual async Task<BaseResult<AppProfileClaimDto>> AddClaimAsync(AppProfileClaimDto dto)
    {
        var user = await FindCurrentUserAsync();
        if (user is null)
            return BaseResult<AppProfileClaimDto>.Error("404", "User not found.", null);

        var type = ResolveClaimType(dto.Key);
        if (type is null)
            return BaseResult<AppProfileClaimDto>.Error("400", $"'{dto.Key}' claim type is not allowed.", null);

        if (string.IsNullOrWhiteSpace(dto.Value))
            return BaseResult<AppProfileClaimDto>.Error("400", "Value is required.", null);

        var existing = (await UserManager.GetClaimsAsync(user)).Where(c => c.Type == type).ToList();
        if (existing.Count > 0)
        {
            var removeResult = await UserManager.RemoveClaimsAsync(user, existing);
            if (!removeResult.Succeeded)
                return ToClaimError<AppProfileClaimDto>(removeResult);
        }

        var result = await UserManager.AddClaimAsync(user, new Claim(type, dto.Value));
        if (!result.Succeeded)
            return ToClaimError<AppProfileClaimDto>(result);

        return BaseResult<AppProfileClaimDto>.Success("200", null, new AppProfileClaimDto { Key = type, Value = dto.Value });
    }

    public virtual async Task<BaseResult<AppProfileClaimDto>> GetClaimAsync(string key)
    {
        var user = await FindCurrentUserAsync();
        if (user is null)
            return BaseResult<AppProfileClaimDto>.Error("404", "User not found.", null);

        var type = ResolveClaimType(key);
        if (type is null)
            return BaseResult<AppProfileClaimDto>.Error("400", $"'{key}' claim type is not allowed.", null);

        var claim = (await UserManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == type);
        if (claim is null)
            return BaseResult<AppProfileClaimDto>.Error("404", $"'{type}' claim not found.", null);

        return BaseResult<AppProfileClaimDto>.Success("200", null, new AppProfileClaimDto { Key = claim.Type, Value = claim.Value });
    }

    public virtual async Task<BaseResult<AppProfileClaimDto>> UpdateClaimAsync(string key, Dictionary<string, object> updateData)
    {
        var user = await FindCurrentUserAsync();
        if (user is null)
            return BaseResult<AppProfileClaimDto>.Error("404", "User not found.", null);

        var type = ResolveClaimType(key);
        if (type is null)
            return BaseResult<AppProfileClaimDto>.Error("400", $"'{key}' claim type is not allowed.", null);

        var valueEntry = updateData.FirstOrDefault(x => string.Equals(x.Key, nameof(AppProfileClaimDto.Value), StringComparison.OrdinalIgnoreCase));
        var dto = new AppProfileClaimDto { Key = type, Value = valueEntry.Value?.ToString() };

        if (string.IsNullOrWhiteSpace(dto.Value))
            return BaseResult<AppProfileClaimDto>.Error("400", "Value is required.", null);

        var current = (await UserManager.GetClaimsAsync(user)).Where(c => c.Type == type).ToList();
        if (current.Count == 0)
            return BaseResult<AppProfileClaimDto>.Error("404", $"'{type}' claim not found.", null);

        var removeResult = await UserManager.RemoveClaimsAsync(user, current);
        if (!removeResult.Succeeded)
            return ToClaimError<AppProfileClaimDto>(removeResult);

        var addResult = await UserManager.AddClaimAsync(user, new Claim(type, dto.Value));
        if (!addResult.Succeeded)
            return ToClaimError<AppProfileClaimDto>(addResult);

        return BaseResult<AppProfileClaimDto>.Success("200", null, new AppProfileClaimDto { Key = type, Value = dto.Value });
    }

    public virtual async Task<BaseResult> DeleteClaimAsync(string key)
    {
        var user = await FindCurrentUserAsync();
        if (user is null)
            return BaseResult.Error("404", "User not found.");

        var type = ResolveClaimType(key);
        if (type is null)
            return BaseResult.Error("400", $"'{key}' claim type is not allowed.");

        var current = (await UserManager.GetClaimsAsync(user)).Where(c => c.Type == type).ToList();
        if (current.Count == 0)
            return BaseResult.Error("404", $"'{type}' claim not found.");

        var result = await UserManager.RemoveClaimsAsync(user, current);
        if (!result.Succeeded)
            return BaseResult.Error("400", "Claim delete failed.", result.Errors.ToDictionary(e => e.Code, e => e.Description));

        return BaseResult.Success("200");
    }

    protected virtual string? ResolveClaimType(string? key) =>
        string.IsNullOrWhiteSpace(key)
            ? null
            : AllowedClaimTypes.FirstOrDefault(a => string.Equals(a, key, StringComparison.OrdinalIgnoreCase));

    private static BaseResult<T> ToClaimError<T>(IdentityResult result) =>
        BaseResult<T>.Error("400", "Claim update failed.", default,
            result.Errors.ToDictionary(e => e.Code, e => e.Description));
}
