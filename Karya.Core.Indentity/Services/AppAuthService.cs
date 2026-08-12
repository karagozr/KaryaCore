using Karya.Core.Indentity.Domains.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;

namespace Karya.Core.Indentity.Services;

public class AppAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly DbContext _dbContext;

    public AppAuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, DbContext dbContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _dbContext = dbContext;
    }

    public async Task<ClaimsPrincipal?> LoginAsync(string userName, string password, string tenantId)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user is null) return null;

        var passwordResult = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (!passwordResult.Succeeded) return null;

        var hasTenant = await _dbContext.Set<AppUserTenant>()
            .AnyAsync(x => x.UserId == user.Id && x.TenantId == tenantId);

        if (!hasTenant) return null;

        var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        identity.AddClaim(OpenIddictConstants.Claims.Subject, user.Id.ToString());
        identity.AddClaim("UserId", user.Id.ToString());
        identity.AddClaim("TenantId", tenantId);
        identity.AddClaim(OpenIddictConstants.Claims.Name, user.UserName ?? string.Empty);

        identity.SetDestinations(_ => new[] { OpenIddictConstants.Destinations.AccessToken });

        return new ClaimsPrincipal(identity);
    }
}