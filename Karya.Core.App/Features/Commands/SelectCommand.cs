using Karya.Core.App.Interfaces.Commands;
using Karya.Core.Interfaces.DTOs;
using Karya.Core.Interfaces.Results;
using Karya.Core.Interfaces.Services;
using System.Linq.Expressions;

namespace Karya.Core.App.Features.Commands;

public record SelectCommand<TEntity, TId, TDto>(
    Expression<Func<TEntity, bool>> Filter,
    IBaseService<TEntity, TId> Service,
    string Permission = ""
) : IExecutableCrudRequest<IBaseResult<IEnumerable<TDto>>>
    where TDto : class, ISelectDto, new()
{
    public Task<IBaseResult<IEnumerable<TDto>>> ExecuteAsync(CancellationToken ct = default)
        => Service.Select<TDto>(Filter);
}