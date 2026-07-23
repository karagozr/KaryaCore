using OpenIddict.EntityFrameworkCore.Models;
using Karya.Core.Interfaces.Entities;

namespace Karya.Core.Indentity.Domains.Entities;

public class AppScope : OpenIddictEntityFrameworkCoreScope<Guid>, IBaseEntity<Guid>
{
}
