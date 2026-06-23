using Karya.Core.Abstracts.Entities;
using Karya.Core.Interfaces.Entities.Tanent;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Karya.Core.Repositories;

public class BaseTenantDetailRepositoryAsync<TEntity, TId, TParentId, TContext> : BaseTenantRepositoryAsync<TEntity, TId, TContext>, ITenantRepository
where TContext : DbContext
where TEntity : BaseTenantEntity<TId>, new()
where TId : notnull
{

    private readonly string _parentFieldName;
    private readonly TParentId _parentFieldValue;

    public BaseTenantDetailRepositoryAsync(TContext context, ICurrentUser currentUser, string parentFieldName, TParentId parentFieldValue) : base(context, currentUser)
    {
        _parentFieldName = parentFieldName;
        _parentFieldValue = parentFieldValue;
    }

    protected override Task BeforeCreate(TEntity entity)
    {
        entity.GetType().GetProperty(_parentFieldName)?.SetValue(entity, _parentFieldValue);
        entity.TenantId = _currentUser.TenantId;
        return base.BeforeCreate(entity);
    }

    protected override Task BeforeCreate(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.GetType().GetProperty(_parentFieldName)?.SetValue(entity, _parentFieldValue);
        }

        return base.BeforeCreate(entities);
    }
   

    protected override Task BeforeUpdate(TEntity entity, bool checkVersion = false, EntityEntry<TEntity>? entry = null)
    {
        var parentProperty = entity.GetType().GetProperty(_parentFieldName);
        if(parentProperty?.GetValue(entity) != null)
            throw new UnauthorizedAccessException("Cannot change the parent field value.");

        return base.BeforeUpdate(entity, checkVersion, entry);
    }

    protected override Task BeforeUpdate(IEnumerable<TEntity> entities)
    {
        var hasParentValue = entities.Any(e => e.GetType().GetProperty(_parentFieldName)?.GetValue(e) != null);

        if (hasParentValue)
            throw new UnauthorizedAccessException("Cannot change the parent field value.");

        return base.BeforeUpdate(entities);
    }

    protected override Task BeforeDelete(TEntity? entity)
    {
        var parentProperty = entity.GetType().GetProperty(_parentFieldName);
        if (parentProperty?.GetValue(entity) != null)
            throw new UnauthorizedAccessException("Cannot change the parent field value.");

        return base.BeforeDelete(entity);
    }

    public override IQueryable<TEntity> Query(Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, bool withDeleted = false, CancellationToken ct = default)
    {
        var qry = base.Query(include, withDeleted, ct).Where(e => e.TenantId == _currentUser.TenantId && EF.Property<TParentId>(e, _parentFieldName).Equals(_parentFieldValue));

        return include == null ? qry : include(qry);
    }

}
