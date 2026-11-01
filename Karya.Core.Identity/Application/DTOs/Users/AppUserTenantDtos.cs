namespace Karya.Core.Identity.Application.DTOs.Users;

/// <summary>Kullanýcýya tenant üyeliði atama/kaldýrma isteði.</summary>
public class AppUserTenantAssignDto
{
    public Guid UserId { get; set; }
    public string TenantId { get; set; } = null!;
}
