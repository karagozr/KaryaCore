using Karya.Core.Interfaces.Results;
using Karya.Core.Services;
using Karya.Test.Entities;

namespace Karya.Test;

public class UserService : BaseService<UserTestRepo,UserTest, string>
{

    public UserService(string userId, string tanentId) : base(new TestUoW(userId, tanentId))
    {
    }

    //public override async Task<IBaseResult<UserTest>> Insert(UserTest entity)
    //{
    //    await _uow.Repo<UserTestLogRepo>().AddAsync(new UserTestLog
    //    {
    //        Description = $"User {nameof(entity)} is being added",
    //        Id = Guid.NewGuid(),
    //        LogDate = DateTime.UtcNow
    //    });
    //    return await base.Insert(entity);
    //}


}
