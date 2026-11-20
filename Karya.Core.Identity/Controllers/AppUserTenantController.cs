using Karya.Core.Identity.Application.DTOs.Users;
using Karya.Core.Identity.Application.Features.Commands;
using Karya.Core.Identity.Application.Services;
using Karya.Core.Web.Abstracts.Controllers;
using Karya.Core.Web.Helpers;
using Karya.Core.Web.Infrastructure.Binders;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Identity.Controllers;

[Authorize]
public abstract class AppUserTenantController(
    IMediator mediator,
    DbContext db,
    AppUserTenantService service) : BaseController(mediator)
{
    [HttpGet("/api/AppUserTenant({parent})")]
    public virtual async Task<ActionResult> SelectByUser(
        [ModelBinder(typeof(GenericParentFilterModelBinder))] AppUserTenantParentFilter parent,
        DataSourceLoadOptions options)
        => ApiActionResult(await _mediator.Send(new SelectUserTenantsCommand(options, db, parent.UserId)));

    [HttpPost("/api/AppUserTenant({parent})")]
    public virtual async Task<ActionResult> AssignByUser(
        [ModelBinder(typeof(GenericParentFilterModelBinder))] AppUserTenantParentFilter parent,
        [FromBody] AppUserTenantAssignDto dto)
        => ApiActionResult(await _mediator.Send(new AssignUserTenantCommand(
            new AppUserTenantAssignDto { UserId = parent.UserId, TenantId = dto.TenantId },
            service)));

    [HttpDelete("/api/AppUserTenant({parent})/{key}")]
    public virtual async Task<ActionResult> RemoveByUser(
        [ModelBinder(typeof(GenericParentFilterModelBinder))] AppUserTenantParentFilter parent,
        string key)
    {
        var parts = key.Split('|', 2);
        if (parts.Length != 2 || !Guid.TryParse(parts[0], out var userId) || userId != parent.UserId)
            return BadRequest("Invalid user tenant key.");

        return ApiActionResult(await _mediator.Send(
            new RemoveUserTenantCommand(parent.UserId, parts[1], service)));
    }
}

public sealed class AppUserTenantParentFilter
{
    public Guid UserId { get; set; }
}
