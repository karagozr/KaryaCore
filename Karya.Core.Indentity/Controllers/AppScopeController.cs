using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.DTOs;
using Karya.Core.Indentity.Services;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Web.Abstracts.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Controllers;

/// <summary>Scope yönetimi (CRUD). Yalnızca Sistem Admin erişebilir.</summary>
[Authorize]
public abstract class AppScopeController : BaseCrudController<AppScope, Guid, AppScopeSDto, AppScopeLDto, AppScopeADto, AppScopeUDto>
{
    public AppScopeController(IMediator mediator, DbContext context, ICurrentUser currentUser)
        : base(mediator, new AppScopeService(context, currentUser))
    {
    }
}
