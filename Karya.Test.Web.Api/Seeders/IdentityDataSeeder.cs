using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.Infrastructure.Migrations;
using Microsoft.AspNetCore.Identity;

namespace Karya.Test.Web.Api.Seeders;

public sealed class IdentityDataSeeder : IDatabaseSeeder
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;

    public IdentityDataSeeder(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedAsync()
    {
        const string adminRoleName = "Admin";

        if (!await _roleManager.RoleExistsAsync(adminRoleName))
        {
            await _roleManager.CreateAsync(new AppRole
            {
                Id = Guid.NewGuid(),
                Name = adminRoleName
            });
        }

        const string adminEmail = "admin@mail.com";
        var adminUser = await _userManager.FindByEmailAsync(adminEmail);

        if (adminUser is not null)
            return;

        var newAdmin = new AppUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            TenantId = "DEFAULT"
        };

        var result = await _userManager.CreateAsync(newAdmin, "Admin123*");

        if (result.Succeeded)
            await _userManager.AddToRoleAsync(newAdmin, adminRoleName);
    }
}
