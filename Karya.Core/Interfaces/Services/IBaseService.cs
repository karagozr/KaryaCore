using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Karya.Core.Interfaces.DTOs;
using Karya.Core.Results;

namespace Karya.Core.Interfaces.Services;


public interface IBaseService : IDisposable
{
    
}

public interface IBaseService<TEntity, TId> : IBaseService
{
    IQueryable<TEntity> Query();
    Task<BaseResult<TDto>> ByKey<TDto>(TId key) where TDto : class, ISingleDto, new();

    Task<BaseResult<LoadResult>> Select<TDto>(DataSourceLoadOptionsBase filterDataOptions) where TDto : class, ISelectDto, new();

    Task<BaseResult> Insert<TDto>(TDto dto) where TDto : class, IInsertDto, new();

    Task<BaseResult> Update<TDto>(TId key, Dictionary<string, object> updateData) where TDto : class, IUpdateDto, new();

    Task<BaseResult> Delete(TId key);

}
