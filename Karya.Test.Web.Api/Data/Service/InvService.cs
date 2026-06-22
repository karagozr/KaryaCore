using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Results;
using Karya.Core.Services;
using Karya.Test.Web.Api.DTOs;
using Karya.TestApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace Karya.Test.Web.Api.Data.Service;

public class InvService : BaseService<InventoryRepository, Inventory, string>
{
    public InvService(ICurrentUser currentUser) : base(new DevUnitOfWork(currentUser))
    {
    }

    public override async Task<BaseResult<LoadResult>> Select<TDto>(DataSourceLoadOptionsBase filterDataOptions)
    {
        var query = _uow.Repo<InventoryRepository>().Query(x=>x.Include(i=>i.Category)).Select(x => new InvLDto
        {
            Id = x.Id,
            Name = x.Name,
            CategoryId = x.CategoryId,
            CategoryName = x.Category.Name,
            Brand = x.Brand,
            MainCategoryId = x.MainCategoryId,
            MainCategoryName = x.MainCategory.Name
        });

        var list = await query.ToListAsync();

        var res = await DataSourceLoader.LoadAsync(query, filterDataOptions);

        return BaseResult<LoadResult>.Success("200", null, res);

    }
}


