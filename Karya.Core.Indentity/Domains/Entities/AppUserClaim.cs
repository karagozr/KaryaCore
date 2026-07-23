using Microsoft.AspNetCore.Identity;
using Karya.Core.Interfaces.Entities;

namespace Karya.Core.Indentity.Domains.Entities;

public class AppUserClaim : IdentityUserClaim<Guid>, IBaseEntity<int>
{
    public AppUser? User { get; set; }
}
