using Karya.Core.Interfaces.Entities.Tanent;
namespace Karya.Core.Interfaces.Repositories;

public interface ITenantRepository<TEntity, TId> : IRepositoryAsync<TEntity, TId>
where TEntity : class, IBaseTenantEntity<TId, string>, new()
where TId : notnull
{

}
