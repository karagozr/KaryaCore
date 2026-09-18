using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.Infrastructure;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Services;

/// <summary>Kullanıcıyı rol grubuna atama servisi.</summary>
public class AppUserRoleGroupService : BaseService<AppUserRoleGroupRepository, AppUserRoleGroup, Guid>
{
    public AppUserRoleGroupService(DbContext context, ICurrentUser currentUser) : base(new IdentityUnitOfWork(context, currentUser)) { }
}
