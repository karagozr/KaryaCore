using Karya.Core.Interfaces.Services;
using Karya.Core.Results;
using Karya.Core.Web.Returns.Api;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    protected BaseController(IMediator mediator, IBaseService service)
    {
        _mediator = mediator;
        _service = service;
    }

    protected ApiResult<TData> ApiActionResult<TData>(BaseResult<TData> result) => new(result);
    protected ApiResult ApiActionResult(BaseResult result) => new(result);
}

public abstract class BaseController<TEntity, TId> : BaseController
    where TId : notnull
{
    protected BaseController(IMediator mediator, IBaseService service)
        : base(mediator, service)
    {
    }
}
