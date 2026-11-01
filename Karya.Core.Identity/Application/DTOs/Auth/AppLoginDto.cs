namespace Karya.Core.Identity.Application.DTOs.Auth;

public class AppLoginDto
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string TenantId { get; set; } = null!;
}