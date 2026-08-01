namespace Karya.Core.Indentity.Domains.Entities;

/// <summary>
/// Join entity mapping the many-to-many relationship between role groups and roles.
/// </summary>
public class AppRoleGroupRole
{
    public Guid RoleGroupId { get; set; }

    public Guid RoleId { get; set; }

    public AppRoleGroup? RoleGroup { get; set; }

    public AppRole? Role { get; set; }
}
