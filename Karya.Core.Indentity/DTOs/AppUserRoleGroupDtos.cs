namespace Karya.Core.Indentity.DTOs;

/// <summary>Kullanıcıyı rol grubuna atama/kaldırma isteği.</summary>
public class AppUserRoleGroupAssignDto
{
    public Guid UserId { get; set; }
    public Guid RoleGroupId { get; set; }
}
