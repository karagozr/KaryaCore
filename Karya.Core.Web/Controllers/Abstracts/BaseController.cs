using DevExtreme.AspNet.Data;
using Karya.Core.App.Features.Commands;
using Karya.Core.Interfaces.DTOs;
using Karya.Core.Interfaces.Services;
using Karya.Core.Results;
using Karya.Core.Web.Helpers;
using Karya.Core.Web.Returns.Api;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Karya.Core.Web.Abstracts.Controllers;

//[Authorize]
[ApiController]
[Route("api/[controller]")]
public abstract class BaseController: ControllerBase
{
    protected readonly IMediator _mediator;
    protected readonly IBaseService _service;

    protected BaseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected BaseController(IMediator mediator, IBaseService service):this(mediator)
    {
        _service = service;
    }

    protected ApiResult<TData> ApiActionResult<TData>(BaseResult<TData> result) => new(result);
    protected ApiResult ApiActionResult(BaseResult result) => new(result);

    protected async Task<ActionResult> LookupResult<TLookupEntity, TLookupEntityId, TForeignSelectDto>(
        DataSourceLoadOptions options,
        string permission,
        System.Linq.Expressions.Expression<Func<TLookupEntity, bool>>? predicate = null)
        where TLookupEntityId : notnull
        where TForeignSelectDto : class, ISelectDto, new()
    {
        var lookupService = HttpContext.RequestServices
            .GetRequiredService<IBaseService<TLookupEntity, TLookupEntityId>>();

        var result = await _mediator.Send(
            new LookupCommand<TLookupEntity, TLookupEntityId, TForeignSelectDto>(options, lookupService, permission, predicate));

        return ApiActionResult(result);
    }
}

public abstract class BaseController<TEntity, TId> : BaseController
    where TId : notnull
{
    protected BaseController(IMediator mediator, IBaseService service)
        : base(mediator, service)
    {
    }
}
