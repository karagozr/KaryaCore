//using Karya.Core.Interfaces.DTOs;
//using Karya.Core.Interfaces.Entities;
//using Karya.Core.Interfaces.Identities;
//using Karya.Core.Interfaces.Results;
//using Karya.Core.Interfaces.Services;
//using Karya.Core.Web.Returns.Api;
//using Microsoft.AspNetCore.Mvc;

//namespace Karya.Core.Web.Controllers.Abstracts;

//[ApiController]
//[Route("api/[controller]")]
//public abstract class GenericBaseController<TEntity, TId, TSingleDto, TSelectDto, TInsertDto, TUpdateDto> : ControllerBase
// where TEntity : class, IBaseEntity<TId>
// where TId : notnull
// where TSingleDto : class, ISingleDto, new()
// where TSelectDto : class, ISelectDto, new()
// where TInsertDto : class, IInsertDto, new()
// where TUpdateDto : class, IUpdateDto, new()
//{
//    protected readonly IBaseService<TEntity, TId> _service;
//    protected readonly ICurrentUser _currentUser;

//    public GenericBaseController(ICurrentUser currentUser, IBaseService<TEntity, TId> service)
//    {
//        _currentUser = currentUser;
//        _service = service;
//    }

//    [HttpGet("{key}")]
//    public virtual async Task<ActionResult> ByKey(TId key) => new ApiResult<TSingleDto>(await _service.ByKey<TSingleDto>(key));

//    [HttpGet("/select")]
//    public virtual async Task<ActionResult> Select() => new ApiResult<IEnumerable<TSelectDto>>(await _service.Select<TSelectDto>(x => true));

//    [HttpPost]
//    public virtual async Task<ActionResult> Insert(TInsertDto dto) => new ApiResult<TInsertDto>(await _service.Insert(dto));

//    [HttpPut("{key}")]
//    public virtual async Task<ActionResult> Update(TId key, TUpdateDto dto) => new ApiResult<TUpdateDto>(await _service.Update(key, dto));

//    [HttpDelete("{key}")]
//    public virtual async Task<ActionResult> Delete(TId key) => new ApiResult(await _service.Delete(key));

//    protected ApiResult<TData> ApiActionResult<TData>(IBaseResult<TData> result) => new ApiResult<TData>(result);

//    protected ApiResult ApiActionResult(IBaseResult result) => new ApiResult(result);
    
//}
