namespace Karya.Core.Identity.Application.DTOs.Users;

/// <summary>Kullanýcý token (IdentityUserToken) listeleme görünümü. Deðer güvenlik gereði dahil edilmez.</summary>
public class AppUserTokenLDto
{
    public Guid UserId { get; set; }
    public string LoginProvider { get; set; } = null!;
    public string Name { get; set; } = null!;
}
