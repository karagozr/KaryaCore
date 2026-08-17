using Karya.Core.App.Interfaces.Services;
using Karya.Core.Indentity.Domains.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Services;

public class IdentityPermissionService : IPermissionService
{
    public const string TenantAdminRole = "TenantAdmin";
    public const string PermissionClaimType = "Permission";

    private readonly UserManager<AppUser> _userManager;
    private readonly AppRoleService _roleService;
    private readonly AppUserRoleService _userRoleService;
    private readonly AppUserRoleGroupService _userRoleGroupService;
    private readonly AppRoleGroupRoleService _roleGroupRoleService;
    private readonly AppUserClaimService _appUserClaimService;

    public IdentityPermissionService(
        UserManager<AppUser> userManager,
        AppRoleService roleService,
        AppUserRoleService userRoleService,
        AppUserRoleGroupService userRoleGroupService,
        AppRoleGroupRoleService roleGroupRoleService,
        AppUserClaimService appUserClaimService)
    {
        _userManager = userManager;
        _roleService = roleService;
        _userRoleService = userRoleService;
        _userRoleGroupService = userRoleGroupService;
        _roleGroupRoleService = roleGroupRoleService;
        _appUserClaimService = appUserClaimService;
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

        if (user.IsSystemAdmin)
            return true;

        var hasUserPermission = await _appUserClaimService.Query()
            .AnyAsync(x =>
                x.UserId == user.Id &&
                x.ClaimType == PermissionClaimType &&
                x.ClaimValue == permission);

        if (hasUserPermission)
            return true;

        var directRoleIds = _userRoleService.Query()
            .Where(x => x.UserId == user.Id)
            .Select(x => x.RoleId);

        var roleGroupIds = _userRoleGroupService.Query()
            .Where(x => x.UserId == user.Id)
            .Select(x => x.RoleGroupId);

        var groupRoleIds = _roleGroupRoleService.Query()
            .Where(x => roleGroupIds.Contains(x.RoleGroupId))
            .Select(x => x.RoleId);

        var roleIds = directRoleIds
            .Concat(groupRoleIds)
            .Distinct();

        if (permission.StartsWith("AppUser.", StringComparison.OrdinalIgnoreCase))
        {
            return await _roleService.Query()
                .AnyAsync(x => roleIds.Contains(x.Id) && x.Name == TenantAdminRole);
        }

        return await _roleService.Query()
            .AnyAsync(x =>
                roleIds.Contains(x.Id) &&
                x.RoleClaims.Any(c =>
                    c.ClaimType == PermissionClaimType &&
                    c.ClaimValue == permission));
    }
}