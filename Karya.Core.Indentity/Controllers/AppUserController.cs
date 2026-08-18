using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.DTOs;
using Karya.Core.Indentity.Services;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Web.Abstracts.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Controllers;

/// <summary>
/// Kullanıcı yönetimi (CRUD). Yetki kontrolü MediatR AuthorizationBehavior
/// pipeline'ı üzerinden (AppUser.Read/Create/Update/Delete) yapılır; tenant
/// kapsamı repository katmanında row-level uygulanır.
/// </summary>
[Authorize]
public abstract class AppUserController : BaseCrudController<AppUser, Guid, AppUserSDto, AppUserLDto, AppUserADto, AppUserUDto>
{
    public AppUserController(IMediator mediator, DbContext context, ICurrentUser currentUser, UserManager<AppUser> userManager)
        : base(mediator, new AppUserService(context, currentUser, userManager))
    {
    }
}
