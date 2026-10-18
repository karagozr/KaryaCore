using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Identity.Application.DTOs;
using Karya.Core.Identity.Application.Services;
using Karya.Core.Web.Abstracts.Controllers;
using MediatR;

namespace Karya.Core.Identity.Controllers;

public class AppUserRoleGroupController : BaseCrudController<AppUserRoleGroup, Guid, AppUserRoleGroupSDto, AppUserRoleGroupLDto, AppUserRoleGroupADto, AppUserRoleGroupUDto>
{
    public AppUserRoleGroupController(IMediator mediator, AppUserRoleGroupService service) : base(mediator, service)
    {
    }
}

