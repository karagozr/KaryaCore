using Karya.Core.App.Interfaces.Commands;
using Karya.Core.Interfaces.Services;
using Karya.Core.Results;

namespace Karya.Core.App.Features.Commands;

public record DeleteCommand<TEntity, TId>(
    TId Key,
    IBaseService<TEntity, TId> Service,
    string Permission = ""
) : IExecutableCrudRequest<BaseResult>
{
    public Task<BaseResult> ExecuteAsync(CancellationToken ct = default)
        => Service.Delete(Key);
}
