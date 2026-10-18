using OpenIddict.EntityFrameworkCore.Models;

namespace Karya.Core.Identity.Domains.Entities;

public class AppToken : OpenIddictEntityFrameworkCoreToken<Guid, AppApplication, AppAuthorization>
{
}
