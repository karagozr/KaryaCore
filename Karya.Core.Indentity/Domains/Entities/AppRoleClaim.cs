using Microsoft.AspNetCore.Identity;
using Karya.Core.Interfaces.Entities;

namespace Karya.Core.Indentity.Domains.Entities;

public class AppRoleClaim : IdentityRoleClaim<Guid>, IBaseEntity<int>
{
    public AppRole? Role { get; set; }
}
