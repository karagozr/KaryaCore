using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Identity.Infrastructure.Persistence;
using Karya.Core.Identity.Infrastructure.Persistence.Repositories;
using Karya.Core.Interfaces.Entities;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Interfaces.Repositories;
using Karya.Core.Results;
using Karya.Core.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text.Json;

namespace Karya.Core.Identity.Application.Services;

public class AppUserService : BaseService<AppUserRepository, AppUser, Guid>, IAppUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ICurrentUser _currentUser;
    private readonly IIdentityEntityFactory _entityFactory;

    public AppUserService(IdentityUnitOfWork uow, ICurrentUser currentUser, UserManager<AppUser> userManager, IIdentityEntityFactory entityFactory)
        : base(uow)
    {
        _userManager = userManager;
        _currentUser = currentUser;
        _entityFactory = entityFactory;
    }

    public async override Task<BaseResult<TDto>> ByKey<TDto>(Guid key)
    {
        var result = await base.ByKey<TDto>(key);
        if(result.IsSuccess && (result.Data is AppUserSDto dto))
        {
            var user = await _userManager.FindByIdAsync(key.ToString());
            if (user != null)
            {
                dto.IsSystemAdmin = user.IsSystemAdmin;
            }
        }
        return result;
    }

    public override async Task<BaseResult<LoadResult>> Select<TDto>(DataSourceLoadOptionsBase filterDataOptions)
    {
        var query = _uow.Repo<IRepositoryAsync<AppUser, Guid>>().Query().Where(x=>x.TenantId==_currentUser.TenantId)
            .Select(x => new AppUserLDto
            {
                Id = x.Id,
                Email = x.Email,
                UserName = x.UserName,
                PhoneNumber = x.PhoneNumber,
                FirstName = x.FirstName,
                LastName = x.LastName,
                IsActive = x.IsActive,
                IsSystemAdmin = x.IsSystemAdmin,

            });

        var res = await DataSourceLoader.LoadAsync(query, filterDataOptions);

        return BaseResult<LoadResult>.Success("200", null, res);
    }

    public override async Task<BaseResult> Insert<TDto>(TDto dto)
    {
        if (dto is not AppUserADto add)
            return await base.Insert(dto);

        var user = _entityFactory.CreateUser();
        user.UserName = add.UserName;
        user.Email = add.Email;
        user.PhoneNumber = add.PhoneNumber;
        user.EmailConfirmed = true;
        user.IsSystemAdmin = false;
        user.TenantId = _currentUser.TenantId;
        user.FirstName = add.FirstName;
        user.LastName = add.LastName;
        user.IsActive = add.IsActive;

        OnUserCreating(user, add);

        // Kullanýcý, oluþturulduðu (aktif) tenant'a üye yapýlýr.
        user.TenantMemberships.Add(new AppUserTenant { TenantId = _currentUser.TenantId });

        var result = await _userManager.CreateAsync(user, add.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.ToDictionary(e => e.Code, e => e.Description);
            return BaseResult.Error("400", "Kullanýcý oluþturulamadý.", errors);
        }

        return BaseResult.SuccessCoded("201", MessageCodes.Created);
    }

    /// <summary>Geniþletilmiþ kullanýcý alanlarýný doldurmak için override edilebilir.</summary>
    protected virtual void OnUserCreating(AppUser user, AppUserADto dto)
    {
    }

    protected virtual void OnUserUpdating(AppUser user, Dictionary<string, object> updateData)
    {
    }

    public virtual async Task<BaseResult<bool>> ChangePasswordAsync(string currentPassword, string newPassword)
    {
        var user = await _userManager.FindByNameAsync(_currentUser.UserName);

        if (user is null)
            return BaseResult<bool>.ErrorCoded("404", MessageCodes.NotFound, false, "AppUser");

        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

        return result.Succeeded
            ? BaseResult<bool>.SuccessCoded("200", MessageCodes.Success, true)
            : BaseResult<bool>.Error(
                "400",
                "Þifre deðiþtirilemedi.",
                false,
                result.Errors.ToDictionary(e => e.Code, e => e.Description)
            );
    }

    public override async Task<BaseResult> Update<TDto>(Guid key, Dictionary<string, object> updateData)
    {
        if (typeof(TDto) != typeof(AppUserUDto)) return await base.Update<TDto>(key, updateData);

        var data = new Dictionary<string, object>(updateData, StringComparer.OrdinalIgnoreCase);

        var user = await _uow.Repo<AppUserRepository>().GetByIdAsync(key);
        if (user is null) return BaseResult.ErrorCoded("404", MessageCodes.NotFound, "AppUser", "Id", key.ToString());

        if (data.TryGetValue(nameof(AppUserUDto.Email), out var email) && email is not null) user.Email = email.ToString();
        if (data.TryGetValue(nameof(AppUserUDto.PhoneNumber), out var phone)) user.PhoneNumber = phone?.ToString();
        //if (data.TryGetValue(nameof(AppUserUDto.IsSystemAdmin), out var isAdmin) && isAdmin is not null) user.IsSystemAdmin = Convert.ToBoolean(isAdmin);

        if (data.TryGetValue(nameof(AppUserUDto.FirstName), out var firstName)) user.FirstName = firstName?.ToString();
        if (data.TryGetValue(nameof(AppUserUDto.LastName), out var lastName)) user.LastName = lastName?.ToString();
        if (data.TryGetValue(nameof(AppUserUDto.IsActive), out var isActive)) user.IsActive = isActive is JsonElement json ? json.GetBoolean() : Convert.ToBoolean(isActive);

        OnUserUpdating(user, data);

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = result.Errors.ToDictionary(e => e.Code, e => e.Description);
            return BaseResult.Error("400", "Kullanýcý güncellenemedi.", errors);
        }

        return BaseResult.SuccessCoded("200", MessageCodes.Success);
    }
}
