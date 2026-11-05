using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Infrastructure;

/// <summary>AppUserRoleGroup için repository.</summary>
public class AppUserRoleGroupRepository : BaseRepositoryAsync<AppUserRoleGroup, Guid, DbContext>
{
    public AppUserRoleGroupRepository(DbContext context, ICurrentUser currentUser) : base(context, currentUser) { }
}
