using Karya.Core.Common.Data;
using Karya.Core.Interfaces.DTOs;
using Karya.Core.Results;

namespace Karya.Core.Interfaces.Services;


public interface IBaseService : IDisposable
{
    
}

public interface IBaseService<TEntity, TId> : IBaseService
{
    IQueryable<TEntity> Query();
    Task<BaseResult> ByKey<TDto>(TId key) where TDto : class, ISingleDto, new();

    Task<BaseResult> Select<TDto>(FilterDataOptions<TEntity> filterDataOptions) where TDto : class, ISelectDto, new();

    Task<BaseResult> Insert<TDto>(TDto dto) where TDto : class, IInsertDto, new();

    Task<BaseResult> Update<TDto>(TId key,TDto dto) where TDto : class, IUpdateDto, new();

    Task<BaseResult> Delete(TId key);

}
