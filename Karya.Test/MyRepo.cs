using Karya.Core.Abstracts.Entities;
using Karya.Core.Repositories;

namespace Karya.Test;

public abstract class MyRepo<TEntity> : BaseTanentRepositoryAsync<TEntity, string, TestContext>
        where TEntity : BaseTanentEntity<string>, new()
{
    public MyRepo(string userId, string tanentId) : base(new TestContext(), userId, tanentId)
    {
    }



}
