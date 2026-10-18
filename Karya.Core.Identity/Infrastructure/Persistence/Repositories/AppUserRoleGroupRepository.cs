using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Identity.Infrastructure.Persistence.Repositories;

/// <summary>AppUserRoleGroup için repository.</summary>
public class AppUserRoleGroupRepository : BaseTenantRepositoryAsync<AppUserRoleGroup, Guid, DbContext>
{
    public AppUserRoleGroupRepository(DbContext context, ICurrentUser currentUser) : base(context, currentUser) { }

    public Task<bool> ExistsAsync(Guid userId, Guid roleGroupId, string tenantId)
        => _dbSet.AnyAsync(x => x.UserId == userId && x.RoleGroupId == roleGroupId && x.TenantId == tenantId);
}
