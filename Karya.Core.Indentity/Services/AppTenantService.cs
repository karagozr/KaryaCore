using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.Infrastructure;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Services;

/// <summary>
/// Tenant yönetimi servisi. Yetki (yalnızca SystemAdmin) MediatR
/// AuthorizationBehavior pipeline'ı üzerinden "AppTenant.*" izinleriyle uygulanır.
/// </summary>
public class AppTenantService : BaseService<AppTenantRepository, AppTenant, string>
{
    public AppTenantService(DbContext context, ICurrentUser currentUser)
        : base(new IdentityUnitOfWork(context, currentUser))
    {
    }
}
