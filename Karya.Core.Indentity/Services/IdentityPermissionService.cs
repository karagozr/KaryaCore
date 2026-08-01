using Karya.Core.App.Interfaces.Services;
using Karya.Core.Indentity.Domains.Entities;
using Microsoft.AspNetCore.Identity;

namespace Karya.Core.Indentity.Services;

/// <summary>
/// Rol/flag tabanlı yetki servisi. SystemAdmin her şeye yetkilidir; TenantAdmin
/// kullanıcı yönetimi (AppUser.*) yetkilerine sahiptir (tenant kapsamı row-level
/// olarak repository katmanında uygulanır).
/// </summary>
public class IdentityPermissionService : IPermissionService
{
    public const string TenantAdminRole = "TenantAdmin";

    private readonly UserManager<AppUser> _userManager;

    public IdentityPermissionService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> HasPermissionAsync(string userId, string permission)
    {
        if (string.IsNullOrEmpty(permission))
            return true;

        if (string.IsNullOrEmpty(userId))
            return false;

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return false;

        // Sistem admini tüm tenant'larda her işlemi yapabilir.
        if (user.IsSystemAdmin)
            return true;

        // Kullanıcı yönetimi işlemleri TenantAdmin rolüne açıktır.
        if (permission.StartsWith("AppUser.", StringComparison.OrdinalIgnoreCase))
            return await _userManager.IsInRoleAsync(user, TenantAdminRole);

        return false;
    }
}
