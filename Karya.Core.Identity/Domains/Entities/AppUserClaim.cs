using Karya.Core.Interfaces.Entities;
using Microsoft.AspNetCore.Identity;

namespace Karya.Core.Identity.Domains.Entities;

public class AppUserClaim : IdentityUserClaim<Guid>, IBaseEntity<int>
{
}
