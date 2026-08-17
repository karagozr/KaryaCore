using Karya.Core.Interfaces.Entities;
using Microsoft.AspNetCore.Identity;

namespace Karya.Core.Indentity.Domains.Entities;

public class AppUser : IdentityUser<Guid>, IBaseEntity<Guid>
{
    /// <summary>Kullanıcının aktif/seçili tenant'ı.</summary>
    public string TenantId { get; set; } = null!;

    /// <summary>True ise kullanıcı tüm tenant'lara erişebilen sistem adminidir.</summary>
    public bool IsSystemAdmin { get; set; }

    public string? ErpUsername { get; set; }

    public string? ErpPersonId { get; set; }

    /// <summary>Kullanıcının erişebildiği tenant üyelikleri (admin için birden çok olabilir).</summary>
    public ICollection<AppUserTenant> TenantMemberships { get; set; } = new List<AppUserTenant>();

    /// <summary>Direct role assignments for this user.</summary>
    public ICollection<AppUserRole> UserRoles { get; set; } = new List<AppUserRole>();

    /// <summary>Role group memberships for this user.</summary>
    public ICollection<AppUserRoleGroup> UserRoleGroups { get; set; } = new List<AppUserRoleGroup>();

    public ICollection<AppUserClaim> Claims { get; set; } = new List<AppUserClaim>();

    public ICollection<AppUserLogin> Logins { get; set; } = new List<AppUserLogin>();

    public ICollection<AppUserToken> Tokens { get; set; } = new List<AppUserToken>();
}