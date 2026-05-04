using Karya.Core.Interfaces.Repositories;
using Karya.Core.Interfaces.Results;

namespace Karya.Core.Interfaces.UnitOfWorks;

public interface IUnitOfWork : IDisposable
{
    TRepo Repo<TRepo>() where TRepo : class, IRepository;

    Task<IBaseResult> CompleteAsync();

    IBaseResult Complete();
}

public interface ITanentUnitOfWork : IDisposable
{
    TRepo Repo<TRepo>() where TRepo : class, ITanentRepository;

    Task<IBaseResult> CompleteAsync();

    IBaseResult Complete();
}


