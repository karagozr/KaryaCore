using OpenIddict.EntityFrameworkCore.Models;

namespace Karya.Core.Identity.Domains.Entities;

public class AppAuthorization : OpenIddictEntityFrameworkCoreAuthorization<Guid, AppApplication, AppToken>
{
}
