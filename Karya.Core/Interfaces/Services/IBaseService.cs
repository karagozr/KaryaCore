using Karya.Core.Interfaces.DTOs;
using Karya.Core.Interfaces.Results;
using System.Linq.Expressions;

namespace Karya.Core.Interfaces.Services;


public interface IBaseService : IDisposable
{
   
}

public interface IBaseService<TEntity, TId> : IBaseService
{
    Task<IBaseResult<TDto>> ByKey<TDto>(TId key) where TDto : class, ISingleDto, new();

    Task<IBaseResult<IEnumerable<TDto>>> Select<TDto>(Expression<Func<TEntity,bool>> expression) where TDto : class, ISelectDto, new();

    Task<IBaseResult<TDto>> Insert<TDto>(TDto dto) where TDto : class, IInsertDto, new();

    Task<IBaseResult<TDto>> Update<TDto>(TId key,TDto dto) where TDto : class, IUpdateDto, new();

    Task<IBaseResult> Delete(TId key);

}
