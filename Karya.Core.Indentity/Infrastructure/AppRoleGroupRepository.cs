using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Infrastructure;

/// <summary>AppRoleGroup için repository.</summary>
public class AppRoleGroupRepository : BaseRepositoryAsync<AppRoleGroup, Guid, DbContext>
{
    public AppRoleGroupRepository(DbContext context, ICurrentUser currentUser) : base(context, currentUser) { }
}
