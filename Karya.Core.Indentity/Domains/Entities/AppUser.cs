using Microsoft.AspNetCore.Identity;

namespace Karya.Core.Indentity.Domains.Entities;

public class AppUser : IdentityUser<Guid>
{
    public string TenantId { get; set; }

    /// <summary>Direct role assignments for this user.</summary>
    public ICollection<AppUserRole> UserRoles { get; set; } = new List<AppUserRole>();

    /// <summary>Role group memberships for this user.</summary>
    public ICollection<AppUserRoleGroup> UserRoleGroups { get; set; } = new List<AppUserRoleGroup>();

    public ICollection<AppUserClaim> Claims { get; set; } = new List<AppUserClaim>();

    public ICollection<AppUserLogin> Logins { get; set; } = new List<AppUserLogin>();

    public ICollection<AppUserToken> Tokens { get; set; } = new List<AppUserToken>();
}