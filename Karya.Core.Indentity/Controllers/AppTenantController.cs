using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.DTOs;
using Karya.Core.Indentity.Services;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Web.Abstracts.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Controllers;

/// <summary>
/// Tenant yönetimi (CRUD). Yalnızca Sistem Admin erişebilir; yetki kontrolü
/// MediatR AuthorizationBehavior pipeline'ı üzerinden (AppTenant.*) yapılır.
/// </summary>
[Authorize]
public abstract class AppTenantController : BaseCrudController<AppTenant, string, AppTenantSDto, AppTenantLDto, AppTenantADto, AppTenantUDto>
{
    public AppTenantController(IMediator mediator, DbContext context, ICurrentUser currentUser)
        : base(mediator, new AppTenantService(context, currentUser))
    {
    }
}
