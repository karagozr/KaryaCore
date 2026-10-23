using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Identity.Application.Services;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Web.Abstracts.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Identity.Controllers;

/// <summary>Rol yönetimi (CRUD). Yalnýzca Sistem Admin eriþebilir.</summary>
[Authorize]
public class AppRoleController : BaseCrudController<AppRole, Guid, AppRoleSDto, AppRoleLDto, AppRoleADto, AppRoleUDto>
{
    public AppRoleController(IMediator mediator, AppRoleService service)
        : base(mediator, service)
    {
    }
}
