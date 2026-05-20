using Microsoft.AspNetCore.Identity;

namespace Karya.Core.Indentity.Domains.Entities;

public class AppUser : IdentityUser<Guid>
{
    public string TenantId { get; set; }
}