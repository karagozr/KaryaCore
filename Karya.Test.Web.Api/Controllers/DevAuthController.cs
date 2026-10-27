using Karya.Core.Identity.Application.Services;
using Karya.Core.Identity.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Karya.Test.Web.Api.Controllers;

public class DevAuthController : AppAuthController
{
    public DevAuthController(IAppAuthService authService) : base(authService)
    {
    }

    public record Office365LoginDto(string IdToken, string TenantId);

    [AllowAnonymous]
    [HttpPost("~/api/auth/office365")]
    public IActionResult Office365Login([FromBody] Office365LoginDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.IdToken) || string.IsNullOrWhiteSpace(dto.TenantId))
            return BadRequest(new { error = "invalid_request", error_description = "IdToken and TenantId are required." });

        // Mock: Office 365 doğrulaması yapılmış varsayılır.
        return Ok(new
        {
            access_token = "mock-access-token",
            token_type = "Bearer",
            expires_in = 900,
            refresh_token = "mock-refresh-token",
            user = new
            {
                id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                userName = "mock.user@contoso.onmicrosoft.com",
                email = "mock.user@contoso.onmicrosoft.com",
                firstName = "Mock",
                lastName = "User",
                tenantId = dto.TenantId,
                provider = "Office365"
            }
        });
    }
}
