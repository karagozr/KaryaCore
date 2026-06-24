using Karya.Core.Interfaces.Entities;
using System.Linq.Expressions;
namespace Karya.Core.Interfaces.Repositories;


public interface IRepository
{

}

public interface ITenantRepository
{

}

public interface IDetailTenantRepository
{

}
public interface IRepositoryAsync<TEntity,TId> : IRepository, ITenantRepository, IDetailTenantRepository, IQuery<TEntity, TId> where TEntity : IBaseEntity<TId>, new()
{
    Task<TEntity?> GetByIdAsync(TId id, 
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, 
        CancellationToken ct = default);

    Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>>? expression = null,
        bool withDeleted = false,
        CancellationToken ct = default);

    Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? expression = null, 
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken ct = default);

    Task<TEntity> GetSingleAsync(
        Expression<Func<TEntity, bool>>? expression = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken ct = default);

    Task AddAsync(TEntity entity, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken ct = default);
    Task UpdateAsync(TEntity entity, CancellationToken ct = default);
    Task UpdateAsync(TEntity entity, string[] columns, CancellationToken ct = default);
    Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken ct = default);
    Task DeleteAsync(TId id, CancellationToken ct = default);
    Task DeleteRangeAsync(IEnumerable<TId> ids, CancellationToken ct = default);
}
