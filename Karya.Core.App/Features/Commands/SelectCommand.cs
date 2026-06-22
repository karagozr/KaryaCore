using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Karya.Core.App.Interfaces.Commands;
using Karya.Core.Interfaces.DTOs;
using Karya.Core.Interfaces.Services;
using Karya.Core.Results;

namespace Karya.Core.App.Features.Commands;

public record SelectCommand<TEntity, TId, TDto>(DataSourceLoadOptionsBase LoadOptions, IBaseService<TEntity, TId> Service, string Permission = "")
    : IExecutableCrudRequest<BaseResult<LoadResult>> where TDto : class, ISelectDto, new()
{
    public async Task<BaseResult<LoadResult>> ExecuteAsync(CancellationToken ct = default) 
    {
        return await Service.Select<TDto>(LoadOptions);
    }
}
