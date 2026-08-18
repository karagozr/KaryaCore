using Karya.Core.Indentity.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace Karya.Core.Indentity.Controllers;

[ApiController]
public abstract class AppAuthController : ControllerBase
{
    private readonly AppAuthService _authService;

    public AppAuthController(AppAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("~/connect/token")]
    [HttpPost("~/api/auth/login")]
    public async Task<IActionResult> Exchange()
    {
        var request = HttpContext.GetOpenIddictServerRequest();

        if (request == null || !request.IsPasswordGrantType())
            return BadRequest();

        var tenantId = request.GetParameter("tenantId")?.ToString();

        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return Forbid(BuildError(OpenIddictConstants.Errors.InvalidRequest, "Username and password are required."),
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        if (string.IsNullOrWhiteSpace(tenantId))
            return Forbid(BuildError(OpenIddictConstants.Errors.InvalidRequest, "TenantId is required."),
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        var principal = await _authService.LoginAsync(request.Username, request.Password, tenantId);

        if (principal == null)
            return Forbid(BuildError(OpenIddictConstants.Errors.InvalidGrant, "Invalid username, password or tenant."),
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private static AuthenticationProperties BuildError(string error, string description) =>
        new(new Dictionary<string, string?>
        {
            [OpenIddictServerAspNetCoreConstants.Properties.Error] = error,
            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = description
        });
}