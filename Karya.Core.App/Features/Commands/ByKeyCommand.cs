using Karya.Core.App.Interfaces.Commands;
using Karya.Core.Interfaces.DTOs;
using Karya.Core.Interfaces.Services;
using Karya.Core.Results;

namespace Karya.Core.App.Features.Commands;

public record ByKeyCommand<TEntity, TId, TDto>(TId Key, IBaseService<TEntity, TId> Service, string Permission = "" ) : 
IExecutableCrudRequest<BaseResult> where TDto : class, ISingleDto, new()
{
    public Task<BaseResult> ExecuteAsync(CancellationToken ct = default) => Service.ByKey<TDto>(Key);
}