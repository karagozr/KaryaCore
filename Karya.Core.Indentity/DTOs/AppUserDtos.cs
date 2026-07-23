using Karya.Core.Interfaces.DTOs;

namespace Karya.Core.Indentity.DTOs;

/// <summary>Tekil kayıt görünümü.</summary>
public class AppUserSDto : ISingleDto
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string TenantId { get; set; } = null!;
    public bool IsSystemAdmin { get; set; }
}

/// <summary>Liste görünümü.</summary>
public class AppUserLDto : ISelectDto
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string TenantId { get; set; } = null!;
    public bool IsSystemAdmin { get; set; }
}

/// <summary>Kullanıcı ekleme.</summary>
public class AppUserADto : IInsertDto
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string Password { get; set; } = null!;
    public bool IsSystemAdmin { get; set; }
}

/// <summary>Kullanıcı güncelleme.</summary>
public class AppUserUDto : IUpdateDto
{
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsSystemAdmin { get; set; }
}
