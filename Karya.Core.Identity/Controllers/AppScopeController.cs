using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Identity.Application.DTOs;
using Karya.Core.Identity.Application.Services;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Web.Abstracts.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Identity.Controllers;

/// <summary>Scope yönetimi (CRUD). Yalnızca Sistem Admin erişebilir.</summary>
[Authorize]
public class AppScopeController : BaseCrudController<AppScope, Guid, AppScopeSDto, AppScopeLDto, AppScopeADto, AppScopeUDto>
{
    public AppScopeController(IMediator mediator, AppScopeService service)
        : base(mediator, service)
    {
    }
}
