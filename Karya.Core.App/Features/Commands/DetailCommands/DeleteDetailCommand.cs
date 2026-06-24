using Karya.Core.App.Interfaces.Commands;
using Karya.Core.Interfaces.Filters;
using Karya.Core.Interfaces.Services;
using Karya.Core.Results;

namespace Karya.Core.App.Features.Commands.DetailCommands;

public record DeleteDetailCommand<TEntity, TId, TParentFilter>(
     TParentFilter ParentFilter,
    TId Key,
    IBaseDetailService<TEntity, TId, TParentFilter> Service,
    string Permission = ""
) : IExecutableCrudRequest<BaseResult>
    where TParentFilter : class, IParentFilter, new()
{
    public Task<BaseResult> ExecuteAsync(CancellationToken ct = default)
        => Service.Delete(ParentFilter, Key);
}