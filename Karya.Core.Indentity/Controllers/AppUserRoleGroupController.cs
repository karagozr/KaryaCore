using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.DTOs;
using Karya.Core.Indentity.Services;
using Karya.Core.Web.Abstracts.Controllers;
using MediatR;

namespace Karya.Core.Indentity.Controllers;

public class AppUserRoleGroupController : BaseCrudController<AppUserRoleGroup, Guid, AppUserRoleGroupSDto, AppUserRoleGroupLDto, AppUserRoleGroupADto, AppUserRoleGroupUDto>
{
    public AppUserRoleGroupController(IMediator mediator, AppUserRoleGroupService service) : base(mediator, service)
    {
    }
}

