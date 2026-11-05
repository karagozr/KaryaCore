using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.DTOs;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Infrastructure;

public class AppRoleGroupRoleRepository : BaseDetailRepositoryAsync<AppRoleGroupRole, Guid, AppRoleGroupRoleParentFilter, DbContext>
{
    public AppRoleGroupRoleRepository(DbContext context, ICurrentUser currentUser, AppRoleGroupRoleParentFilter parentFilter) : base(context, currentUser, parentFilter) { }
}