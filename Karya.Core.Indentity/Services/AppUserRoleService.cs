using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.Infrastructure;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Services;

/// <summary>Kullanıcıya direkt rol/yetki atama servisi.</summary>
public class AppUserRoleService : BaseService<AppUserRoleRepository, AppUserRole, Guid>
{
    public AppUserRoleService(DbContext context, ICurrentUser currentUser) : base(new IdentityUnitOfWork(context, currentUser)) { }
}
