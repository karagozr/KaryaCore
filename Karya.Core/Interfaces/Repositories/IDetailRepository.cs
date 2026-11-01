using Karya.Core.Interfaces.Entities;
using Karya.Core.Interfaces.Filters;
namespace Karya.Core.Interfaces.Repositories;


public interface IDetailRepository<TEntity, TId, TParentFilter> : IRepositoryAsync<TEntity, TId>
where TEntity : class, IBaseEntity<TId>, new()
where TId : notnull
where TParentFilter : IParentFilter
{

}
