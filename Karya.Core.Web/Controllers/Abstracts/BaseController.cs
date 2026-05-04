using Karya.Core.Interfaces.DTOs;
using Karya.Core.Interfaces.Results;
using Karya.Core.Interfaces.Services;
using Karya.Core.App.Features.Commands;
using Karya.Core.Web.Returns.Api;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Karya.Core.Web.Abstracts.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController<TEntity, TId> : ControllerBase
    where TId : notnull
{
    protected readonly IMediator _mediator;
    protected readonly IBaseService<TEntity, TId> _service;

    protected BaseController(IMediator mediator, IBaseService<TEntity, TId> service)
    {
        _mediator = mediator;
        _service = service;
    }

    protected ApiResult<TData> ApiActionResult<TData>(IBaseResult<TData> result) => new(result);
    protected ApiResult ApiActionResult(IBaseResult result) => new(result);
}
