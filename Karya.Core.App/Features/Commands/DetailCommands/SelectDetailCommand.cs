using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Karya.Core.App.Interfaces.Commands;
using Karya.Core.Interfaces.DTOs;
using Karya.Core.Interfaces.Filters;
using Karya.Core.Interfaces.Services;
using Karya.Core.Results;

namespace Karya.Core.App.Features.Commands.DetailCommands;

public record SelectDetailCommand<TEntity, TId, TParentFilter, TDto>(
    TParentFilter ParentFilter, 
    DataSourceLoadOptionsBase LoadOptions, 
    IBaseDetailService<TEntity, TId, TParentFilter> Service, 
    string Permission = "")
    : IExecutableCrudRequest<BaseResult<LoadResult>> where TDto : class, ISelectDto, new()
    where TParentFilter : class, IParentFilter, new()
{
    public async Task<BaseResult<LoadResult>> ExecuteAsync(CancellationToken ct = default)
    {
        return await Service.Select<TDto>(ParentFilter, LoadOptions);
    }
}

