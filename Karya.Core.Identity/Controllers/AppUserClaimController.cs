using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Identity.Application.Services;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Web.Abstracts.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Identity.Controllers;

/// <summary>Kullanýcý claim yönetimi (CRUD). Yalnýzca Sistem Admin eriþebilir.</summary>
[Authorize]
public class AppUserClaimController : BaseCrudController<AppUserClaim, int, AppUserClaimSDto, AppUserClaimLDto, AppUserClaimADto, AppUserClaimUDto>
{
    public AppUserClaimController(IMediator mediator, AppUserClaimService service)
        : base(mediator, service)
    {
    }
}
