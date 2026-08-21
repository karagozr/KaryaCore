using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.DTOs;
using Karya.Core.Indentity.Infrastructure.Migrations;
using Karya.Core.Indentity.Providers;
using Karya.Core.Indentity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

    public IdentityDataSeeder(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        AppRoleGroupService roleGroupService,
        AppRoleGroupRoleService roleGroupRoleService,
        AppUserRoleGroupService userRoleGroupService,
        AppTenantService tenantService,
        AppUserTenantService userTenantService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _roleGroupService = roleGroupService;
        _roleGroupRoleService = roleGroupRoleService;
        _userRoleGroupService = userRoleGroupService;
        _tenantService = tenantService;
        _userTenantService = userTenantService;
    }

    public async Task SeedAsync()
    {
        const string tenantId = "DEFAULT";
        const string adminGroupName = "Admin";
        const string adminEmail = "admin@mail.com";
        const string adminPassword = "Admin123*";

        var roles = new List<AppRole>();

        var tenantExists = await _tenantService.Query().AnyAsync(x => x.Id == tenantId);

        if (!tenantExists)
        {
            await _tenantService.Insert(new AppTenantADto
            {
                Id = tenantId,
                Name = "Default",
                Description = "Default Tenant",
                IsActive = true
            });
        }

        foreach (var definition in RoleProvider.GetRoles())
        {
            var role = await _roleManager.FindByNameAsync(definition.Name);

            if (role is null)
            {
                role = new AppRole
                {
                    Id = Guid.NewGuid(),
                    Name = definition.Name,
                    Description = definition.Description,
                };

                var roleResult = await _roleManager.CreateAsync(role);

                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(", ", roleResult.Errors.Select(x => x.Description));
                    throw new Exception($"{definition.Name} rolü oluşturulamadı: {errors}");
                }
            }

            roles.Add(role);
        }

        var adminGroup = await _roleGroupService.EnsureAsync(adminGroupName, tenantId);

        foreach (var role in roles)
        {
            var exists = await _roleGroupRoleService.ExistsAsync(adminGroup.Id, role.Id, tenantId);

            if (!exists)
                await _roleGroupRoleService.AssignAsync(adminGroup.Id, role.Id, tenantId);
        }

        var adminUser = await _userManager.FindByEmailAsync(adminEmail);

        if (adminUser is null)
        {
            adminUser = new AppUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                TenantId = tenantId,
                IsSystemAdmin = false
            };

            var userResult = await _userManager.CreateAsync(adminUser, adminPassword);

            if (!userResult.Succeeded)
            {
                var errors = string.Join(", ", userResult.Errors.Select(x => x.Description));
                throw new Exception($"Admin kullanıcısı oluşturulamadı: {errors}");
            }
        }

        await _userTenantService.AssignAsync(adminUser.Id, tenantId);

        var userGroupExists = await _userRoleGroupService.ExistsAsync(adminUser.Id, adminGroup.Id, tenantId);

        if (!userGroupExists)
            await _userRoleGroupService.AssignAsync(adminUser.Id, adminGroup.Id, tenantId);
    }
}