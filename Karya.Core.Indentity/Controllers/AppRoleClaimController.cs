using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.DTOs;
using Karya.Core.Indentity.Services;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Web.Abstracts.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Controllers;

/// <summary>Rol claim yönetimi (CRUD). Yalnızca Sistem Admin erişebilir.</summary>
[Authorize]
public abstract class AppRoleClaimController : BaseCrudController<AppRoleClaim, int, AppRoleClaimSDto, AppRoleClaimLDto, AppRoleClaimADto, AppRoleClaimUDto>
{
    public AppRoleClaimController(IMediator mediator, DbContext context, ICurrentUser currentUser)
        : base(mediator, new AppRoleClaimService(context, currentUser))
    {
    }
}
