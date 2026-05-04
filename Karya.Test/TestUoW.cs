using Karya.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Karya.Test;

public class TestUoW : BaseUnitOfWork
{

    public TestUoW(string userId,string tanentId) : base(new TestContext(), userId,tanentId) 
    {
        
    }

}
