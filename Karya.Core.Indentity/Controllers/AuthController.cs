using Karya.Core.Indentity.Domains.Entities;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;

namespace Karya.Core.Indentity.Controllers;

[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("~/connect/token")]
    public async Task<IActionResult> Exchange()
    {
        var request = HttpContext.GetOpenIddictServerRequest();
        var user = await _userManager.FindByNameAsync(request.Username);

        if (user == null || !(await _signInManager.CheckPasswordSignInAsync(user, request.Password, false)).Succeeded)
            return Unauthorized();

        var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        identity.AddClaim(OpenIddictConstants.Claims.Subject, user.Id.ToString());
        identity.AddClaim("TenantId", user.TenantId); // Senin özel alanın

        // Rol ve grup bilgileri token'a gömülmez; çağrı anında IUserClaimsService
        // üzerinden veritabanından çözülür.

        identity.SetDestinations(c => new[] { OpenIddictConstants.Destinations.AccessToken });

        return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}