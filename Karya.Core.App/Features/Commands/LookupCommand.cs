using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Karya.Core.App.Interfaces.Commands;
using Karya.Core.Interfaces.DTOs;
using Karya.Core.Interfaces.Services;
using Karya.Core.Results;
using System.Linq.Expressions;

namespace Karya.Core.App.Features.Commands;

public record LookupCommand<TEntity, TId, TDto>(DataSourceLoadOptionsBase LoadOptions, IBaseService<TEntity, TId> Service, string Permission = "", Expression<Func<TEntity, bool>>? Predicate = null)
    : IExecutableCrudRequest<BaseResult<LoadResult>> where TDto : class, ILookupDto, new()
{
    public async Task<BaseResult<LoadResult>> ExecuteAsync(CancellationToken ct = default)
    {
        return await Service.Lookup<TDto>(LoadOptions, Predicate);
    }
}
