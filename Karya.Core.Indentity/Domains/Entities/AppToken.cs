using OpenIddict.EntityFrameworkCore.Models;

namespace Karya.Core.Indentity.Domains.Entities;

public class AppToken : OpenIddictEntityFrameworkCoreToken<Guid, AppApplication, AppAuthorization>
{
}
