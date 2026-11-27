using Karya.Core.App.Interfaces.Commands;
using Karya.Core.Interfaces.DTOs;
using Karya.Core.Interfaces.Filters;
using Karya.Core.Interfaces.Services;
using Karya.Core.Results;

namespace Karya.Core.App.Features.Commands.DetailCommands;

public record InsertDetailCommand<TEntity, TId, TParentFilter, TDto, TrDto>(
     TParentFilter ParentFilter,
    TDto Dto,
    IBaseDetailService<TEntity, TId, TParentFilter> Service,
    string Permission = ""
) : IExecutableCrudRequest<BaseResult<TrDto>>
    where TDto : class, IInsertDto, new()
    where TrDto : class, IByKeyDto, new()
    where TParentFilter : class, IParentFilter, new()
{
    public Task<BaseResult<TrDto>> ExecuteAsync(CancellationToken ct = default)
        => Service.Insert<TDto, TrDto>(ParentFilter, Dto);
}