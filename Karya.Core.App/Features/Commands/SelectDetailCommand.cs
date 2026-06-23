using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Karya.Core.App.Interfaces.Commands;
using Karya.Core.Interfaces.DTOs;
using Karya.Core.Interfaces.Services;
using Karya.Core.Results;
using System.Collections;

namespace Karya.Core.App.Features.Commands;

public record SelectDetailCommand<TEntity, TId,TParentId, TDto>(string parentFieldName,TParentId value, DataSourceLoadOptionsBase LoadOptions, IBaseService<TEntity, TId> Service, string Permission = "")
    : IExecutableCrudRequest<BaseResult<LoadResult>> where TDto : class, ISelectDto, new()
{
    public async Task<BaseResult<LoadResult>> ExecuteAsync(CancellationToken ct = default) 
    {
        if (LoadOptions.Filter == null)
        {
            LoadOptions.Filter = new ArrayList { parentFieldName, "=", value };
        }
        else
        { 
            var combinedFilter = new ArrayList
            {
                new ArrayList { parentFieldName, "=", value },
                "and",
                LoadOptions.Filter
            };

            LoadOptions.Filter = combinedFilter;
        }
        return await Service.Select<TDto>(LoadOptions);
    }
}
