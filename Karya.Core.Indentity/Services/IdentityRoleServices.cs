using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.Infrastructure;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Results;
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

/// <summary>Kullanıcı rol yönetimi servisi.</summary>
public class AppUserRoleService : BaseService<AppUserRoleRepository, AppUserRole, Guid>
{
    public AppUserRoleService(DbContext context, ICurrentUser currentUser)
        : base(new IdentityUnitOfWork(context, currentUser)) { }

    public async Task<BaseResult> AssignAsync(Guid userId, Guid roleId, string tenantId)
    {
        var repo = _uow.Repo<AppUserRoleRepository>();

        await repo.AddAsync(new AppUserRole
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            UserId = userId,
            RoleId = roleId
        });

        return await _uow.CompleteAsync();
    }

    public Task<bool> ExistsAsync(Guid userId, Guid roleId, string tenantId)
    {
        return _uow.Repo<AppUserRoleRepository>().ExistsAsync(userId, roleId, tenantId);
    }
}

/// <summary>Kullanıcı rol grubu yönetimi servisi.</summary>
public class AppUserRoleGroupService : BaseService<AppUserRoleGroupRepository, AppUserRoleGroup, Guid>
{
    public AppUserRoleGroupService(DbContext context, ICurrentUser currentUser)
        : base(new IdentityUnitOfWork(context, currentUser)) { }
}

/// <summary>Rol grubu rol yönetimi servisi.</summary>
public class AppRoleGroupRoleService : BaseService<AppRoleGroupRoleRepository, AppRoleGroupRole, Guid>
{
    public AppRoleGroupRoleService(DbContext context, ICurrentUser currentUser)
        : base(new IdentityUnitOfWork(context, currentUser)) { }
}
