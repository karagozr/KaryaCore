using Microsoft.AspNetCore.Identity;

namespace Karya.Core.Indentity.Domains.Entities;

public class AppUserClaim : IdentityUserClaim<Guid>
{
    public AppUser? User { get; set; }
}
