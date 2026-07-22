using OpenIddict.EntityFrameworkCore.Models;

namespace Karya.Core.Indentity.Domains.Entities;

public class AppApplication : OpenIddictEntityFrameworkCoreApplication<Guid, AppAuthorization, AppToken>
{
}
