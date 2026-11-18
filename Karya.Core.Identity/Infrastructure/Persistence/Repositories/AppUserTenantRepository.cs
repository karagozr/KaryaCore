using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Identity.Infrastructure.Persistence.Repositories;

public class AppUserTenantRepository : BaseRepositoryAsync<AppUserTenant, Guid, DbContext>
{
    public AppUserTenantRepository(DbContext context, ICurrentUser currentUser) : base(context, currentUser)
    {
    }
}
