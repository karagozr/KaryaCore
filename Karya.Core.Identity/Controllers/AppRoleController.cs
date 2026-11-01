using Karya.Core.App.Features.Commands;
using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Identity.Application.Services;
using Karya.Core.Web.Abstracts.Controllers;
using Karya.Core.Web.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Karya.Core.Identity.Controllers;

/// <summary>Rol sorgulama (salt okunur).</summary>
[Authorize]
public abstract class AppRoleController : BaseController
{
    protected new readonly AppRoleService _service;

    public AppRoleController(IMediator mediator, AppRoleService service)
        : base(mediator, service)
    {
        _service = service;
    }

    [HttpGet("{key}")]
    public virtual async Task<ActionResult> ByKey(Guid key)
    {
        var result = await _mediator.Send(
            new ByKeyCommand<AppRole, Guid, AppRoleSDto>(key, _service, $"{nameof(AppRole)}.Read"));
        return ApiActionResult(result);
    }

    [HttpGet]
    public virtual async Task<ActionResult> Select(DataSourceLoadOptions options)
    {
        var result = await _mediator.Send(
            new SelectCommand<AppRole, Guid, AppRoleLDto>(options, _service, $"{nameof(AppRole)}.Read"));
        return ApiActionResult(result);
    }
}
