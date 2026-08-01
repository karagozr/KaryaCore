using Karya.Core.Indentity.Domains.Entities;
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
public class AppRoleGroupRepository : BaseRepositoryAsync<AppRoleGroup, Guid, DbContext>
{
    public AppRoleGroupRepository(DbContext context, ICurrentUser currentUser) : base(context, currentUser) { }
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
