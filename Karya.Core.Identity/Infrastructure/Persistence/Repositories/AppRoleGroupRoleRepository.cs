using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Identity.Infrastructure.Persistence.Repositories;

public class AppRoleGroupRoleRepository : BaseDetailRepositoryAsync<AppRoleGroupRole, Guid, AppRoleGroupRoleParentFilter, DbContext>
{
    public AppRoleGroupRoleRepository(DbContext context, ICurrentUser currentUser, AppRoleGroupRoleParentFilter parentFilter) : base(context, currentUser, parentFilter) { }
}