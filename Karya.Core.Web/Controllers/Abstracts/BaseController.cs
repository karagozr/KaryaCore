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
using System.Linq.Expressions;

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

    protected async Task<ActionResult> LookupResult<TLookupEntity, TLookupEntityId, TLookupDto>(
        IBaseService<TLookupEntity, TLookupEntityId> lookupService,
        DataSourceLoadOptions options,
        string permission,
        Expression<Func<TLookupEntity, bool>>? predicate = null)
        where TLookupEntityId : notnull
        where TLookupDto : class, ILookupDto, new()
    {
        var result = await _mediator.Send(
            new LookupCommand<TLookupEntity, TLookupEntityId, TLookupDto>(options, lookupService, permission, predicate));

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
