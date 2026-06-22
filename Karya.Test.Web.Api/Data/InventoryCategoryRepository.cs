using Karya.Core.Interfaces.Identities;
using Karya.Core.Repositories;
using Karya.TestApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace Karya.Test.Web.Api.Data;

public class InventoryCategoryRepository : BaseTenantRepositoryAsync<InventoryCategory, string, DbContext>
{
    public InventoryCategoryRepository(DbContext context, ICurrentUser currentUser) : base(context, currentUser)
    {
    }
}

public class InventoryMainCategoryRepository : BaseTenantRepositoryAsync<InventoryMainCategory, string, DbContext>
{
    public InventoryMainCategoryRepository(DbContext context, ICurrentUser currentUser) : base(context, currentUser)
    {
    }
}
