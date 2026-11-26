using Karya.Core.Identity.Application.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;

namespace Karya.Core.Identity.Controllers;

[ApiController]
public abstract class AppAuthController : ControllerBase
{
    private readonly IAppAuthService _authService;

    public AppAuthController(IAppAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("~/connect/token")]
    [HttpPost("~/api/auth/login")]
    [HttpPost("~/api/auth/change-tenant")]
    public async Task<IActionResult> Exchange()
    {
        var request = HttpContext.GetOpenIddictServerRequest();

        if (request == null)
            return BadRequest();

        if (request.IsRefreshTokenGrantType())
            return await RefreshAsync();

        if (!request.IsPasswordGrantType())
            return BadRequest();

        var tenantId = request.GetParameter("tenantId")?.ToString();

        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return Forbid(BuildError(OpenIddictConstants.Errors.InvalidRequest, "Username and password are required."),
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        if (string.IsNullOrWhiteSpace(tenantId))
            return Forbid(BuildError(OpenIddictConstants.Errors.InvalidRequest, "TenantId is required."),
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        ClaimsPrincipal principal;
        try
        {
            principal = await _authService.LoginAsync(request.Username, request.Password, tenantId)
                ?? throw new UnauthorizedAccessException("Invalid username, password or tenant.");
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(BuildError(OpenIddictConstants.Errors.InvalidGrant, ex.Message),
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }


    private async Task<IActionResult> RefreshAsync()
    {
        var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        if (result.Principal is null)
            return Forbid(BuildError(OpenIddictConstants.Errors.InvalidGrant, "The refresh token is no longer valid."),
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        var request = HttpContext.GetOpenIddictServerRequest();
        var tenantId = request?.GetParameter("tenantId")?.ToString();

        ClaimsPrincipal principal;
        try
        {
            principal = string.IsNullOrWhiteSpace(tenantId)
                ? await _authService.RefreshAsync(result.Principal)
                : await _authService.ChangeTenantAsync(result.Principal, tenantId);

            if (principal is null)
                throw new UnauthorizedAccessException("The refresh token is no longer valid.");
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(BuildError(OpenIddictConstants.Errors.InvalidGrant, ex.Message),
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    [HttpPost("api/auth/forgot-password")]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        await _authService.ForgotPasswordAsync(email);
        return Ok();
    }

    private static AuthenticationProperties BuildError(string error, string description) =>
        new(new Dictionary<string, string?>
        {
            [OpenIddictServerAspNetCoreConstants.Properties.Error] = error,
            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = description
        });
}
