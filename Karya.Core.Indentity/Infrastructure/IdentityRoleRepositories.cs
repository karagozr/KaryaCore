using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.DTOs;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Infrastructure;

/// <summary>AppRole için repository.</summary>
public class AppRoleRepository : BaseRepositoryAsync<AppRole, Guid, DbContext>
{
    public AppRoleRepository(DbContext context, ICurrentUser currentUser) : base(context, currentUser) { }
}

/// <summary>AppRoleGroup için repository.</summary>
public class AppRoleGroupRepository : BaseTenantRepositoryAsync<AppRoleGroup, Guid, DbContext>
{
    public AppRoleGroupRepository(DbContext context, ICurrentUser currentUser) : base(context, currentUser) { }

    public Task<AppRoleGroup?> GetByNameAsync(string name, string tenantId)
        => _dbSet.FirstOrDefaultAsync(x => x.Name == name && x.TenantId == tenantId);
}

/// <summary>AppScope için repository.</summary>
public class AppScopeRepository : BaseRepositoryAsync<AppScope, Guid, DbContext>
{
    public AppScopeRepository(DbContext context, ICurrentUser currentUser) : base(context, currentUser) { }
}

/// <summary>AppUserClaim için repository.</summary>
public class AppUserClaimRepository : BaseRepositoryAsync<AppUserClaim, int, DbContext>
{
    public AppUserClaimRepository(DbContext context, ICurrentUser currentUser) : base(context, currentUser) { }
}

/// <summary>AppRoleClaim için repository.</summary>
public class AppRoleClaimRepository : BaseRepositoryAsync<AppRoleClaim, int, DbContext>
{
    public AppRoleClaimRepository(DbContext context, ICurrentUser currentUser) : base(context, currentUser) { }
}

/// <summary>AppUserRole için repository.</summary>
public class AppUserRoleRepository : BaseTenantRepositoryAsync<AppUserRole, Guid, DbContext>
{
    public AppUserRoleRepository(DbContext context, ICurrentUser currentUser) : base(context, currentUser) { }

    public Task<bool> ExistsAsync(Guid userId, Guid roleId, string tenantId)
    {
        return _dbSet.AnyAsync(x => x.UserId == userId && x.RoleId == roleId && x.TenantId == tenantId);
    }
}

/// <summary>AppUserRoleGroup için repository.</summary>
public class AppUserRoleGroupRepository : BaseTenantRepositoryAsync<AppUserRoleGroup, Guid, DbContext>
{
    public AppUserRoleGroupRepository(DbContext context, ICurrentUser currentUser) : base(context, currentUser) { }

    public Task<bool> ExistsAsync(Guid userId, Guid roleGroupId, string tenantId)
        => _dbSet.AnyAsync(x => x.UserId == userId && x.RoleGroupId == roleGroupId && x.TenantId == tenantId);
}

/// <summary>AppRoleGroupRole için repository.</summary>
public class AppRoleGroupRoleRepository : BaseTenantDetailRepositoryAsync<AppRoleGroupRole, Guid, AppRoleGroupRoleParentFilter, DbContext>
{
    public AppRoleGroupRoleRepository(DbContext context, ICurrentUser currentUser, AppRoleGroupRoleParentFilter parentFilter) : base(context, currentUser, parentFilter) { }

    public Task<bool> ExistsAsync(Guid roleGroupId, Guid roleId, string tenantId)
        => _dbSet.AnyAsync(x => x.RoleGroupId == roleGroupId && x.RoleId == roleId && x.TenantId == tenantId);
}