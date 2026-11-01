using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Identity.Infrastructure.Persistence.Repositories;

/// <summary>AppTenant için repository (soyut DbContext üzerinden).</summary>
public class AppTenantRepository : BaseRepositoryAsync<AppTenant, string, DbContext>
{
    public AppTenantRepository(DbContext context, ICurrentUser currentUser) : base(context, currentUser)
    {
    }
}
