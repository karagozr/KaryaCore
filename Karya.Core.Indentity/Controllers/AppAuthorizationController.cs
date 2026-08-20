using Karya.Core.Indentity.Features.Commands;
using Karya.Core.Web.Abstracts.Controllers;
using Karya.Core.Web.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Controllers;

/// <summary>
/// OpenIddict authorization (yetkilendirme) yönetimi. Listeleme/silme işlemleri
/// MediatR command'ları üzerinden yürütülür; yetki kontrolü AuthorizationBehavior
/// pipeline'ında (AppAuthorization.*) uygulanır. Yalnızca Sistem Admin erişebilir.
/// </summary>
[Authorize]
public class AppAuthorizationController : BaseController
{
    private readonly DbContext _context;

    public AppAuthorizationController(IMediator mediator, DbContext context)
        : base(mediator)
    {
        _context = context;
    }

    /// <summary>Tüm yetkilendirmeleri listeler (DataSourceLoadOptions destekli).</summary>
    [HttpGet]
    public async Task<ActionResult> Select(DataSourceLoadOptions options)
    {
        var result = await _mediator.Send(
            new SelectAuthorizationsCommand(options, _context, "AppAuthorization.Read"));
        return ApiActionResult(result);
    }

    /// <summary>Id'ye göre yetkilendirme getirir.</summary>
    [HttpGet("{id}")]
    public async Task<ActionResult> ByKey(Guid id)
    {
        var result = await _mediator.Send(
            new ByKeyAuthorizationCommand(id, _context, "AppAuthorization.Read"));
        return ApiActionResult(result);
    }

    /// <summary>Id'ye göre yetkilendirmeyi siler.</summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(
            new DeleteAuthorizationCommand(id, _context, "AppAuthorization.Delete"));
        return ApiActionResult(result);
    }
}
