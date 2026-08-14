using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.Infrastructure.Migrations;
using Karya.Core.Indentity.Services;
using Microsoft.AspNetCore.Identity;

namespace Karya.Core.Indentity.Seeders;

public sealed class IdentityDataSeeder : IDatabaseSeeder
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly AppUserRoleService _userRoleService;
    private readonly AppUserTenantService _userTenantService;

    public IdentityDataSeeder(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, AppUserRoleService userRoleService, AppUserTenantService userTenantService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _userRoleService = userRoleService;
        _userTenantService = userTenantService;
    }

    public async Task SeedAsync()
    {
        const string adminRoleName = "Admin";
        const string adminEmail = "admin@mail.com";
        const string adminPassword = "Admin123*";
        const string tenantId = "DEFAULT";

        var adminRole = await _roleManager.FindByNameAsync(adminRoleName);

        if (adminRole is null)
        {
            adminRole = new AppRole
            {
                Id = Guid.NewGuid(),
                Name = adminRoleName
            };

            var roleResult = await _roleManager.CreateAsync(adminRole);

            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(x => x.Description));
                throw new Exception($"Admin rolü oluşturulamadı: {errors}");
            }
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
                IsSystemAdmin = true
            };

            var userResult = await _userManager.CreateAsync(adminUser, adminPassword);

            if (!userResult.Succeeded)
            {
                var errors = string.Join(", ", userResult.Errors.Select(x => x.Description));
                throw new Exception($"Admin kullanıcısı oluşturulamadı: {errors}");
            }
        }

        await _userTenantService.AssignAsync(adminUser.Id, tenantId);

        var userRoleExists = await _userRoleService.ExistsAsync(adminUser.Id, adminRole.Id, tenantId);

        if (!userRoleExists)
            await _userRoleService.AssignAsync(adminUser.Id, adminRole.Id, tenantId);
    }
}