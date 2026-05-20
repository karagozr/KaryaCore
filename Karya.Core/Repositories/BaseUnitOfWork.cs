using Karya.Core.Interfaces.Identities;
using Karya.Core.Interfaces.Repositories;
using Karya.Core.Interfaces.UnitOfWorks;
using Karya.Core.Results;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Karya.Core.Repositories;

public abstract class BaseUnitOfWork : IUnitOfWork
{
    protected readonly DbContext _context;
    protected readonly ICurrentUser _currentUser;

    protected BaseUnitOfWork(DbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public TRepo Repo<TRepo>() where TRepo : class, IRepository
    {
        return (Activator.CreateInstance(typeof(TRepo), _context, _currentUser) as TRepo)!;
    }

    public BaseResult Complete()
    {
        try
        {
            _context.SaveChanges();
            return BaseResult.Success();
        }
        catch (Exception ex)
        {
            var inex = ex.InnerException;
            var message = inex != null ? inex.Message : ex.Message;
            return BaseResult.Error("500",message);
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
                return BaseResult.Success("201");

            return BaseResult.Success();

        }
        catch (DbUpdateException ex)
        {

            var inn = ex.InnerException;
            if(inn == null)
                return BaseResult.Error("500","Unknown DB Error");
            else
            {
                var sqlEx = inn as SqlException;
                if(sqlEx == null)
                    return BaseResult.Error("500", "DB Error : " + sqlEx?.Message);

                return BaseResult.Error("400", sqlEx.Number.ToString() + "-" + sqlEx.Message);
            }
                
        }
    }

    #region Disposing
    private bool _disposed = false;
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
