using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.DTOs;
using Karya.Core.Indentity.Services;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Web.Abstracts.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Controllers;

/// <summary>Kullanıcı claim yönetimi (CRUD). Yalnızca Sistem Admin erişebilir.</summary>
[Authorize]
public class AppUserClaimController : BaseCrudController<AppUserClaim, int, AppUserClaimSDto, AppUserClaimLDto, AppUserClaimADto, AppUserClaimUDto>
{
    public AppUserClaimController(IMediator mediator, DbContext context, ICurrentUser currentUser)
        : base(mediator, new AppUserClaimService(context, currentUser))
    {
    }
}
