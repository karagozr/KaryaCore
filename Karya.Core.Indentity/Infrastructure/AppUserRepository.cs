using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Infrastructure;

/// <summary>
/// AppUser için repository. Soyut <see cref="DbContext"/> üzerinden çalışır;
/// gerçek context (AppDbContext) UnitOfWork tarafından sağlanır.
/// </summary>
public class AppUserRepository : BaseRepositoryAsync<AppUser, Guid, DbContext>
{
    public AppUserRepository(DbContext context, ICurrentUser currentUser) : base(context, currentUser)
    {
    }

    /// <summary>
    /// Row-level tenant filtrelemesi: SystemAdmin tüm tenant'lardaki kullanıcıları
    /// görür; diğer adminler yalnızca aktif tenant'larındaki kullanıcıları görür.
    /// </summary>
    public override IQueryable<AppUser> Query(Func<IQueryable<AppUser>, IQueryable<AppUser>>? include = null, bool withDeleted = false, CancellationToken ct = default)
    {
        var query = base.Query(include, withDeleted, ct);

        var isSystemAdmin = false;
        if (Guid.TryParse(_currentUser.UserId, out var currentUserId))
        {
            isSystemAdmin = _context.Set<AppUser>().AsNoTracking()
                .Where(u => u.Id == currentUserId)
                .Select(u => u.IsSystemAdmin)
                .FirstOrDefault();
        }

        if (!isSystemAdmin)
            query = query.Where(u => u.TenantId == _currentUser.TenantId);

        return query;
    }
}
