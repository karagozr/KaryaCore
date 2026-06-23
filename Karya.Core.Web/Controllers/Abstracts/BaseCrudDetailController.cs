using DevExtreme.AspNet.Data.ResponseModel;
using Karya.Core.App.Features.Commands;
using Karya.Core.Interfaces.DTOs;
using Karya.Core.Interfaces.Services;
using Karya.Core.Web.Helpers;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text.Json;

namespace Karya.Core.Web.Abstracts.Controllers;


[ApiController]
[Route("api/[controller]")]
public abstract class BaseCrudDetailController<TEntity, TId, TParentId, TSingleDto, TSelectDto, TInsertDto, TUpdateDto> : BaseController<TEntity,TId>
    where TId : notnull
    where TParentId : notnull
    where TSingleDto : class, ISingleDto, new()
    where TSelectDto : class, ISelectDto, new()
    where TInsertDto : class, IInsertDto, new()
    where TUpdateDto : class, IUpdateDto, new()
{

    protected BaseCrudDetailController(IMediator mediator, IBaseService<TEntity, TId> service):base(mediator, service)
    {
    }

    [HttpGet("{key}")]
    public virtual async Task<ActionResult> ByKey(TId key)
    {
        var result = await _mediator.Send(
            new ByKeyCommand<TEntity, TId, TSingleDto>(key, _service, $"{typeof(TEntity).Name}.Read"));
        return ApiActionResult(result);
    }

    [HttpGet("master({parentName}={parentValue})")]
    public virtual async Task<ActionResult> Select(string parentName, TParentId parentValue, DataSourceLoadOptions options)
    {

        var result = await _mediator.Send(
            new SelectDetailCommand<TEntity, TId, TParentId, TSelectDto>(parentName, parentValue, options, _service, $"{typeof(TEntity).Name}.Read"));
        return ApiActionResult(result);
    }

    [HttpPost("master({parentName}={parentValue})")]
    public virtual async Task<ActionResult> Insert(string parentName, TParentId parentValue, [FromBody] TInsertDto dto)
    {
        var result = await _mediator.Send(
            new InsertCommand<TEntity, TId, TInsertDto>(dto, _service, $"{typeof(TEntity).Name}.Create"));
        return ApiActionResult(result);
    }

    [HttpPut("master({parentName}={parentValue})/{key}")]
    public virtual async Task<ActionResult> Update(string parentName, TParentId parentValue, TId key, [FromBody] Dictionary<string, object> updateData)
    {

        var result = await _mediator.Send(
            new UpdateCommand<TEntity, TId, TUpdateDto>(key, updateData, _service, $"{typeof(TEntity).Name}.Update"));
        return ApiActionResult(result);
    }

    [HttpDelete("master({parentName}={parentValue})/{key}")]
    public virtual async Task<ActionResult> Delete(TId key)
    {
        var result = await _mediator.Send(
            new DeleteCommand<TEntity, TId>(key, _service, $"{typeof(TEntity).Name}.Delete"));
        return ApiActionResult(result);
    }

}
