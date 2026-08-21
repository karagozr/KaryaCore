using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.DTOs;
using Karya.Core.Indentity.Infrastructure;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Interfaces.UnitOfWorks;
using Karya.Core.Results;
using Karya.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Services;

/// <summary>Rol/yetki yönetimi servisi.</summary>
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

    public async Task<AppRoleGroup> EnsureAsync(string name, string tenantId)
    {
        var repo = _uow.Repo<AppRoleGroupRepository>();

        var roleGroup = await repo.GetByNameAsync(name, tenantId);

        if (roleGroup is not null)
            return roleGroup;

        roleGroup = new AppRoleGroup
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Name = name
        };

        await repo.AddAsync(roleGroup);

        var result = await _uow.CompleteAsync();

        if (!result.IsSuccess)
            throw new Exception($"{name} role group oluşturulamadı.");

        return roleGroup;
    }
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
    public AppRoleClaimService(IUnitOfWork uow) : base(uow) { }

    public AppRoleClaimService(DbContext context, ICurrentUser currentUser)
        : base(new IdentityUnitOfWork(context, currentUser)) { }
}

/// <summary>Kullanıcıya direkt rol/yetki atama servisi.</summary>
public class AppUserRoleService : BaseService<AppUserRoleRepository, AppUserRole, Guid>
{
    public AppUserRoleService(DbContext context, ICurrentUser currentUser)
        : base(new IdentityUnitOfWork(context, currentUser)) { }

    public Task<bool> ExistsAsync(Guid userId, Guid roleId, string tenantId)
        => _uow.Repo<AppUserRoleRepository>().ExistsAsync(userId, roleId, tenantId);

    public async Task<BaseResult> AssignAsync(Guid userId, Guid roleId, string tenantId)
    {
        if (await ExistsAsync(userId, roleId, tenantId))
            return BaseResult.Success();

        await _uow.Repo<AppUserRoleRepository>().AddAsync(new AppUserRole
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            UserId = userId,
            RoleId = roleId
        });

        return await _uow.CompleteAsync();
    }
}

/// <summary>Kullanıcıyı rol grubuna atama servisi.</summary>
public class AppUserRoleGroupService : BaseService<AppUserRoleGroupRepository, AppUserRoleGroup, Guid>
{
    public AppUserRoleGroupService(DbContext context, ICurrentUser currentUser)
        : base(new IdentityUnitOfWork(context, currentUser)) { }

    public Task<bool> ExistsAsync(Guid userId, Guid roleGroupId, string tenantId)
        => _uow.Repo<AppUserRoleGroupRepository>().ExistsAsync(userId, roleGroupId, tenantId);

    public async Task<BaseResult> AssignAsync(Guid userId, Guid roleGroupId, string tenantId)
    {
        if (await ExistsAsync(userId, roleGroupId, tenantId))
            return BaseResult.Success();

        await _uow.Repo<AppUserRoleGroupRepository>().AddAsync(new AppUserRoleGroup
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            UserId = userId,
            RoleGroupId = roleGroupId
        });

        return await _uow.CompleteAsync();
    }
}

/// <summary>Rol grubuna rol/yetki atama servisi.</summary>
/// <summary>Rol grubu rol yönetimi servisi.</summary>
public class AppRoleGroupRoleService
    : BaseDetailService<AppRoleGroupRoleRepository, AppRoleGroupRole, Guid, AppRoleGroupRoleParentFilter>
{
    public AppRoleGroupRoleService(DbContext context, ICurrentUser currentUser)
        : base(new IdentityUnitOfWork(context, currentUser)) { }

    public override async Task<BaseResult<LoadResult>> Select<TDto>(AppRoleGroupRoleParentFilter parentFilter, DataSourceLoadOptionsBase filterDataOptions)
    {
        if (typeof(TDto) == typeof(AppRoleGroupRoleLDto))
        {
            var query = Query(parentFilter)
                .Select(x => new AppRoleGroupRoleLDto
                {
                    Id = x.Id,
                    RoleGroupId = x.RoleGroupId,
                    RoleId = x.RoleId,
                    RoleName = x.Role != null ? x.Role.Name : null
                });

            var res = await DataSourceLoader.LoadAsync(query, filterDataOptions);

            return BaseResult<LoadResult>.Success("200", null, res);
        }

        return await base.Select<TDto>(parentFilter, filterDataOptions);
    }

    public Task<bool> ExistsAsync(Guid roleGroupId, Guid roleId, string tenantId)
    {
        var parentFilter = new AppRoleGroupRoleParentFilter
        {
            RoleGroupId = roleGroupId
        };

        return _uow.Repo<AppRoleGroupRoleRepository>(parentFilter)
            .ExistsAsync(roleGroupId, roleId, tenantId);
    }

    public async Task<BaseResult> AssignAsync(Guid roleGroupId, Guid roleId, string tenantId)
    {
        if (await ExistsAsync(roleGroupId, roleId, tenantId))
            return BaseResult.Success();

        var parentFilter = new AppRoleGroupRoleParentFilter
        {
            RoleGroupId = roleGroupId
        };

        await _uow.Repo<AppRoleGroupRoleRepository>(parentFilter).AddAsync(new AppRoleGroupRole
        {
            Id = Guid.NewGuid(),
            RoleId = roleId,
            TenantId = tenantId
        });

        return await _uow.CompleteAsync();
    }
}