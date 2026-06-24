using Karya.Core.App.Interfaces.Commands;
using Karya.Core.Interfaces.DTOs;
using Karya.Core.Interfaces.Services;
using Karya.Core.Results;

namespace Karya.Core.App.Features.Commands;

public record InsertCommand<TEntity, TId, TDto>(
    TDto Dto,
    IBaseService<TEntity, TId> Service,
    string Permission = ""
) : IExecutableCrudRequest<BaseResult>
    where TDto : class, IInsertDto, new()
{
    public Task<BaseResult> ExecuteAsync(CancellationToken ct = default)
        => Service.Insert(Dto);
}
