using Karya.Core.App.Interfaces.Commands;
using Karya.Core.Interfaces.Results;
using Karya.Core.Interfaces.Services;

namespace Karya.Core.App.Features.Commands;

public record DeleteCommand<TEntity, TId>(
    TId Key,
    IBaseService<TEntity, TId> Service,
    string Permission = ""
) : IExecutableCrudRequest<IBaseResult>
{
    public Task<IBaseResult> ExecuteAsync(CancellationToken ct = default)
        => Service.Delete(Key);
}