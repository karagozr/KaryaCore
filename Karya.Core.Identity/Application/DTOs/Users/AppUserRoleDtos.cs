namespace Karya.Core.Identity.Application.DTOs.Users;

/// <summary>Kullanýcýya rol atama/kaldýrma isteði.</summary>
public class AppUserRoleAssignDto
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}
