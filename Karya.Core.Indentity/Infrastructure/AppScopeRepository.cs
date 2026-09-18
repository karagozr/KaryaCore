using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Infrastructure
{
    public class AppScopeRepository : BaseRepositoryAsync<AppScope, Guid, DbContext>
    {
        public AppScopeRepository(DbContext context, ICurrentUser currentUser) : base(context, currentUser) { }
    }
}
