using Karya.Core.Abstracts.Entities;

namespace Karya.Core.Indentity.Domains.Entities;

/// <summary>
/// A named collection of roles that can be assigned to users as a group.
/// A user can belong to multiple role groups, and roles are resolved from
/// both direct role assignments and role group memberships.
/// </summary>
public class AppRoleGroup : BaseTenantEntity<Guid>
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public ICollection<AppRoleGroupRole> RoleGroupRoles { get; set; } = new List<AppRoleGroupRole>();

    public ICollection<AppUserRoleGroup> UserRoleGroups { get; set; } = new List<AppUserRoleGroup>();
}
