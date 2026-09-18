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
    private readonly AppTenantService _tenantService;
    private readonly AppUserTenantService _userTenantService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEnumerable<ICustomRoleProvider> _customRoleProviders;

    public IdentityDataSeeder(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        AppTenantService tenantService,
        AppUserTenantService userTenantService,
        IHttpContextAccessor httpContextAccessor,
        IEnumerable<ICustomRoleProvider> customRoleProviders)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _tenantService = tenantService;
        _userTenantService = userTenantService;
        _httpContextAccessor = httpContextAccessor;
        _customRoleProviders = customRoleProviders;
    }

    public async Task SeedAsync()
    {
        const string tenantId = "BASE_TENANT";
        const string adminUserName = "admin";
        const string adminEmail = "admin@mail.com";
        const string adminPassword = "Admin123*";

        await EnsureTenantAsync(tenantId);

        var roles = await EnsureRolesAsync();
        var adminUser = await EnsureAdminUserAsync(adminUserName, adminEmail, adminPassword, tenantId);

        await RunWithSeederContextAsync(adminUser, tenantId, async () =>
        {
            await _userTenantService.AssignAsync(adminUser.Id, tenantId);
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

        var customRoles = _customRoleProviders.SelectMany(x => x.GetRoles());

        var definitions = RoleProvider.GetRoles(customRoles);

        foreach (var definition in definitions)
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

    private async Task<AppUser> EnsureAdminUserAsync(string userName, string email, string password, string tenantId)
    {
        var user = await _userManager.FindByNameAsync(userName);

        if (user is not null)
            return user;

        user = new AppUser
        {
            UserName = userName,
            Email = email,
            EmailConfirmed = true,
            TenantId = tenantId,
            IsSystemAdmin = true
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            throw new Exception($"Admin kullanıcısı oluşturulamadı: {GetErrors(result)}");

        return user;
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