namespace Karya.Core.Identity.Application.DTOs.Users;

public class AppProfileDto
{
    public AppBasicProfileUserDto User { get; set; } = null!;

    public Dictionary<string, string?> Claims { get; set; } = new();
}

public class AppBasicProfileUserDto
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public string TenantId { get; set; } = null!;
    public string? TenantName { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FullName => $"{FirstName} {LastName}".Trim();
}

public class AppProfileUserDto
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string TenantId { get; set; } = null!;
    public string? TenantName { get; set; } = null!;
    public bool IsSystemAdmin { get; set; }
    public string? ErpPersonId { get; set; }
    public string? ErpUsername { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FullName => $"{FirstName} {LastName}".Trim();
    public bool IsActive { get; set; }
}

/// <summary>Profil claim güncelleme. Değeri null/boş olan claim silinir.</summary>
public class AppProfileUDto
{
    public Dictionary<string, string?> Claims { get; set; } = new();
}

/// <summary>Profil claim'i (key-value).</summary>
public class AppProfileClaimDto
{
    public string Key { get; set; } = null!;
    public string? Value { get; set; }
}
