using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Karya.Core.Interfaces.DTOs;
using Karya.Core.Interfaces.Filters;
using Karya.Core.Results;

namespace Karya.Core.Interfaces.Services;


public interface IBaseDetailService<TEntity, TId, TParentFilter> : IBaseService
    where TParentFilter : IParentFilter, new()
{
    IQueryable<TEntity> Query(TParentFilter parentFilter);
    Task<BaseResult<TDto>> ByKey<TDto>(TParentFilter parentFilter, TId key) where TDto : class, ISingleDto, new();

    Task<BaseResult<LoadResult>> Select<TDto>(TParentFilter parentFilter, DataSourceLoadOptionsBase filterDataOptions) where TDto : class, ISelectDto, new();

    Task<BaseResult> Insert<TDto>(TParentFilter parentFilter, TDto dto) where TDto : class, IInsertDto, new();

    Task<BaseResult> Update<TDto>(TParentFilter parentFilter, TId key, Dictionary<string, object> updateData) where TDto : class, IUpdateDto, new();

    Task<BaseResult> Delete(TParentFilter parentFilter, TId key);

}
   