using Karya.Core.App.Interfaces.Commands;
using Karya.Core.Common.Data;
using Karya.Core.Interfaces.DTOs;
using Karya.Core.Interfaces.Services;
using Karya.Core.Results;
using System.Linq.Expressions;

namespace Karya.Core.App.Features.Commands;

public record SelectCommand<TEntity, TId, TDto>(FilterDataOptions<TEntity> filterDataOptions, IBaseService<TEntity, TId> Service, string Permission = "") 
    : IExecutableCrudRequest<BaseResult>
    where TDto : class, ISelectDto, new()
{
    public Task<BaseResult> ExecuteAsync(CancellationToken ct = default)
        => Service.Select<TDto>(filterDataOptions);
}
