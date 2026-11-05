using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Identity.Infrastructure.Persistence.Repositories;

/// <summary>AppRoleGroup için repository.</summary>
public class AppRoleGroupRepository : BaseRepositoryAsync<AppRoleGroup, Guid, DbContext>
{
    public AppRoleGroupRepository(DbContext context, ICurrentUser currentUser) : base(context, currentUser) { }
}
