using Microsoft.AspNetCore.Identity;

namespace Karya.Core.Indentity.Domains.Entities;

public class AppRole : IdentityRole<Guid>
{
    /// <summary>Users directly assigned to this role.</summary>
    public ICollection<AppUserRole> UserRoles { get; set; } = new List<AppUserRole>();

    /// <summary>Role groups that include this role.</summary>
    public ICollection<AppRoleGroupRole> RoleGroupRoles { get; set; } = new List<AppRoleGroupRole>();

    public ICollection<AppRoleClaim> RoleClaims { get; set; } = new List<AppRoleClaim>();
}
