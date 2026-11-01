using OpenIddict.EntityFrameworkCore.Models;
using Karya.Core.Interfaces.Entities;

namespace Karya.Core.Identity.Domains.Entities;

public class AppScope : OpenIddictEntityFrameworkCoreScope<Guid>, IBaseEntity<Guid>
{
}
