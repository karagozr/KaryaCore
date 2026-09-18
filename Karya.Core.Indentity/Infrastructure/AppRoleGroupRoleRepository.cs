using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.DTOs;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Infrastructure;

public class AppRoleGroupRoleRepository : BaseTenantDetailRepositoryAsync<AppRoleGroupRole, Guid, AppRoleGroupRoleParentFilter, DbContext>
{
    public AppRoleGroupRoleRepository(DbContext context, ICurrentUser currentUser, AppRoleGroupRoleParentFilter parentFilter) : base(context, currentUser, parentFilter) { }

    public Task<bool> ExistsAsync(Guid roleGroupId, Guid roleId, string tenantId)
        => _dbSet.AnyAsync(x => x.RoleGroupId == roleGroupId && x.RoleId == roleId && x.TenantId == tenantId);
}