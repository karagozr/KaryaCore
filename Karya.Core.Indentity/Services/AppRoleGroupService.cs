using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.Infrastructure;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Services;

/// <summary>Rol grubu yönetimi servisi.</summary>
public class AppRoleGroupService : BaseService<AppRoleGroupRepository, AppRoleGroup, Guid>
{
    public AppRoleGroupService(DbContext context, ICurrentUser currentUser) : base(new IdentityUnitOfWork(context, currentUser)) { }
}
