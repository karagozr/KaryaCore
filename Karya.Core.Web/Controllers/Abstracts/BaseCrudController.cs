using Karya.Core.App.Features.Commands;
using Karya.Core.Interfaces.DTOs;
using Karya.Core.Interfaces.Services;
using Karya.Core.Web.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Karya.Core.Web.Abstracts.Controllers;


[ApiController]
[Route("api/[controller]")]
public abstract class BaseCrudController<TEntity, TId, TSingleDto, TSelectDto, TInsertDto, TUpdateDto> : BaseController<TEntity, TId>
    where TId : notnull
    where TSingleDto : class, IByKeyDto, new()
    where TSelectDto : class, ISelectDto, new()
    where TInsertDto : class, IInsertDto, new()
    where TUpdateDto : class, IUpdateDto, new()
{
    protected new IBaseService<TEntity, TId> _service;
    protected BaseCrudController(IMediator mediator, IBaseService<TEntity, TId> service) : base(mediator, service)
    {
        _service = service;
    }

    [HttpGet("{key}")]
    public virtual async Task<ActionResult> ByKey(TId key)
    {
        var result = await _mediator.Send(
            new ByKeyCommand<TEntity, TId, TSingleDto>(key, _service, $"{typeof(TEntity).Name}.Read"));
        return ApiActionResult(result);
    }

    [HttpGet]
    public virtual async Task<ActionResult> Select(DataSourceLoadOptions options)
    {

        var result = await _mediator.Send(
            new SelectCommand<TEntity, TId, TSelectDto>(options, _service, $"{typeof(TEntity).Name}.Read"));
        return ApiActionResult(result);
    }

    [HttpPost]
    public virtual async Task<ActionResult> Insert([FromBody] TInsertDto dto)
    {
        var result = await _mediator.Send(
            new InsertCommand<TEntity, TId, TInsertDto>(dto, _service, $"{typeof(TEntity).Name}.Create"));
        return ApiActionResult(result);
    }

    [HttpPut("{key}")]
    public virtual async Task<ActionResult> Update(TId key, [FromBody] Dictionary<string, object> updateData)
    {

        var result = await _mediator.Send(
            new UpdateCommand<TEntity, TId, TUpdateDto>(key, updateData, _service, $"{typeof(TEntity).Name}.Update"));
        return ApiActionResult(result);
    }

    [HttpDelete("{key}")]
    public virtual async Task<ActionResult> Delete(TId key)
    {
        var result = await _mediator.Send(
            new DeleteCommand<TEntity, TId>(key, _service, $"{typeof(TEntity).Name}.Delete"));
        return ApiActionResult(result);
    }

}
