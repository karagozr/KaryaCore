using Karya.Core.Abstracts.Entities;
using Karya.Core.Interfaces.Entities.Tanent;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Karya.Core.Repositories;

public class BaseTanentRepositoryAsync<TEntity, TId, TContext> : BaseRepositoryAsync<TEntity, TId, TContext>,ITanentRepository
where TContext : DbContext
where TEntity : BaseTanentEntity<TId>, new()
where TId : notnull
{

    public BaseTanentRepositoryAsync(TContext context, ICurrentUser currentUser) : base(context, currentUser)
    {
    }

    protected override Task BeforeCreate(TEntity entity)
    {
        entity.TanentId = _currentUser.UserId;
        return base.BeforeCreate(entity);
    }

    protected override Task BeforeCreate(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
            entity.TanentId = _currentUser.UserId;

        return base.BeforeCreate(entities);
    }

    protected override Task BeforeUpdate(TEntity entity, bool checkVersion = false, EntityEntry<TEntity>? entry = null)
    {
        if((entity as IBaseTanentEntity<TId, string>).TanentId !=null && (entity as IBaseTanentEntity<TId, string>).TanentId != _currentUser.UserId)
            throw new UnauthorizedAccessException("Entity does not belong to the current tenant.");

        if ((entity as IBaseTanentEntity<TId, string>).TanentId == null)
            entity.TanentId = _currentUser.UserId;

        return base.BeforeUpdate(entity, checkVersion, entry);
    }

    protected override Task BeforeUpdate(IEnumerable<TEntity> entities)
    {
        foreach (IBaseTanentEntity<TId, string> entity in entities)
        {
            if (entity.TanentId != null && entity.TanentId != _currentUser.UserId)
                throw new UnauthorizedAccessException("Entity does not belong to the current tenant.");

            if (entity.TanentId == null)
                entity.TanentId = _currentUser.UserId;
        }
            

        return base.BeforeUpdate(entities);
    }
  
    public override IQueryable<TEntity> Query(Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, bool withDeleted = false, CancellationToken ct = default)
    {
        var qry = base.Query(include, withDeleted, ct).Where(e => e.TanentId == _currentUser.UserId);

        return include == null ? qry : include(qry);
    }

}
