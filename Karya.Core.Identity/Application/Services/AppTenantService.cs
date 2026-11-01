using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Identity.Infrastructure;
using Karya.Core.Identity.Infrastructure.Persistence;
using Karya.Core.Identity.Infrastructure.Persistence.Repositories;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Identity.Application.Services;

/// <summary>
/// Tenant yönetimi servisi. Yetki (yalnızca SystemAdmin) MediatR
/// AuthorizationBehavior pipeline'ı üzerinden "AppTenant.*" izinleriyle uygulanır.
/// </summary>
public class AppTenantService : BaseService<AppTenantRepository, AppTenant, string>
{
    public AppTenantService(IdentityUnitOfWork uow) : base(uow) { }
}
