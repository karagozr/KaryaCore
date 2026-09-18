using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Infrastructure;

/// <summary>AppRole için repository.</summary>
public class AppRoleRepository : BaseRepositoryAsync<AppRole, Guid, DbContext>
{
    public AppRoleRepository(DbContext context, ICurrentUser currentUser) : base(context, currentUser) { }
}
