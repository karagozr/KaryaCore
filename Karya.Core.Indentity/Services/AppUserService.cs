using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.DTOs;
using Karya.Core.Indentity.Infrastructure;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Results;
using Karya.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Services;

public class AppUserService : BaseService<AppUserRepository, AppUser, Guid>, IAppUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ICurrentUser _currentUser;

    public AppUserService(DbContext context, ICurrentUser currentUser, UserManager<AppUser> userManager)
        : base(new IdentityUnitOfWork(context, currentUser))
    {
        _userManager = userManager;
        _currentUser = currentUser;
    }

    public override async Task<BaseResult> Insert<TDto>(TDto dto)
    {
        if (dto is not AppUserADto add)
            return await base.Insert(dto);

        var user = new AppUser
        {
            UserName = add.UserName,
            Email = add.Email,
            PhoneNumber = add.PhoneNumber,
            EmailConfirmed = true,
            IsSystemAdmin = add.IsSystemAdmin,
            TenantId = add.TenantId,
            ErpPersonId = add.ErpPersonId,
            ErpUsername = add.ErpUsername,
            FirstName = add.FirstName,
            LastName = add.LastName,
            Site = add.Site,
        };

        // Kullanıcı, oluşturulduğu (aktif) tenant'a üye yapılır.
        user.TenantMemberships.Add(new AppUserTenant { TenantId = add.TenantId });

        var result = await _userManager.CreateAsync(user, add.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.ToDictionary(e => e.Code, e => e.Description);
            return BaseResult.Error("400", "Kullanıcı oluşturulamadı.", errors);
        }

        return BaseResult.SuccessCoded("201", MessageCodes.Created);
    }

    public virtual async Task<BaseResult<bool>> ResetPasswordAsync(string email, string token, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return BaseResult<bool>.ErrorCoded("404", MessageCodes.NotFound, false, "AppUser", "Email", email);

        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

        return result.Succeeded
            ? BaseResult<bool>.SuccessCoded("200", MessageCodes.Success, true)
            : BaseResult<bool>.Error("400", "Şifre sıfırlanamadı.", false, result.Errors.ToDictionary(e => e.Code, e => e.Description));
    }

    public override async Task<BaseResult> Update<TDto>(Guid key, Dictionary<string, object> updateData)
    {
        if (typeof(TDto) != typeof(AppUserUDto))
            return await base.Update<TDto>(key, updateData);

        // Tenant scoping repository Query üzerinden uygulanır.
        var user = await _uow.Repo<AppUserRepository>().GetByIdAsync(key);
        if (user is null)
            return BaseResult.ErrorCoded("404", MessageCodes.NotFound, "AppUser", "Id", key.ToString());

        if (updateData.TryGetValue(nameof(AppUserUDto.Email), out var email) && email is not null)
            user.Email = email.ToString();

        if (updateData.TryGetValue(nameof(AppUserUDto.PhoneNumber), out var phone))
            user.PhoneNumber = phone?.ToString();

        if (updateData.TryGetValue(nameof(AppUserUDto.IsSystemAdmin), out var isAdmin) && isAdmin is not null)
            user.IsSystemAdmin = Convert.ToBoolean(isAdmin);

        if (updateData.TryGetValue(nameof(AppUserUDto.ErpPersonId), out var erpPersonId))
            user.ErpPersonId = erpPersonId?.ToString();

        if (updateData.TryGetValue(nameof(AppUserUDto.ErpUsername), out var erpUsername))
            user.ErpUsername = erpUsername?.ToString();

        if (updateData.TryGetValue(nameof(AppUserUDto.FirstName), out var firstName))
            user.FirstName = firstName?.ToString();

        if (updateData.TryGetValue(nameof(AppUserUDto.LastName), out var lastName))
            user.LastName = lastName?.ToString();

        if (updateData.TryGetValue(nameof(AppUserUDto.Site), out var site))
            user.Site = site?.ToString();

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = result.Errors.ToDictionary(e => e.Code, e => e.Description);
            return BaseResult.Error("400", "Kullanıcı güncellenemedi.", errors);
        }

        return BaseResult.SuccessCoded("200", MessageCodes.Success);
    }
}
