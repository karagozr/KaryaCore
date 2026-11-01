using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Identity.Infrastructure.Persistence.Repositories;

/// <summary>AppRoleClaim için repository.</summary>
public class AppRoleClaimRepository : BaseRepositoryAsync<AppRoleClaim, int, DbContext>
{
    public AppRoleClaimRepository(DbContext context, ICurrentUser currentUser) : base(context, currentUser) { }
}
