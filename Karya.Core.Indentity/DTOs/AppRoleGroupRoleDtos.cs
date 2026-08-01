namespace Karya.Core.Indentity.DTOs;

/// <summary>Rol grubuna rol atama/kaldırma isteği.</summary>
public class AppRoleGroupRoleAssignDto
{
    public Guid RoleGroupId { get; set; }
    public Guid RoleId { get; set; }
}
