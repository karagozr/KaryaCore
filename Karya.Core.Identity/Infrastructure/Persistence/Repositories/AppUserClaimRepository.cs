using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Identity.Infrastructure.Persistence.Repositories;

/// <summary>AppUserClaim için repository.</summary>
public class AppUserClaimRepository : BaseRepositoryAsync<AppUserClaim, int, DbContext>
{
    public AppUserClaimRepository(DbContext context, ICurrentUser currentUser) : base(context, currentUser) { }
}
