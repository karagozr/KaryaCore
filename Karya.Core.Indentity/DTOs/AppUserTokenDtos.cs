namespace Karya.Core.Indentity.DTOs;

/// <summary>Kullanıcı token (IdentityUserToken) listeleme görünümü. Değer güvenlik gereği dahil edilmez.</summary>
public class AppUserTokenLDto
{
    public Guid UserId { get; set; }
    public string LoginProvider { get; set; } = null!;
    public string Name { get; set; } = null!;
}
