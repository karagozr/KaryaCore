using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Infrastructure;

/// <summary>AppUserClaim için repository.</summary>
public class AppUserClaimRepository : BaseRepositoryAsync<AppUserClaim, int, DbContext>
{
    public AppUserClaimRepository(DbContext context, ICurrentUser currentUser) : base(context, currentUser) { }
}
