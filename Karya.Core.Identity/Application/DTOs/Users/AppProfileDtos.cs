namespace Karya.Core.Identity.Application.DTOs.Users;

/// <summary>Oturumdaki kullanıcının profil görünümü.</summary>
public class AppProfileDto
{
    public AppProfileUserDto User { get; set; } = null!;

    /// <summary>Profil claim'leri (ör. theme, language).</summary>
    public Dictionary<string, string?> Claims { get; set; } = new();
}

/// <summary>Profilde gösterilen kullanıcı bilgileri (salt okunur).</summary>
public class AppProfileUserDto
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string TenantId { get; set; } = null!;
    public bool IsSystemAdmin { get; set; }
    public string? ErpPersonId { get; set; }
    public string? ErpUsername { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>Profil claim güncelleme. Değeri null/boş olan claim silinir.</summary>
public class AppProfileUDto
{
    public Dictionary<string, string?> Claims { get; set; } = new();
}
