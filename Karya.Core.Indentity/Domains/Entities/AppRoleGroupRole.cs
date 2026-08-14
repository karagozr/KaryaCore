using Karya.Core.Abstracts.Entities;

namespace Karya.Core.Indentity.Domains.Entities;

/// <summary>
/// Join entity mapping the many-to-many relationship between role groups and roles.
/// </summary>
public class AppRoleGroupRole : BaseTenantEntity<Guid>
{

    public Guid RoleGroupId { get; set; }

    public Guid RoleId { get; set; }

    public AppRoleGroup? RoleGroup { get; set; }

    public AppRole? Role { get; set; }
    public string TenantId { get; set; }
    public Guid Id { get; set; }
}
