using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Identity.Infrastructure.Persistence;
using Karya.Core.Identity.Infrastructure.Persistence.Repositories;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Results;
using Karya.Core.Services;
using Microsoft.AspNetCore.Identity;

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

    public override async Task<BaseResult> Insert<TDto>(TDto dto)
    {
        if (dto is not AppUserADto add)
            return await base.Insert(dto);

        var user = _entityFactory.CreateUser();
        user.UserName = add.UserName;
        user.Email = add.Email;
        user.PhoneNumber = add.PhoneNumber;
        user.EmailConfirmed = true;
        user.IsSystemAdmin = add.IsSystemAdmin;
        user.TenantId = _currentUser.TenantId;
        user.ErpPersonId = add.ErpPersonId;
        user.ErpUsername = add.ErpUsername;
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
        if (data.TryGetValue(nameof(AppUserUDto.IsSystemAdmin), out var isAdmin) && isAdmin is not null) user.IsSystemAdmin = Convert.ToBoolean(isAdmin);

        if (data.TryGetValue(nameof(AppUserUDto.ErpPersonId), out var erpPersonId)) user.ErpPersonId = erpPersonId?.ToString();
        if (data.TryGetValue(nameof(AppUserUDto.ErpUsername), out var erpUsername)) user.ErpUsername = erpUsername?.ToString();
        if (data.TryGetValue(nameof(AppUserUDto.FirstName), out var firstName)) user.FirstName = firstName?.ToString();
        if (data.TryGetValue(nameof(AppUserUDto.LastName), out var lastName)) user.LastName = lastName?.ToString();
        if (data.TryGetValue(nameof(AppUserUDto.IsActive), out var isActive)) user.IsActive = Convert.ToBoolean(isActive);

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = result.Errors.ToDictionary(e => e.Code, e => e.Description);
            return BaseResult.Error("400", "Kullanýcý güncellenemedi.", errors);
        }

        return BaseResult.SuccessCoded("200", MessageCodes.Success);
    }
}
