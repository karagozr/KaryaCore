using Karya.Core.Interfaces.Entities;
using Microsoft.AspNetCore.Identity;

namespace Karya.Core.Identity.Domains.Entities;

public class AppUser : IdentityUser<Guid>, IBaseEntity<Guid>
{
    public string TenantId { get; set; } = null!;
    public bool IsSystemAdmin { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public bool IsActive { get; set; }

    public ICollection<AppUserTenant> TenantMemberships { get; set; } = new List<AppUserTenant>();

    public ICollection<AppUserRole> UserRoles { get; set; } = new List<AppUserRole>();

    public ICollection<AppUserRoleGroup> UserRoleGroups { get; set; } = new List<AppUserRoleGroup>();

    public ICollection<AppUserClaim> Claims { get; set; } = new List<AppUserClaim>();

    public ICollection<AppUserLogin> Logins { get; set; } = new List<AppUserLogin>();

    public ICollection<AppUserToken> Tokens { get; set; } = new List<AppUserToken>();
}