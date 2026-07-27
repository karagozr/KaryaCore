using Karya.Core.Indentity.DTOs;
using Karya.Core.Indentity.Features.Commands;
using Karya.Core.Web.Abstracts.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;

namespace Karya.Core.Indentity.Controllers;

/// <summary>
/// OpenIddict uygulama (client) yönetimi. Tüm işlemler MediatR command'ları
/// üzerinden yürütülür; yetki kontrolü AuthorizationBehavior pipeline'ında
/// (AppApplication.*) uygulanır. Yalnızca Sistem Admin erişebilir.
/// </summary>
[Authorize]
public class ApplicationController : BaseController
{
    private readonly IOpenIddictApplicationManager _applicationManager;

    public ApplicationController(IMediator mediator, IOpenIddictApplicationManager applicationManager)
        : base(mediator)
    {
        _applicationManager = applicationManager;
    }

    /// <summary>Tüm uygulamaları listeler.</summary>
    [HttpGet]
    public async Task<ActionResult> Select()
    {
        var result = await _mediator.Send(
            new SelectApplicationsCommand(_applicationManager, "AppApplication.Read"));
        return ApiActionResult(result);
    }

    /// <summary>ClientId'ye göre uygulama getirir.</summary>
    [HttpGet("{clientId}")]
    public async Task<ActionResult> ByClientId(string clientId)
    {
        var result = await _mediator.Send(
            new ByClientIdApplicationCommand(clientId, _applicationManager, "AppApplication.Read"));
        return ApiActionResult(result);
    }

    /// <summary>Yeni uygulama (client) oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult> Insert([FromBody] AppApplicationADto dto)
    {
        var result = await _mediator.Send(
            new InsertApplicationCommand(dto, _applicationManager, "AppApplication.Create"));
        return ApiActionResult(result);
    }

    /// <summary>ClientId'ye göre uygulamayı siler.</summary>
    [HttpDelete("{clientId}")]
    public async Task<ActionResult> Delete(string clientId)
    {
        var result = await _mediator.Send(
            new DeleteApplicationCommand(clientId, _applicationManager, "AppApplication.Delete"));
        return ApiActionResult(result);
    }
}
