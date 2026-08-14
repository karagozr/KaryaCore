using Karya.Core.Interfaces.Entities.Tanent;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Karya.Core.Repositories;

public class BaseTenantRepositoryAsync<TEntity, TId, TContext> : BaseRepositoryAsync<TEntity, TId, TContext>,ITenantRepository
where TContext : DbContext
where TEntity : class,IBaseTenantEntity<TId,string>, new()
where TId : notnull
{

    public BaseTenantRepositoryAsync(TContext context, ICurrentUser currentUser) : base(context, currentUser)
    {
    }

    protected override Task BeforeCreate(TEntity entity)
    {
        entity.TenantId = _currentUser.TenantId;
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
        if((entity as IBaseTenantEntity<TId, string>).TenantId !=null && (entity as IBaseTenantEntity<TId, string>).TenantId != _currentUser.UserId)
            throw new UnauthorizedAccessException("Entity does not belong to the current tenant.");

        if ((entity as IBaseTenantEntity<TId, string>).TenantId == null)
            entity.TenantId = _currentUser.TenantId;

        return base.BeforeUpdate(entity, checkVersion, entry);
    }

    protected override Task BeforeUpdate(IEnumerable<TEntity> entities)
    {
        foreach (IBaseTenantEntity<TId, string> entity in entities)
        {
            if (entity.TenantId != null && entity.TenantId != _currentUser.UserId)
                throw new UnauthorizedAccessException("Entity does not belong to the current tenant.");

            if (entity.TenantId == null)
                entity.TenantId = _currentUser.TenantId;
        }
            

        return base.BeforeUpdate(entities);
    }
  
    public override IQueryable<TEntity> Query(Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, bool withDeleted = false, CancellationToken ct = default)
    {
        var qry = base.Query(include, withDeleted, ct).Where(e => e.TenantId == _currentUser.TenantId);

        return include == null ? qry : include(qry);
    }

}
