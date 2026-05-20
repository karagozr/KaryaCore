using Karya.Core.Interfaces.DTOs;
using Karya.Core.Interfaces.Results;
using Karya.Core.Results;
using System.Linq.Expressions;

namespace Karya.Core.Interfaces.Services;


public interface IBaseService : IDisposable
{
   
}

public interface IBaseService<TEntity, TId> : IBaseService
{
    Task<BaseResult> ByKey<TDto>(TId key) where TDto : class, ISingleDto, new();

    Task<BaseResult> Select<TDto>(Expression<Func<TEntity,bool>> expression) where TDto : class, ISelectDto, new();

    Task<BaseResult> Insert<TDto>(TDto dto) where TDto : class, IInsertDto, new();

    Task<BaseResult> Update<TDto>(TId key,TDto dto) where TDto : class, IUpdateDto, new();

    Task<BaseResult> Delete(TId key);

}
