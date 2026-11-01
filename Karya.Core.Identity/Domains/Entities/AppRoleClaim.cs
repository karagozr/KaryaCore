using Karya.Core.Interfaces.Entities;
using Microsoft.AspNetCore.Identity;

namespace Karya.Core.Identity.Domains.Entities;

public class AppRoleClaim : IdentityRoleClaim<Guid>, IBaseEntity<int>
{
}
