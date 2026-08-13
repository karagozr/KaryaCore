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
    private readonly RoleManager<AppRole> _roleManager;

    public IdentityPermissionService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
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

        var roles = await _userManager.GetRolesAsync(user);

        foreach (var roleName in roles)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role is null)
                continue;

            var claims = await _roleManager.GetClaimsAsync(role);

            var hasPermission = claims.Any(x =>
                x.Type == "Permission" &&
                string.Equals(
                    x.Value,
                    permission,
                    StringComparison.OrdinalIgnoreCase));

            if (hasPermission)
                return true;
        }
        return false;
    }
}
