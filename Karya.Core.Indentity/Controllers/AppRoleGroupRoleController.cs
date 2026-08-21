using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.DTOs;
using Karya.Core.Indentity.Services;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Web.Abstracts.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Controllers;

[Authorize]
public class AppRoleGroupRoleController
    : BaseCrudDetailController<AppRoleGroupRole, Guid, AppRoleGroupRoleParentFilter, AppRoleGroupRoleSDto, AppRoleGroupRoleLDto, AppRoleGroupRoleADto, AppRoleGroupRoleUDto>
{
    public AppRoleGroupRoleController(IMediator mediator, DbContext context, ICurrentUser currentUser)
        : base(mediator, new AppRoleGroupRoleService(context, currentUser)) { }
}