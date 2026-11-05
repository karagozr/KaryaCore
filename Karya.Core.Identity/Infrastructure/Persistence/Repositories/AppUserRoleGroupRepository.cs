using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Identity.Infrastructure.Persistence.Repositories;

/// <summary>AppUserRoleGroup için repository.</summary>
public class AppUserRoleGroupRepository : BaseRepositoryAsync<AppUserRoleGroup, Guid, DbContext>
{
    public AppUserRoleGroupRepository(DbContext context, ICurrentUser currentUser) : base(context, currentUser) { }
}
