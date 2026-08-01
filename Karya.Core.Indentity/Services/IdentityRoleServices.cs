using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.Infrastructure;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Services;

/// <summary>Rol yönetimi servisi. Yetki (yalnızca SystemAdmin) pipeline'da uygulanır.</summary>
public class AppRoleService : BaseService<AppRoleRepository, AppRole, Guid>
{
    public AppRoleService(DbContext context, ICurrentUser currentUser)
        : base(new IdentityUnitOfWork(context, currentUser)) { }
}

/// <summary>Rol grubu yönetimi servisi.</summary>
public class AppRoleGroupService : BaseService<AppRoleGroupRepository, AppRoleGroup, Guid>
{
    public AppRoleGroupService(DbContext context, ICurrentUser currentUser)
        : base(new IdentityUnitOfWork(context, currentUser)) { }
}

/// <summary>Scope yönetimi servisi.</summary>
public class AppScopeService : BaseService<AppScopeRepository, AppScope, Guid>
{
    public AppScopeService(DbContext context, ICurrentUser currentUser)
        : base(new IdentityUnitOfWork(context, currentUser)) { }
}

/// <summary>Kullanıcı claim yönetimi servisi.</summary>
public class AppUserClaimService : BaseService<AppUserClaimRepository, AppUserClaim, int>
{
    public AppUserClaimService(DbContext context, ICurrentUser currentUser)
        : base(new IdentityUnitOfWork(context, currentUser)) { }
}

/// <summary>Rol claim yönetimi servisi.</summary>
public class AppRoleClaimService : BaseService<AppRoleClaimRepository, AppRoleClaim, int>
{
    public AppRoleClaimService(DbContext context, ICurrentUser currentUser)
        : base(new IdentityUnitOfWork(context, currentUser)) { }
}
