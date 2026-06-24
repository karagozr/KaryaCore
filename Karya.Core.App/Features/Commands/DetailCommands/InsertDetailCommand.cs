using Karya.Core.App.Interfaces.Commands;
using Karya.Core.Interfaces.DTOs;
using Karya.Core.Interfaces.Filters;
using Karya.Core.Interfaces.Services;
using Karya.Core.Results;

namespace Karya.Core.App.Features.Commands.DetailCommands;

public record InsertDetailCommand<TEntity, TId, TParentFilter, TDto>(
     TParentFilter ParentFilter,
    TDto Dto,
    IBaseDetailService<TEntity, TId, TParentFilter> Service,
    string Permission = ""
) : IExecutableCrudRequest<BaseResult>
    where TDto : class, IInsertDto, new()
    where TParentFilter : class, IParentFilter, new()
{
    public Task<BaseResult> ExecuteAsync(CancellationToken ct = default)
        => Service.Insert(ParentFilter, Dto);
}