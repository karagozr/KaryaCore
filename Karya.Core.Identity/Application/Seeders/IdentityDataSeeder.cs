using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Identity.Application.Seeders;
using Karya.Core.Identity.Application.Services;
using Karya.Core.Identity.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Karya.Core.Identity.Application.Seeders;

public class IdentityDataSeeder : IDatabaseSeeder
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly AppTenantService _tenantService;
    private readonly AppUserTenantService _userTenantService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEnumerable<ICustomRoleProvider> _customRoleProviders;
    private readonly IIdentityEntityFactory _entityFactory;
    private readonly KaryaSeedOptions _seedOptions;

    public IdentityDataSeeder(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        AppTenantService tenantService,
        AppUserTenantService userTenantService,
        IHttpContextAccessor httpContextAccessor,
        IEnumerable<ICustomRoleProvider> customRoleProviders,
        IIdentityEntityFactory entityFactory,
        IOptions<CoreIdentityOptions> options)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _tenantService = tenantService;
        _userTenantService = userTenantService;
        _httpContextAccessor = httpContextAccessor;
        _customRoleProviders = customRoleProviders;
        _entityFactory = entityFactory;
        _seedOptions = options.Value.Seed;
    }

    public virtual async Task SeedAsync()
    {
        if (!_seedOptions.Enabled)
            return;

        var tenantId = _seedOptions.TenantId;

        await EnsureTenantAsync(tenantId);

        await EnsureRolesAsync();
        var adminUser = await EnsureAdminUserAsync(_seedOptions.AdminUserName, _seedOptions.AdminEmail, _seedOptions.AdminPassword, tenantId);

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
            Name = _seedOptions.TenantName,
            Description = "Default Tenant",
            IsActive = true
        });
    }

    protected virtual async Task<List<AppRole>> EnsureRolesAsync()
    {
        var roles = new List<AppRole>();

        var customRoles = _customRoleProviders.SelectMany(x => x.GetRoles());

        var definitions = RoleProvider.GetRoles(customRoles);

        foreach (var definition in definitions)
        {
            var role = await _roleManager.FindByNameAsync(definition.Name);

            if (role is null)
            {
                role = _entityFactory.CreateRole();
                role.Id = Guid.NewGuid();
                role.Name = definition.Name;
                role.Description = definition.Description;

                var result = await _roleManager.CreateAsync(role);

                if (!result.Succeeded)
                    throw new Exception($"{definition.Name} rol� olu�turulamad�: {GetErrors(result)}");
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

        user = _entityFactory.CreateUser();
        user.UserName = userName;
        user.Email = email;
        user.EmailConfirmed = true;
        user.TenantId = tenantId;
        user.IsSystemAdmin = true;
        user.IsActive = true;

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            throw new Exception($"Admin kullan�c�s� olu�turulamad�: {GetErrors(result)}");

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
