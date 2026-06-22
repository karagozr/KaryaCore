using Karya.Core.App.Interfaces.Commands;
using Karya.Core.Interfaces.DTOs;
using Karya.Core.Interfaces.Services;
using Karya.Core.Results;

namespace Karya.Core.App.Features.Commands;

public record UpdateCommand<TEntity, TId, TDto>(
    TId Key,
    Dictionary<string, object> updateData,
    IBaseService<TEntity, TId> Service,
    string Permission = ""
) : IExecutableCrudRequest<BaseResult>
    where TDto : class, IUpdateDto, new()
{
    public Task<BaseResult> ExecuteAsync(CancellationToken ct = default)
        => Service.Update<TDto>(Key, updateData);
}