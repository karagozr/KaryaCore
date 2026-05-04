using Karya.Core.Interfaces.Identities;
using Karya.Core.Services;
using Karya.TestApi.Entities;

namespace Karya.Test.Web.Api.Data.Service;

public class InvService : BaseService<InventoryRepository, Inventory, string>
{
    public InvService(ICurrentUser currentUser) : base(new DevUnitOfWork(currentUser))
    {
    }
}


