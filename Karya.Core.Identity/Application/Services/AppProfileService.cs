using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Identity.Extensions;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Karya.Core.Identity.Application.Services;

public interface IAppProfileService
{
    Task<BaseResult<AppProfileDto>> GetAsync();

    Task<BaseResult<AppProfileDto>> UpdateAsync(AppProfileUDto dto);
}

public class AppProfileService : IAppProfileService
{
    protected readonly UserManager<AppUser> UserManager;
    protected readonly ICurrentUser CurrentUser;
    protected readonly UserProfileOptions Options;

    public AppProfileService(UserManager<AppUser> userManager, ICurrentUser currentUser, IOptions<CoreIdentityOptions> options)
    {
        UserManager = userManager;
        CurrentUser = currentUser;
        Options = options.Value.Profile;
    }

    /// Profil üzerinden yönetilebilen claim tipleri.
    protected virtual IReadOnlyCollection<string> AllowedClaimTypes => Options.ClaimTypes;

    public virtual async Task<BaseResult<AppProfileDto>> GetAsync()
    {
        var user = await FindCurrentUserAsync();
        if (user is null)
            return BaseResult<AppProfileDto>.Error("404", "User not found.", null);

        return BaseResult<AppProfileDto>.Success("200", null, await BuildProfileAsync(user));
    }

    public virtual async Task<BaseResult<AppProfileDto>> UpdateAsync(AppProfileUDto dto)
    {
        var user = await FindCurrentUserAsync();
        if (user is null)
            return BaseResult<AppProfileDto>.Error("404", "User not found.", null);

        var allowed = AllowedClaimTypes;
        var invalid = dto.Claims.Keys.Where(k => !allowed.Contains(k, StringComparer.OrdinalIgnoreCase)).ToList();
        if (invalid.Count > 0)
            return BaseResult<AppProfileDto>.Error("400", "Invalid profile claim type.", null,
                invalid.ToDictionary(k => k, _ => "Not allowed."));

        var existing = await UserManager.GetClaimsAsync(user);

        foreach (var (key, value) in dto.Claims)
        {
            var type = allowed.First(a => string.Equals(a, key, StringComparison.OrdinalIgnoreCase));
            var current = existing.Where(c => c.Type == type).ToList();

            if (current.Count > 0)
            {
                var removeResult = await UserManager.RemoveClaimsAsync(user, current);
                if (!removeResult.Succeeded)
                    return ToError(removeResult);
            }

            if (!string.IsNullOrWhiteSpace(value))
            {
                var addResult = await UserManager.AddClaimAsync(user, new Claim(type, value));
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

    protected virtual async Task<AppProfileDto> BuildProfileAsync(AppUser user)
    {
        var allowed = AllowedClaimTypes;
        var claims = await UserManager.GetClaimsAsync(user);

        return new AppProfileDto
        {
            User = MapUser(user),
            Claims = allowed.ToDictionary(t => t, t => claims.FirstOrDefault(c => c.Type == t)?.Value)
        };
    }

    protected virtual AppProfileUserDto MapUser(AppUser user) => new()
    {
        Id = user.Id,
        UserName = user.UserName,
        Email = user.Email,
        PhoneNumber = user.PhoneNumber,
        TenantId = string.IsNullOrWhiteSpace(CurrentUser.TenantId) ? user.TenantId : CurrentUser.TenantId,
        IsSystemAdmin = user.IsSystemAdmin,
        ErpPersonId = user.ErpPersonId,
        ErpUsername = user.ErpUsername,
        FirstName = user.FirstName,
        LastName = user.LastName,
        IsActive = user.IsActive
    };

    private static BaseResult<AppProfileDto> ToError(IdentityResult result) =>
        BaseResult<AppProfileDto>.Error("400", "Profile update failed.", null,
            result.Errors.ToDictionary(e => e.Code, e => e.Description));
}
