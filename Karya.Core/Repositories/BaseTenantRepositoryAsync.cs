using Karya.Core.Interfaces.Entities.Tanent;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Karya.Core.Repositories;

public class BaseTenantRepositoryAsync<TEntity, TId, TContext> : BaseRepositoryAsync<TEntity, TId, TContext>, ITenantRepository<TEntity, TId>
where TContext : DbContext
where TEntity : class, IBaseTenantEntity<TId, string>, new()
where TId : notnull
{

    public BaseTenantRepositoryAsync(TContext context, ICurrentUser currentUser) : base(context, currentUser)
    {
    }

    protected override Task BeforeCreate(TEntity entity)
    {
        if (string.IsNullOrEmpty(entity.TenantId))
            entity.TenantId = _currentUser.TenantId;
        else if (!string.IsNullOrEmpty(_currentUser.TenantId) && entity.TenantId != _currentUser.TenantId)
            throw new UnauthorizedAccessException("Entity does not belong to the current tenant.");

        return base.BeforeCreate(entity);
    }

    protected override Task BeforeCreate(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
            entity.TenantId = _currentUser.TenantId;

        return base.BeforeCreate(entities);
    }

    protected override Task BeforeUpdate(TEntity entity, bool checkVersion = false, EntityEntry<TEntity>? entry = null)
    {
        if (!string.IsNullOrEmpty(entity.TenantId) && entity.TenantId != _currentUser.TenantId)
            throw new UnauthorizedAccessException("Entity does not belong to the current tenant.");

        if (string.IsNullOrEmpty(entity.TenantId))
            entity.TenantId = _currentUser.TenantId;

        return base.BeforeUpdate(entity, checkVersion, entry);
    }

    protected override Task BeforeUpdate(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            if (!string.IsNullOrEmpty(entity.TenantId) && entity.TenantId != _currentUser.TenantId)
                throw new UnauthorizedAccessException("Entity does not belong to the current tenant.");

            if (string.IsNullOrEmpty(entity.TenantId))
                entity.TenantId = _currentUser.TenantId;
        }

        return base.BeforeUpdate(entities);
    }

    public override IQueryable<TEntity> Query(Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, bool withDeleted = false, CancellationToken ct = default)
    {
        var qry = base.Query(include, withDeleted, ct).Where(e => e.TenantId == _currentUser.TenantId);

        return include == null ? qry : include(qry);
    }

    public override async Task UpdateAsync(TEntity entity, string[] columns, CancellationToken ct = default)
    {
        if (!string.IsNullOrEmpty(entity.TenantId) && entity.TenantId != _currentUser.TenantId)
            throw new UnauthorizedAccessException("Entity does not belong to the current tenant.");

        entity.TenantId = _currentUser.TenantId;

        await base.UpdateAsync(entity, columns, ct);
    }

}
