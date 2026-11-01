using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Identity.Infrastructure.Persistence.Repositories;

/// <summary>AppRole için repository.</summary>
public class AppRoleRepository : BaseRepositoryAsync<AppRole, Guid, DbContext>
{
    public AppRoleRepository(DbContext context, ICurrentUser currentUser) : base(context, currentUser) { }
}
