using Karya.Core.Indentity.Domains.Entities;
using Microsoft.AspNetCore.Identity;

namespace Karya.Test.Web.Api.Seeders;

public static class IdentityDataSeeder
{
    public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();

        // 1. Önce Rolleri Kontrol Et
        string adminRoleName = "Admin";
        if (!await roleManager.RoleExistsAsync(adminRoleName))
        {
            await roleManager.CreateAsync(new AppRole
            {
                Id = Guid.NewGuid(),
                Name = adminRoleName,
            });
        }

        // 2. Örnek Kullanıcıyı Kontrol Et
        var adminEmail = "admin@mail.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            var newAdmin = new AppUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                TenantId = "DEFAULT", 
            };

            // Şifreyi hashleyerek kullanıcıyı oluştur
            var result = await userManager.CreateAsync(newAdmin, "Admin123*");

            if (result.Succeeded)
            {
                // Kullanıcıya Admin rolünü ata
                await userManager.AddToRoleAsync(newAdmin, adminRoleName);
            }
        }
    }
}
