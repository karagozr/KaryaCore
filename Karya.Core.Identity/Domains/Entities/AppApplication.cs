using OpenIddict.EntityFrameworkCore.Models;

namespace Karya.Core.Identity.Domains.Entities;

public class AppApplication : OpenIddictEntityFrameworkCoreApplication<Guid, AppAuthorization, AppToken>
{
}
