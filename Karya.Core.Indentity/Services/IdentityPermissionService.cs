using Karya.Core.App.Interfaces.Services;
using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Services;

public class IdentityPermissionService : IPermissionService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly AppRoleService _roleService;
    private readonly AppUserRoleService _userRoleService;
    private readonly AppUserRoleGroupService _userRoleGroupService;
    private readonly AppRoleGroupRoleService _roleGroupRoleService;

    public IdentityPermissionService(UserManager<AppUser> userManager, AppRoleService roleService, AppUserRoleService userRoleService, AppUserRoleGroupService userRoleGroupService, AppRoleGroupRoleService roleGroupRoleService)
    {
        _userManager = userManager;
        _roleService = roleService;
        _userRoleService = userRoleService;
        _userRoleGroupService = userRoleGroupService;
        _roleGroupRoleService = roleGroupRoleService;
    }

    public virtual async Task<bool> HasPermissionAsync(string userId, string permission)
    {
        if (string.IsNullOrWhiteSpace(permission))
            return true;

        if (string.IsNullOrWhiteSpace(userId))
            return false;

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return false;

        if (user.IsSystemAdmin)
            return true;

        var normalizedPermission = permission.ToUpperInvariant();

        var directRoleIds = _userRoleService.Query()
            .Where(x => x.UserId == user.Id)
            .Select(x => x.RoleId);

        var hasDirectPermission = await _roleService.Query()
            .AnyAsync(x => directRoleIds.Contains(x.Id) && x.NormalizedName == normalizedPermission);

        if (hasDirectPermission)
            return true;

        var roleGroupIds = await _userRoleGroupService.Query()
            .Where(x => x.UserId == user.Id)
            .Select(x => x.RoleGroupId)
            .ToListAsync();

        foreach (var roleGroupId in roleGroupIds)
        {
            var parent = new AppRoleGroupRoleParentFilter
            {
                RoleGroupId = roleGroupId
            };

            var groupRoleIds = _roleGroupRoleService.Query(parent)
                .Select(x => x.RoleId);

            var hasGroupPermission = await _roleService.Query()
                .AnyAsync(x => groupRoleIds.Contains(x.Id) && x.NormalizedName == normalizedPermission);

            if (hasGroupPermission)
                return true;
        }

        return false;
    }
}