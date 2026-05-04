using Karya.Core.Helpers.Repository;
using Karya.Core.Interfaces.Entities;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Karya.Core.Repositories;

public abstract class BaseQuery<TEntity, TId, TContext> : IQuery<TEntity, TId>
where TContext : DbContext
where TEntity : class, IBaseEntity<TId>, new()
{
    protected readonly TContext _context;
    protected readonly DbSet<TEntity> _dbSet;
    protected readonly ICurrentUser _currentUser;

    public BaseQuery(TContext context, ICurrentUser currentUser)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
        _currentUser = currentUser;
    }

    public virtual IQueryable<TEntity> Query(Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, bool withDeleted = false, CancellationToken ct = default)
    {
        var qry = _dbSet.AsQueryable();

        if (typeof(TEntity).IsAssignableTo(typeof(ISoftDelete)))
            qry = withDeleted ? _dbSet.AsQueryable() : _dbSet.Where(x => ((ISoftDelete)x).IsDeleted == false).AsQueryable();

        return include == null ? qry : include(qry);
    }

    protected virtual async Task BeforeCreate(TEntity entity)
    {
        if (entity is IVersionable)
            RepositoryLogHelper.AtCreate((IVersionable)entity, _currentUser.UserId);
    }

    protected virtual async Task BeforeCreate(IEnumerable<TEntity> entities)
    {
        if (entities.First() is IVersionable)
        {
            foreach (var entity in entities)
            {
                RepositoryLogHelper.AtCreate((IVersionable)entity, _currentUser.UserId);
            }
        }
    }

    protected virtual async Task BeforeUpdate(TEntity entity, bool checkVersion = false, EntityEntry<TEntity>? entry = null)
    {
        if (entity is IVersionable)
        {
            if (checkVersion)
            {
                TEntity? oldEntity = await Query(null, false).AsNoTracking().FirstOrDefaultAsync(x => x.Id!.Equals(entity.Id));
                RepositoryLogHelper.VersionControl(oldEntity as IVersionable, entity as IVersionable);
            }

            RepositoryLogHelper.AtUpdate((IVersionable)entity, _currentUser.UserId);

            if (entry != null)
            {
                entry.Property(x => ((IVersionable)x).UpdatedAt).IsModified = true;
                entry.Property(x => ((IVersionable)x).UpdatedBy).IsModified = true;
                entry.Property(x => ((IVersionable)x).Version).IsModified = true;
            }
        }
    }

    protected virtual async Task BeforeUpdate(IEnumerable<TEntity> entities)
    {
        if (entities.First() is IVersionable)
        {
            IEnumerable<TEntity> oldEntities = await Query(null, false).Where(x => entities.Select(e => e.Id).Contains(x.Id)).ToListAsync();

            foreach (var entity in entities)
            {
                var oldEntity = oldEntities.FirstOrDefault(x => x.Id!.Equals(entity.Id));
                RepositoryLogHelper.VersionControl(oldEntity as IVersionable, entity as IVersionable);
                RepositoryLogHelper.AtUpdate((IVersionable)entity, _currentUser.UserId);
            }
        }
    }

    protected virtual async Task BeforeDelete(TEntity? entity)
    {
        if (entity == null)
            throw new Exception("Entity not found. The entity may have been deleted by another process.");

        if (entity is ISoftDelete)
        {
            if (((ISoftDelete)entity).IsDeleted == true)
                throw new Exception("Entity not found. The entity may have been deleted by another process.");

            RepositoryLogHelper.AtDelete((ISoftDelete)entity, _currentUser.UserId);
        }
    }

    protected virtual async Task BeforeDelete(IEnumerable<TEntity>? entities)
    {
        if (entities.Any() == false || entities.First() == null)
            throw new Exception("Entities not found. The entities may have been deleted by another process.");
    }
}


