using Karya.Core.Indentity.Features.Commands;
using Karya.Core.Web.Abstracts.Controllers;
using Karya.Core.Web.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Controllers;

/// <summary>
/// Kullanıcı token (IdentityUserToken) yönetimi. Listeleme/silme işlemleri MediatR
/// command'ları üzerinden yürütülür; yetki kontrolü AuthorizationBehavior
/// pipeline'ında (AppUserToken.*) uygulanır. Yalnızca Sistem Admin erişebilir.
/// </summary>
[Authorize]
public abstract class AppUserTokenController : BaseController
{
    private readonly DbContext _context;

    public AppUserTokenController(IMediator mediator, DbContext context)
        : base(mediator)
    {
        _context = context;
    }

    /// <summary>Tüm kullanıcı token'larını listeler (DataSourceLoadOptions destekli).</summary>
    [HttpGet]
    public async Task<ActionResult> Select(DataSourceLoadOptions options)
    {
        var result = await _mediator.Send(
            new SelectUserTokensCommand(options, _context, "AppUserToken.Read"));
        return ApiActionResult(result);
    }

    /// <summary>Kullanıcı, sağlayıcı ve ada göre token'ı siler.</summary>
    [HttpDelete("{userId}/{loginProvider}/{name}")]
    public async Task<ActionResult> Delete(Guid userId, string loginProvider, string name)
    {
        var result = await _mediator.Send(
            new DeleteUserTokenCommand(userId, loginProvider, name, _context, "AppUserToken.Delete"));
        return ApiActionResult(result);
    }
}
