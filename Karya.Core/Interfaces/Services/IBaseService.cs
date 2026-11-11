using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Karya.Core.Interfaces.DTOs;
using Karya.Core.Results;
using System.Linq.Expressions;

namespace Karya.Core.Interfaces.Services;


public interface IBaseService : IDisposable
{
    
}

public interface IBaseService<TEntity, TId> : IBaseService
{
    IQueryable<TEntity> Query();
    Task<BaseResult<TDto>> ByKey<TDto>(TId key) where TDto : class, IByKeyDto, new();

    Task<BaseResult<LoadResult>> Select<TDto>(DataSourceLoadOptionsBase filterDataOptions) where TDto : class, ISelectDto, new();

    Task<BaseResult<LoadResult>> Lookup<TDto>(DataSourceLoadOptionsBase filterDataOptions, Expression<Func<TEntity, bool>>? predicate = null) where TDto : class, ISelectDto, new();

    Task<BaseResult> Insert<TDto>(TDto dto) where TDto : class, IInsertDto, new();

    Task<BaseResult> Update<TDto>(TId key, Dictionary<string, object> updateData) where TDto : class, IUpdateDto, new();

    Task<BaseResult> Delete(TId key);

}
