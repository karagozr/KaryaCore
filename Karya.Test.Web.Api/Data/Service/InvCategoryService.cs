using Karya.Core.Interfaces.Identities;
using Karya.Core.Services;
using Karya.TestApi.Entities;

namespace Karya.Test.Web.Api.Data.Service;

public class InvCategoryService(ICurrentUser currentUser) 
    : BaseService<InventoryCategoryRepository, InventoryCategory, string>(new DevUnitOfWork(currentUser))
{
}
