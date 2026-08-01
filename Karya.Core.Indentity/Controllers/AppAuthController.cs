using Karya.Core.Indentity.Domains.Entities;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;

namespace Karya.Core.Indentity.Controllers;

[ApiController]
public class AppAuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public AppAuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("~/connect/token")]
    public async Task<IActionResult> Exchange()
    {
        var request = HttpContext.GetOpenIddictServerRequest()
            ?? throw new InvalidOperationException("OpenID Connect request cannot be retrieved.");

        if (!request.IsPasswordGrantType())
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: BuildError(OpenIddictConstants.Errors.UnsupportedGrantType,
                    "Yalnızca password grant type desteklenmektedir."));

        var user = await _userManager.FindByNameAsync(request.Username!);

        if (user is null || !(await _signInManager.CheckPasswordSignInAsync(user, request.Password!, false)).Succeeded)
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: BuildError(OpenIddictConstants.Errors.InvalidGrant,
                    "Kullanıcı adı veya şifre hatalı."));

        // TenantId doğrulaması (custom parametre olarak gönderilir)
        var tenantId = request.GetParameter("tenantId")?.ToString();

        if (string.IsNullOrWhiteSpace(tenantId) ||
            !string.Equals(user.TenantId, tenantId, StringComparison.OrdinalIgnoreCase))
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: BuildError(OpenIddictConstants.Errors.InvalidGrant,
                    "Geçersiz tenant bilgisi."));

        var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        identity.AddClaim(OpenIddictConstants.Claims.Subject, user.Id.ToString());
        identity.AddClaim(OpenIddictConstants.Claims.Name, user.UserName ?? string.Empty);
        identity.AddClaim("TenantId", user.TenantId);

        // Rol ve grup bilgileri token'a gömülmez; çağrı anında IUserClaimsService
        // üzerinden veritabanından çözülür.

        identity.SetDestinations(c => new[] { OpenIddictConstants.Destinations.AccessToken });

        return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private static AuthenticationProperties BuildError(string error, string description)
        => new(new Dictionary<string, string?>
        {
            [OpenIddictServerAspNetCoreConstants.Properties.Error] = error,
            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = description
        });
}