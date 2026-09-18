using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Infrastructure;

/// <summary>AppRoleGroup için repository.</summary>
public class AppRoleGroupRepository : BaseTenantRepositoryAsync<AppRoleGroup, Guid, DbContext>
{
    public AppRoleGroupRepository(DbContext context, ICurrentUser currentUser) : base(context, currentUser) { }

    public Task<AppRoleGroup?> GetByNameAsync(string name, string tenantId)
        => _dbSet.FirstOrDefaultAsync(x => x.Name == name && x.TenantId == tenantId);
}
