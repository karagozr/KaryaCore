namespace Karya.Core.Indentity.DTOs;

/// <summary>Kullanıcıya tenant üyeliği atama/kaldırma isteği.</summary>
public class AppUserTenantAssignDto
{
    public Guid UserId { get; set; }
    public string TenantId { get; set; } = null!;
}
