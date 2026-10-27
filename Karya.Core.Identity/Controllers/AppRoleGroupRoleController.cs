using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Identity.Application.Services;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Web.Abstracts.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Identity.Controllers;

[Authorize]
public abstract class AppRoleGroupRoleController
    : BaseCrudDetailController<AppRoleGroupRole, Guid, AppRoleGroupRoleParentFilter, AppRoleGroupRoleSDto, AppRoleGroupRoleLDto, AppRoleGroupRoleADto, AppRoleGroupRoleUDto>
{
    public AppRoleGroupRoleController(IMediator mediator, AppRoleGroupRoleService service)
        : base(mediator, service) { }
}
