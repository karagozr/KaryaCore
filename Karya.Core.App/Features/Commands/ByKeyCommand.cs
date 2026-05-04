using Karya.Core.App.Interfaces.Commands;
using Karya.Core.Interfaces.DTOs;
using Karya.Core.Interfaces.Results;
using Karya.Core.Interfaces.Services;

namespace Karya.Core.App.Features.Commands;

public record ByKeyCommand<TEntity, TId, TDto>(TId Key, IBaseService<TEntity, TId> Service, string Permission = "" ) : 
IExecutableCrudRequest<IBaseResult<TDto>> where TDto : class, ISingleDto, new()
{
    public Task<IBaseResult<TDto>> ExecuteAsync(CancellationToken ct = default) => Service.ByKey<TDto>(Key);
}