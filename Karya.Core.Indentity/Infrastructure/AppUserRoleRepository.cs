using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Infrastructure
{
    public class AppUserRoleRepository : BaseTenantRepositoryAsync<AppUserRole, Guid, DbContext>
    {
        public AppUserRoleRepository(DbContext context, ICurrentUser currentUser) : base(context, currentUser) { }

        public Task<bool> ExistsAsync(Guid userId, Guid roleId, string tenantId)
        {
            return _dbSet.AnyAsync(x => x.UserId == userId && x.RoleId == roleId && x.TenantId == tenantId);
        }
    }
}
