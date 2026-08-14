using Karya.Core.Abstracts.Entities;

namespace Karya.Core.Indentity.Domains.Entities;

/// <summary>
/// Join entity mapping the many-to-many relationship between users and role groups.
/// A user can be a member of multiple role groups.
/// </summary>
public class AppUserRoleGroup : BaseTenantEntity<Guid>
{
    public Guid UserId { get; set; }

    public Guid RoleGroupId { get; set; }

    public AppUser? User { get; set; }

    public AppRoleGroup? RoleGroup { get; set; }
}
