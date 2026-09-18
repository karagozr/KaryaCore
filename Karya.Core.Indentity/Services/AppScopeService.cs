using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.Infrastructure;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Services;

/// <summary>Scope yönetimi servisi.</summary>
public class AppScopeService : BaseService<AppScopeRepository, AppScope, Guid>
{
    public AppScopeService(DbContext context, ICurrentUser currentUser) : base(new IdentityUnitOfWork(context, currentUser)) { }
}
