using Karya.Core.Interfaces.Identities;
using Karya.Core.Repositories;

namespace Karya.Test.Web.Api.Data;


public class DevUnitOfWork : BaseUnitOfWork
{
    public DevUnitOfWork(ICurrentUser currentUser) : base(new DevContext(), currentUser)
    {
    }
}