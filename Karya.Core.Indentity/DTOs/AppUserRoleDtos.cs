namespace Karya.Core.Indentity.DTOs;

/// <summary>Kullanıcıya rol atama/kaldırma isteği.</summary>
public class AppUserRoleAssignDto
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}
