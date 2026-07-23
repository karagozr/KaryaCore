using Karya.Core.Indentity.DTOs;
using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.Infrastructure;
using Karya.Core.Interfaces.DTOs;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Results;
using Karya.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Services;

/// <summary>
/// AppUser CRUD servisi. Standart pipeline'ı kullanır; ekleme/güncellemede
/// ASP.NET Identity UserManager ile parola hash'leme ve tenant üyeliği yönetir.
/// </summary>
public class AppUserService : BaseService<AppUserRepository, AppUser, Guid>
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
            TenantId = _currentUser.TenantId
        };

        // Kullanıcı, oluşturulduğu (aktif) tenant'a üye yapılır.
        user.TenantMemberships.Add(new AppUserTenant { TenantId = _currentUser.TenantId });

        var result = await _userManager.CreateAsync(user, add.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.ToDictionary(e => e.Code, e => e.Description);
            return BaseResult.Error("400", "Kullanıcı oluşturulamadı.", errors);
        }

        return BaseResult.SuccessCoded("201", MessageCodes.Created);
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

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = result.Errors.ToDictionary(e => e.Code, e => e.Description);
            return BaseResult.Error("400", "Kullanıcı güncellenemedi.", errors);
        }

        return BaseResult.SuccessCoded("200", MessageCodes.Success);
    }
}
