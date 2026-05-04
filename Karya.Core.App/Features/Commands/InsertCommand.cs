using Karya.Core.App.Interfaces.Commands;
using Karya.Core.Interfaces.DTOs;
using Karya.Core.Interfaces.Results;
using Karya.Core.Interfaces.Services;
using System.Linq.Expressions;

namespace Karya.Core.App.Features.Commands;

public record InsertCommand<TEntity, TId, TDto>(
    TDto Dto,
    IBaseService<TEntity, TId> Service,
    string Permission = ""
) : IExecutableCrudRequest<IBaseResult<TDto>>
    where TDto : class, IInsertDto, new()
{
    public Task<IBaseResult<TDto>> ExecuteAsync(CancellationToken ct = default)
        => Service.Insert<TDto>(Dto);
}