using Karya.Core.Indentity.Features.Commands;
using Karya.Core.Web.Abstracts.Controllers;
using Karya.Core.Web.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Controllers;

/// <summary>
/// OpenIddict token yönetimi. Listeleme/silme işlemleri MediatR command'ları
/// üzerinden yürütülür; yetki kontrolü AuthorizationBehavior pipeline'ında
/// (AppToken.*) uygulanır. Yalnızca Sistem Admin erişebilir.
/// </summary>
[Authorize]
public class AppTokenController : BaseController
{
    private readonly DbContext _context;

    public AppTokenController(IMediator mediator, DbContext context)
        : base(mediator)
    {
        _context = context;
    }

    /// <summary>Tüm token'ları listeler (DataSourceLoadOptions destekli).</summary>
    [HttpGet]
    public async Task<ActionResult> Select(DataSourceLoadOptions options)
    {
        var result = await _mediator.Send(
            new SelectTokensCommand(options, _context, "AppToken.Read"));
        return ApiActionResult(result);
    }

    /// <summary>Id'ye göre token getirir.</summary>
    [HttpGet("{id}")]
    public async Task<ActionResult> ByKey(Guid id)
    {
        var result = await _mediator.Send(
            new ByKeyTokenCommand(id, _context, "AppToken.Read"));
        return ApiActionResult(result);
    }

    /// <summary>Id'ye göre token'ı siler.</summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(
            new DeleteTokenCommand(id, _context, "AppToken.Delete"));
        return ApiActionResult(result);
    }
}
