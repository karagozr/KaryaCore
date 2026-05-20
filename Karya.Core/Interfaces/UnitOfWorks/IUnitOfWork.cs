using Karya.Core.Interfaces.Repositories;
using Karya.Core.Results;

namespace Karya.Core.Interfaces.UnitOfWorks;


public interface IUnitOfWork : IDisposable
{
    TRepo Repo<TRepo>() where TRepo : class, IRepository;

    Task<BaseResult> CompleteAsync();

    BaseResult Complete();
}



