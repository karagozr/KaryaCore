using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Identity.Application.Services;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Web.Abstracts.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Identity.Controllers;

/// <summary>Rol grubu yönetimi (CRUD). Yalnýzca Sistem Admin eriþebilir.</summary>
[Authorize]
public class AppRoleGroupController : BaseCrudController<AppRoleGroup, Guid, AppRoleGroupSDto, AppRoleGroupLDto, AppRoleGroupADto, AppRoleGroupUDto>
{
    public AppRoleGroupController(IMediator mediator, AppRoleGroupService service)
        : base(mediator, service)
    {
    }
}
