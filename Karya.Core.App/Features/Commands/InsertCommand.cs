using Karya.Core.App.Interfaces.Commands;
using Karya.Core.Interfaces.DTOs;
using Karya.Core.Interfaces.Services;
using Karya.Core.Results;

namespace Karya.Core.App.Features.Commands;

public record InsertCommand<TEntity, TId, TDto,TrDto>(
    TDto Dto,
    IBaseService<TEntity, TId> Service,
    string Permission = ""
) : IExecutableCrudRequest<BaseResult<TrDto>>
    where TDto : class, IInsertDto, new()
    where TrDto : class, IByKeyDto, new()
{
    public Task<BaseResult<TrDto>> ExecuteAsync(CancellationToken ct = default)
        => Service.Insert<TDto, TrDto>(Dto);
}
