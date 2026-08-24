using Karya.Core.Indentity.Features.Commands;
using Karya.Core.Web.Abstracts.Controllers;
using Karya.Core.Web.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Controllers;

/// <summary>
/// Kullanıcı harici giriş (external login) yönetimi. Listeleme/silme işlemleri
/// MediatR command'ları üzerinden yürütülür; yetki kontrolü AuthorizationBehavior
/// pipeline'ında (AppUserLogin.*) uygulanır. Yalnızca Sistem Admin erişebilir.
/// </summary>
[Authorize]
public class AppUserLoginController : BaseController
{
    private readonly DbContext _context;

    public AppUserLoginController(IMediator mediator, DbContext context)
        : base(mediator)
    {
        _context = context;
    }

    /// <summary>Tüm harici girişleri listeler (DataSourceLoadOptions destekli).</summary>
    [HttpGet]
    public async Task<ActionResult> Select(DataSourceLoadOptions options)
    {
        var result = await _mediator.Send(
            new SelectUserLoginsCommand(options, _context, "AppUserLogin.Read"));
        return ApiActionResult(result);
    }

    /// <summary>Sağlayıcı ve anahtara göre harici girişi siler.</summary>
    [HttpDelete("{loginProvider}/{providerKey}")]
    public async Task<ActionResult> Delete(string loginProvider, string providerKey)
    {
        var result = await _mediator.Send(
            new DeleteUserLoginCommand(loginProvider, providerKey, _context, "AppUserLogin.Delete"));
        return ApiActionResult(result);
    }
}
