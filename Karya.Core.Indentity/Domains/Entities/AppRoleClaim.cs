using Microsoft.AspNetCore.Identity;

namespace Karya.Core.Indentity.Domains.Entities;

public class AppRoleClaim : IdentityRoleClaim<Guid>
{
    public AppRole? Role { get; set; }
}
