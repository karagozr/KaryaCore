using Karya.Core.Helpers.Repository;
using Karya.Core.Interfaces.Filters;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Interfaces.Repositories;
using Karya.Core.Interfaces.UnitOfWorks;
using Karya.Core.Results;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Repositories;

public abstract class BaseUnitOfWork : IUnitOfWork
{
    protected readonly DbContext _context;
    protected readonly ICurrentUser _currentUser;
    protected readonly IParentFilter? _parentFilter;
    protected BaseUnitOfWork(DbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }


    public TRepo Repo<TRepo>() where TRepo : class, IRepository
    {
        return (Activator.CreateInstance(typeof(TRepo), _context, _currentUser) as TRepo)!;
    }

    public TRepo Repo<TRepo>(IParentFilter parentFilter) where TRepo : class, IDetailTenantRepository
    {
        return (Activator.CreateInstance(typeof(TRepo), _context, _currentUser, parentFilter) as TRepo)!;
    }

    public BaseResult Complete()
    {
        try
        {
            _context.SaveChanges();
            return BaseResult.SuccessCoded("200", MessageCodes.Success);
        }
        catch (Exception)
        {
            return BaseResult.ErrorCoded("500", MessageCodes.ServerError);
        }
    }


    public async Task<BaseResult> CompleteAsync()
    {
        try
        {
            var isCreated = _context.ChangeTracker.Entries()
                     .Any(e => e.State == EntityState.Added);

            await _context.SaveChangesAsync();

            if(isCreated)
                return BaseResult.SuccessCoded("201", MessageCodes.Created);

            return BaseResult.SuccessCoded("200", MessageCodes.Success);

        }
        catch (DbUpdateException ex)
        {

            var inn = ex.InnerException;
            if(inn == null)
                return BaseResult.ErrorCoded("500", MessageCodes.DbError);
            else
            {
                var sqlEx = inn as SqlException;

                return BaseResult.ErrorCoded("400", SqlErrorHandlerHelper.GetMessageCode(sqlEx));
            }

        }

    }

    #region Disposing
    private bool _disposed = false;
    private object qlEx;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);

    }

    private void Dispose(bool disposing)
    {
        if(!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }
    #endregion
}
