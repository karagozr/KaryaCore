using Karya.Core.Interfaces.Filters;
using Karya.Core.Interfaces.Repositories;
using Karya.Core.Results;

namespace Karya.Core.Interfaces.UnitOfWorks;


public interface IUnitOfWork : IDisposable
{
    TRepo Repo<TRepo>() where TRepo : class, IRepository;

    TRepo Repo<TRepo>(IParentFilter parentFilter) where TRepo : class, IDetailRepository;

    Task<BaseResult> CompleteAsync();

    BaseResult Complete();
}



