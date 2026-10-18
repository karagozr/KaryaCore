using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Identity.Application.DTOs;
using Karya.Core.Identity.Application.Services;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Web.Abstracts.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Identity.Controllers;

/// <summary>
/// Tenant yönetimi (CRUD). Yalnızca Sistem Admin erişebilir; yetki kontrolü
/// MediatR AuthorizationBehavior pipeline'ı üzerinden (AppTenant.*) yapılır.
/// </summary>
[Authorize]
public class AppTenantController : BaseCrudController<AppTenant, string, AppTenantSDto, AppTenantLDto, AppTenantADto, AppTenantUDto>
{
    public AppTenantController(IMediator mediator, AppTenantService service)
        : base(mediator, service)
    {
    }
}
