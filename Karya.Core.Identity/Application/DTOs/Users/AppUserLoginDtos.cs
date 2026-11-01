namespace Karya.Core.Identity.Application.DTOs.Users;

/// <summary>Kullanýcý harici giriþ (external login) listeleme görünümü.</summary>
public class AppUserLoginLDto
{
    public string LoginProvider { get; set; } = null!;
    public string ProviderKey { get; set; } = null!;
    public string? ProviderDisplayName { get; set; }
    public Guid UserId { get; set; }
}
