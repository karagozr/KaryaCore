using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Results;
using Karya.Core.Services;
using Karya.Test.Web.Api.DTOs;
using Karya.TestApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace Karya.Test.Web.Api.Data.Service;

public class InvCategoryService(ICurrentUser currentUser) 
    : BaseService<InventoryCategoryRepository, InventoryCategory, string>(new DevUnitOfWork(currentUser))
{
    public override async Task<BaseResult<LoadResult>> Select<TDto>(DataSourceLoadOptionsBase filterDataOptions)
    {
        var query = _uow.Repo<InventoryCategoryRepository>().Query(x => x.Include(i => i.MainCategory))
            .Select(x => new InvCategoryLDto
        {
            Id = x.Id,
            Name = x.Name,
            MainCategoryId = x.MainCategoryId,
            MainCategoryName = x.MainCategory.Name
        });

        var list = await query.ToListAsync();

        var res = await DataSourceLoader.LoadAsync(query, filterDataOptions);

        return BaseResult<LoadResult>.Success("200", null, res);

    }
}


public class InvMainCategoryService(ICurrentUser currentUser)
    : BaseService<InventoryMainCategoryRepository, InventoryMainCategory, string>(new DevUnitOfWork(currentUser))
{
}
