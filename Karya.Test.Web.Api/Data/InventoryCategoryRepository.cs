using Karya.Core.Interfaces.Identities;
using Karya.Core.Repositories;
using Karya.TestApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace Karya.Test.Web.Api.Data
{
    public class InventoryCategoryRepository : BaseTanentRepositoryAsync<InventoryCategory, string, DbContext>
    {
        public InventoryCategoryRepository(DbContext context, ICurrentUser currentUser) : base(context, currentUser)
        {
        }
    }
}
