using Karya.Core.Interfaces.Identities;
using Karya.Core.Repositories;
using Karya.Test.Web.Api.Controllers;
using Karya.TestApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace Karya.Test.Web.Api.Data
{
    public class InventoryRepository : BaseTenantRepositoryAsync<Inventory, string, DbContext>
    {
        public InventoryRepository(DbContext context, ICurrentUser currentUser) : base(context, currentUser)
        {
        }
    }

    public class InventoryDetailRepository : BaseTenantDetailRepositoryAsync<InventoryDetail, int, InvParentFilter, DbContext>
    {
        public InventoryDetailRepository(DbContext context, ICurrentUser currentUser, InvParentFilter parentFilter) : base(context, currentUser, parentFilter)
        {
        }
    }
}
