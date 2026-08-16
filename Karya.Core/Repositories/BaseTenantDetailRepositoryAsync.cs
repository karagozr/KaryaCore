using Karya.Core.Interfaces.Entities.Tanent;
using Karya.Core.Interfaces.Filters;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace Karya.Core.Repositories;

public class BaseTenantDetailRepositoryAsync<TEntity, TId, TParentFilter, TContext> : BaseTenantRepositoryAsync<TEntity, TId, TContext>, IDetailTenantRepository<TEntity, TId, TParentFilter>
where TContext : DbContext
where TEntity : class, IBaseTenantEntity<TId, string>, new()
where TId : notnull
where TParentFilter : IParentFilter
{

    private readonly TParentFilter _parentFilter;

    public BaseTenantDetailRepositoryAsync(TContext context, ICurrentUser currentUser, TParentFilter parentFilter) : base(context, currentUser)
    {
        _parentFilter = parentFilter;
    }

    protected override Task BeforeCreate(TEntity entity)
    {
        foreach (var item in _parentFilter.GetType().GetProperties())
        {
            var prop = entity.GetType().GetProperty(item.Name) as PropertyInfo;
            if (prop != null)
            {
                var propVal = item.GetValue(_parentFilter);
                typeof(TEntity).GetProperty(item.Name)?.SetValue(entity, propVal);
            }
                
        }
        
        entity.TenantId = _currentUser.TenantId;
        return base.BeforeCreate(entity);
    }

    protected override Task BeforeCreate(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            foreach (var item in _parentFilter.GetType().GetProperties())
            {
                entity.GetType().GetProperty(item.Name)?.SetValue(item.Name, item.GetValue(_parentFilter));
            }
           
        }

        return base.BeforeCreate(entities);
    }
   

    protected override Task BeforeUpdate(TEntity entity, bool checkVersion = false, EntityEntry<TEntity>? entry = null)
    {
        foreach (var item in _parentFilter.GetType().GetProperties())
        {
            if (entity.GetType().GetProperty(item.Name)?.GetValue(entity) != null)
                throw new UnauthorizedAccessException("Cannot change the parent field value.");
        }
        

        return base.BeforeUpdate(entity, checkVersion, entry);
    }

    protected override Task BeforeUpdate(IEnumerable<TEntity> entities)
    {
        var hasParentValue = entities.Any(e => e.GetType().GetProperties().Any(x=> _parentFilter.GetType().GetProperties().Select(s=>s.Name).Contains(x.Name)));

        if (hasParentValue)
            throw new UnauthorizedAccessException("Cannot change the parent field value.");

        return base.BeforeUpdate(entities);
    }

    protected override Task BeforeDelete(TEntity? entity)
    {
        //var parentProperty = entity.GetType().GetProperty(_parentFieldName);
        //if (parentProperty?.GetValue(entity) != null)
        //    throw new UnauthorizedAccessException("Cannot change the parent field value.");

        return base.BeforeDelete(entity);
    }

    public override IQueryable<TEntity> Query(Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, bool withDeleted = false, CancellationToken ct = default)
    {
        var qry = base.Query(include, withDeleted, ct).Where(e => e.TenantId == _currentUser.TenantId);
        
        foreach (var item in _parentFilter.GetType().GetProperties())
            if( typeof(TEntity).GetProperty(item.Name) != null)
                qry = qry.Where(e => EF.Property<object>(e, item.Name).Equals(item.GetValue(_parentFilter)));
        
        return include == null ? qry : include(qry);
    }

}
