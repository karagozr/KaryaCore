using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Identity.Application.Services;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Web.Abstracts.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Identity.Controllers;

/// <summary>Rol claim yönetimi (CRUD). Yalnýzca Sistem Admin eriþebilir.</summary>
[Authorize]
public class AppRoleClaimController : BaseCrudController<AppRoleClaim, int, AppRoleClaimSDto, AppRoleClaimLDto, AppRoleClaimADto, AppRoleClaimUDto>
{
    public AppRoleClaimController(IMediator mediator, AppRoleClaimService service)
        : base(mediator, service)
    {
    }
}
