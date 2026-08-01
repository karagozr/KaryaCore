namespace Karya.Core.Indentity.DTOs;

/// <summary>Kullanıcı harici giriş (external login) listeleme görünümü.</summary>
public class AppUserLoginLDto
{
    public string LoginProvider { get; set; } = null!;
    public string ProviderKey { get; set; } = null!;
    public string? ProviderDisplayName { get; set; }
    public Guid UserId { get; set; }
}
