using Karya.Core.Interfaces.Entities;
using Microsoft.AspNetCore.Identity;

namespace Karya.Core.Indentity.Domains.Entities;

public class AppUserRole : IdentityUserRole<Guid>, IBaseEntity<Guid>
{
    public Guid Id { get; set; }
}
