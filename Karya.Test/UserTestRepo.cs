using Karya.Core.Repositories;
using Karya.Test.Entities;
using Microsoft.EntityFrameworkCore;

namespace Karya.Test;

public class UserTestLogRepo : BaseTanentRepositoryAsync<UserTestLog, Guid, DbContext>
{
    public UserTestLogRepo(DbContext context, string userId, string tanentId) : base(context, userId, tanentId)
    {
    }
}

public class UserTestRepo : BaseTanentRepositoryAsync<UserTest, string, DbContext>
{
    public UserTestRepo(DbContext context,string userId, string tanentId) : base(context, userId, tanentId)
    {
    }

    
}
