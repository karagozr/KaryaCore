using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.DTOs;
using Karya.Core.Indentity.Infrastructure.Migrations;
using Karya.Core.Indentity.Providers;
using Karya.Core.Indentity.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Karya.Core.Indentity.Seeders;

public sealed class IdentityDataSeeder : IDatabaseSeeder
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly AppRoleGroupService _roleGroupService;
    private readonly AppRoleGroupRoleService _roleGroupRoleService;
    private readonly AppUserRoleGroupService _userRoleGroupService;
    private readonly AppTenantService _tenantService;
    private readonly AppUserTenantService _userTenantService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityDataSeeder(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        AppRoleGroupService roleGroupService,
        AppRoleGroupRoleService roleGroupRoleService,
        AppUserRoleGroupService userRoleGroupService,
        AppTenantService tenantService,
        AppUserTenantService userTenantService,
        IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _roleGroupService = roleGroupService;
        _roleGroupRoleService = roleGroupRoleService;
        _userRoleGroupService = userRoleGroupService;
        _tenantService = tenantService;
        _userTenantService = userTenantService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task SeedAsync()
    {
        const string tenantId = "DEFAULT";
        const string adminGroupName = "Admin";
        const string adminEmail = "admin@mail.com";
        const string adminPassword = "Admin123*";

        await EnsureTenantAsync(tenantId);

        var roles = await EnsureRolesAsync();
        var adminGroup = await _roleGroupService.EnsureAsync(adminGroupName, tenantId);
        var adminUser = await EnsureAdminUserAsync(adminEmail, adminPassword, tenantId);

        await RunWithSeederContextAsync(adminUser, tenantId, async () =>
        {
            await EnsureGroupRolesAsync(adminGroup.Id, roles, tenantId);
            await _userTenantService.AssignAsync(adminUser.Id, tenantId);
            await EnsureUserGroupAsync(adminUser.Id, adminGroup.Id, tenantId);
        });
    }

    private async Task EnsureTenantAsync(string tenantId)
    {
        if (await _tenantService.Query().AnyAsync(x => x.Id == tenantId))
            return;

        await _tenantService.Insert(new AppTenantADto
        {
            Id = tenantId,
            Name = "Default",
            Description = "Default Tenant",
            IsActive = true
        });
    }

    private async Task<List<AppRole>> EnsureRolesAsync()
    {
        var roles = new List<AppRole>();

        foreach (var definition in RoleProvider.GetRoles())
        {
            var role = await _roleManager.FindByNameAsync(definition.Name);

            if (role is null)
            {
                role = new AppRole
                {
                    Id = Guid.NewGuid(),
                    Name = definition.Name,
                    Description = definition.Description
                };

                var result = await _roleManager.CreateAsync(role);

                if (!result.Succeeded)
                    throw new Exception($"{definition.Name} rolü oluşturulamadı: {GetErrors(result)}");
            }

            roles.Add(role);
        }

        return roles;
    }

    private async Task<AppUser> EnsureAdminUserAsync(string email, string password, string tenantId)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is not null)
            return user;

        user = new AppUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            TenantId = tenantId,
            IsSystemAdmin = false
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            throw new Exception($"Admin kullanıcısı oluşturulamadı: {GetErrors(result)}");

        return user;
    }

    private async Task EnsureGroupRolesAsync(Guid groupId, IEnumerable<AppRole> roles, string tenantId)
    {
        foreach (var role in roles)
        {
            if (await _roleGroupRoleService.ExistsAsync(groupId, role.Id, tenantId))
                continue;

            var result = await _roleGroupRoleService.AssignAsync(groupId, role.Id, tenantId);

            if (!result.IsSuccess)
                throw new Exception($"{role.Name} rolü gruba atanamadı.");
        }
    }

    private async Task EnsureUserGroupAsync(Guid userId, Guid groupId, string tenantId)
    {
        if (await _userRoleGroupService.ExistsAsync(userId, groupId, tenantId))
            return;

        var result = await _userRoleGroupService.AssignAsync(userId, groupId, tenantId);

        if (!result.IsSuccess)
            throw new Exception("Admin kullanıcısı gruba atanamadı.");
    }

    private async Task RunWithSeederContextAsync(AppUser user, string tenantId, Func<Task> action)
    {
        var previousContext = _httpContextAccessor.HttpContext;

        try
        {
            _httpContextAccessor.HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new[]
                        {
                            new Claim("UserId", user.Id.ToString()),
                            new Claim("TenantId", tenantId)
                        },
                        "Seeder"))
            };

            await action();
        }
        finally
        {
            _httpContextAccessor.HttpContext = previousContext;
        }
    }

    private static string GetErrors(IdentityResult result)
    {
        return string.Join(", ", result.Errors.Select(x => x.Description));
    }
}