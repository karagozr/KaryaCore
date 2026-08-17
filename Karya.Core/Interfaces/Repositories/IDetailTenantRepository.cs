using Karya.Core.Interfaces.Entities.Tanent;
using Karya.Core.Interfaces.Filters;
namespace Karya.Core.Interfaces.Repositories;

public interface IDetailRepository
{

}
public interface IDetailTenantRepository<TEntity, TId, TParentFilter> : IDetailRepository, ITenantRepository<TEntity, TId>
where TEntity : class, IBaseTenantEntity<TId, string>, new()
where TId : notnull
where TParentFilter : IParentFilter
{

}
