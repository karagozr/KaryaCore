using Karya.Core.App.Features.Commands.DetailCommands;
using Karya.Core.Interfaces.DTOs;
using Karya.Core.Interfaces.Filters;
using Karya.Core.Interfaces.Services;
using Karya.Core.Web.Helpers;
using Karya.Core.Web.Infrastructure.Binders;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Karya.Core.Web.Abstracts.Controllers;


[ApiController]
[Route("api/[controller]({parent})")]
public abstract class BaseCrudDetailController<TEntity, TId, TParentFilter, TSingleDto, TSelectDto, TInsertDto, TUpdateDto> : BaseController<TEntity,TId>
    where TId : notnull
    where TParentFilter : class, IParentFilter, new()
    where TSingleDto : class, ISingleDto, new()
    where TSelectDto : class, ISelectDto, new()
    where TInsertDto : class, IInsertDto, new()
    where TUpdateDto : class, IUpdateDto, new()
{
    protected new IBaseDetailService<TEntity, TId, TParentFilter> _service;
    protected BaseCrudDetailController(IMediator mediator, IBaseDetailService<TEntity, TId, TParentFilter> service):base(mediator, service)
    {
        _service = service;
    }

    [HttpGet("{key}")]
    public virtual async Task<ActionResult> ByKey([ModelBinder(typeof(GenericParentFilterModelBinder))] TParentFilter parent, TId key)
    {
        var result = await _mediator.Send(
            new ByKeyDetailCommand<TEntity, TId, TParentFilter, TSingleDto>(parent, key, _service, $"{typeof(TEntity).Name}.Read"));
        return ApiActionResult(result);
    }

    [HttpGet]
    public virtual async Task<ActionResult> Select([ModelBinder(typeof(GenericParentFilterModelBinder))] TParentFilter parent, DataSourceLoadOptions options)
    {
        foreach (var item in parent.GetType().GetProperties())
        {
            var ddd = item.Name; 
            var ss = item.GetValue(parent);
        }

        var result = await _mediator.Send(
            new SelectDetailCommand<TEntity, TId, TParentFilter, TSelectDto>(parent, options, _service, $"{typeof(TEntity).Name}.Read"));
        return ApiActionResult(result);
    }

    [HttpPost]
    public virtual async Task<ActionResult> Insert([ModelBinder(typeof(GenericParentFilterModelBinder))] TParentFilter parent, [FromBody] TInsertDto dto)
    {
        var result = await _mediator.Send(
            new InsertDetailCommand<TEntity, TId, TParentFilter, TInsertDto>(parent, dto, _service, $"{typeof(TEntity).Name}.Create"));
        return ApiActionResult(result);
    }

    [HttpPut("{key}")]
    public virtual async Task<ActionResult> Update([ModelBinder(typeof(GenericParentFilterModelBinder))] TParentFilter parent, TId key, [FromBody] Dictionary<string, object> updateData)
    {

        var result = await _mediator.Send(
            new UpdateDetailCommand<TEntity, TId, TParentFilter, TUpdateDto>(parent, key, updateData, _service, $"{typeof(TEntity).Name}.Update"));
        return ApiActionResult(result);
    }

    [HttpDelete("{key}")]
    public virtual async Task<ActionResult> Delete([ModelBinder(typeof(GenericParentFilterModelBinder))] TParentFilter parent, TId key)
    {
        var result = await _mediator.Send(
            new DeleteDetailCommand<TEntity, TId, TParentFilter>(parent, key, _service, $"{typeof(TEntity).Name}.Delete"));
        return ApiActionResult(result);
    }

}
