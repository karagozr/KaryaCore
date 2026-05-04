using Karya.Core.App.Interfaces.Commands;
using Karya.Core.Interfaces.DTOs;
using Karya.Core.Interfaces.Results;
using Karya.Core.Interfaces.Services;

namespace Karya.Core.App.Features.Commands;

public record UpdateCommand<TEntity, TId, TDto>(
    TId Key,
    TDto Dto,
    IBaseService<TEntity, TId> Service,
    string Permission = ""
) : IExecutableCrudRequest<IBaseResult<TDto>>
    where TDto : class, IUpdateDto, new()
{
    public Task<IBaseResult<TDto>> ExecuteAsync(CancellationToken ct = default)
        => Service.Update<TDto>(Key, Dto);
}