using Karya.Core.Interfaces.Entities.Tanent;
using Microsoft.AspNetCore.Identity;

namespace Karya.Core.Indentity.Domains.Entities;

public class AppUserRole : IdentityUserRole<Guid>, IBaseTenantEntity<Guid, string>
{
    public Guid Id { get; set; }

    public string TenantId { get; set; } = null!;
}
