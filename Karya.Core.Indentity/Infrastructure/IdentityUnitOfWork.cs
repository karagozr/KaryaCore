using Karya.Core.Interfaces.Identities;
using Karya.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Infrastructure;

/// <summary>
/// Identity DbContext üzerinde çalışan UnitOfWork. DI tarafından sağlanan
/// gerçek <see cref="DbContext"/> (AppDbContext) örneğini kullanır.
/// </summary>
public class IdentityUnitOfWork : BaseUnitOfWork
{
    public IdentityUnitOfWork(DbContext context, ICurrentUser currentUser) : base(context, currentUser)
    {
    }
}
