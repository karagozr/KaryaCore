using Karya.Core.App.Interfaces.Commands;
using Karya.Core.Interfaces.DTOs;
using Karya.Core.Interfaces.Filters;
using Karya.Core.Interfaces.Services;
using Karya.Core.Results;

namespace Karya.Core.App.Features.Commands.DetailCommands;

public record UpdateDetailCommand<TEntity, TId, TParentFilter, TDto>(
    TParentFilter ParentFilter,
    TId Key,
    Dictionary<string, object> updateData,
    IBaseDetailService<TEntity, TId, TParentFilter> Service,
    string Permission = ""
) : IExecutableCrudRequest<BaseResult>
    where TDto : class, IUpdateDto, new()
    where TParentFilter : class, IParentFilter, new()
{
    public Task<BaseResult> ExecuteAsync(CancellationToken ct = default)
        => Service.Update<TDto>(ParentFilter, Key, updateData);
}