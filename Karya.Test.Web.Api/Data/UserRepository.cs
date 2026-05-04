using Karya.Core.Interfaces.Identities;
using Karya.Core.Repositories;
using Karya.Test.Web.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Karya.Test.Web.Api.Data;

public class UserRepository : BaseRepositoryAsync<User, string, DbContext>
{
    public UserRepository(DbContext context, ICurrentUser currentUser) : base(context, currentUser)
    {
    }
}
