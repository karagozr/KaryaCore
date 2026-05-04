using Karya.Core.Abstracts.Entities;
using Karya.Core.Helpers.Generals;
using Karya.Core.Interfaces.Entities;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Karya.Core.Repositories;

public class BaseRepositoryAsync<TEntity, TId, TContext> : BaseQuery<TEntity, TId, TContext>, IRepositoryAsync<TEntity, TId>
where TContext : DbContext
where TEntity : class, IBaseEntity<TId>, new()
where TId : notnull
{
    public BaseRepositoryAsync(TContext context, ICurrentUser currentUser) : base(context, currentUser)
    {
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? expression = null, bool withDeleted = false, CancellationToken ct = default)
    {
        return expression!=null? await Query(null, withDeleted, ct).AnyAsync(expression):await Query(null, withDeleted, ct).AnyAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? expression = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, bool withDeleted = false, bool enableTracking = false, CancellationToken ct = default)
    {
        return expression != null ? await Query(null, withDeleted, ct).Where(expression).ToListAsync() : await Query(null, withDeleted, ct).ToListAsync();
    }

    public async Task<TEntity?> GetSingleAsync(Expression<Func<TEntity, bool>>? expression = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, bool withDeleted = false, bool enableTracking = false, CancellationToken ct = default)
    {
        return expression != null ? await Query(null, withDeleted, ct).FirstOrDefaultAsync(expression) : await Query(null, withDeleted, ct).FirstOrDefaultAsync();
    }

    public async Task<TEntity?> GetByIdAsync(TId id, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, CancellationToken ct=default)
    {
        return await Query(null,false, ct).FirstOrDefaultAsync(x => x.Id!.Equals(id), ct);
    }

    public virtual async Task AddAsync(TEntity entity, CancellationToken ct = default)
    {
        await BeforeCreate(entity);

        await _dbSet.AddAsync(entity);

    }

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken ct = default)
    {
        await BeforeCreate(entities);

        await _dbSet.AddRangeAsync(entities);

    }

    public virtual async Task UpdateAsync(TEntity entity, CancellationToken ct = default)
    {
        await BeforeUpdate(entity);

        _dbSet.Attach(entity);

        var entry = _context.Entry(entity);
        entry.State = EntityState.Modified;

    }

    public virtual async Task UpdateAsync(TEntity entity, string[] columns, CancellationToken ct = default)
    {
        var entry = _dbSet.Entry(entity);

        await BeforeUpdate(entity,false, entry);

        foreach (var column in columns)
        {
            if (entity.HasProperty(column))
                entry.Property(column).IsModified = true;
            else
                throw new Exception($"Column '{column}' not found in entity '{typeof(TEntity).Name}'. Please check the column name and try again.");
        }
        
    }

    public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken ct = default)
    {
        await BeforeUpdate(entities);

        _dbSet.UpdateRange(entities);
    }

    public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities, List<string> columns, CancellationToken ct = default)
    {

        foreach (var entity in entities)
        {
            var entry = _dbSet.Entry(entity);
            await BeforeUpdate(entity, false, entry);

            foreach (var column in columns)
                if (entity.HasProperty(column))
                    entry.Property(column).IsModified = true;
        }
    }

    public virtual async Task DeleteAsync(TId id, CancellationToken ct = default)
    {
        TEntity? entity = await GetByIdAsync(id, null, ct);
        
        await BeforeDelete(entity);

        if (entity is ISoftDelete)
            _dbSet.Update(entity);
        else
            _dbSet.Remove(entity);
       
    }

    public virtual async Task DeleteRangeAsync(IEnumerable<TId> ids, CancellationToken ct = default)
    {
        IQueryable<TEntity> query = Query(null, false, ct).Where(x => ids.Contains(x.Id));

        IEnumerable<TEntity> entities = await query.ToListAsync(ct);

        await BeforeDelete(entities);

        if (typeof(TEntity).IsAssignableTo(typeof(ISoftDelete)))
        {
            await query.ExecuteUpdateAsync(x => x
            .SetProperty(p => ((ISoftDelete)p).IsDeleted, _ => true)
            .SetProperty(p => ((ISoftDelete)p).DeletedBy, _ => _currentUser.UserId)
            .SetProperty(p => ((ISoftDelete)p).DeletedAt, _ => DateTime.UtcNow), ct);
        }
        else
        {
            _dbSet.RemoveRange(entities);
        }

    }

    public virtual async Task UndeleteAsync(TId id, CancellationToken ct = default)
    {
        if (!typeof(TEntity).IsAssignableTo(typeof(ISoftDelete)))
            throw new Exception("This entity does not support undelete operation. The entity must implement ISoftDelete interface to use undelete operation.");
        
        TEntity? entity = await GetByIdAsync(id, null, ct);

        if (entity == null)
            throw new Exception("Entity not found. The entity may have been deleted by another process.");


        var entry = _dbSet.Entry(entity);
        await BeforeUpdate(entity, false, entry);
        ((ISoftDelete)entity).IsDeleted = false;
        ((ISoftDelete)entity).DeletedBy = null;
        ((ISoftDelete)entity).DeletedAt = null;

        entry.Property(x => ((ISoftDelete)x).DeletedBy).IsModified = true;
        entry.Property(x => ((ISoftDelete)x).DeletedAt).IsModified = true;
        entry.Property(x => ((ISoftDelete)x).IsDeleted).IsModified = true;

    }

    public virtual async Task UndeleteRangeAsync(IEnumerable<TId> ids, CancellationToken ct = default)
    {
        if (!typeof(TEntity).IsAssignableTo(typeof(ISoftDelete)))
            throw new Exception("This entity does not support undelete operation. The entity must implement ISoftDelete interface to use undelete operation.");

        IQueryable<TEntity> query = Query(null, true, ct).Where(x => ids.Contains(x.Id));

        IEnumerable<TEntity> entities = await query.ToListAsync(ct);

        if (entities.Any() == false || entities.First() == null)
            throw new Exception("Entities not found. The entities may have been deleted by another process.");

        await query.ExecuteUpdateAsync(x => x
        .SetProperty(p => ((ISoftDelete)p).IsDeleted, _ => false)
        .SetProperty(p => ((ISoftDelete)p).DeletedBy, _ => null)
        .SetProperty(p => ((ISoftDelete)p).DeletedAt, _ => null), ct);

    }

}
