using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Identity.Application.Services;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Web.Abstracts.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Identity.Controllers;

/// <summary>
/// Tenant y�netimi (CRUD). Yaln�zca Sistem Admin eri�ebilir; yetki kontrol�
/// MediatR AuthorizationBehavior pipeline'� �zerinden (AppTenant.*) yap�l�r.
/// </summary>
[Authorize]
public abstract class AppTenantController : BaseCrudController<AppTenant, string, AppTenantSDto, AppTenantLDto, AppTenantADto, AppTenantUDto>
{
    public AppTenantController(IMediator mediator, AppTenantService service)
        : base(mediator, service)
    {
    }
}
