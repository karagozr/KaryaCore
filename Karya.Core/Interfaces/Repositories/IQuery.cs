using Karya.Core.Interfaces.Entities;

namespace Karya.Core.Interfaces.Repositories;

public interface IQuery<TEntity, TId> where TEntity : IBaseEntity<TId>, new()
{
    IQueryable<TEntity> Query( Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, bool withDeleted = false, CancellationToken ct = default);
}
