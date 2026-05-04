using Karya.Test;
using Karya.Test.Entities;

Console.WriteLine("Hello, World!");
//var uow = new TestUoW("test_usr","COMP01");
var service = new UserService("test_usr", "COMP02");

var _tId = "COMP02";

var item = new UserTest()
{
    Id = "0004",
    FirstName = "lllll",
    LastName = "cdcdcd"
};

var item2 = new UserTest()
{
    Id = "0002",
    FirstName = "woow",
    LastName = "cooc"
};

var ss = await service.Insert(item);

var list = new List<UserTest>() { item, item2 };

//var dd = await testrepo.GetAsync(withDeleted:false,enableTracking:false);
//await testrepo.UndeleteRangeAsync(dd.Select(x=>x.Id));

//await testrepo.UndeleteRangeAsync(dd.Select(x=>x.Id));
//await testrepo.DeleteAsync("0001");

//await testrepo.UpdateAsync(item, ["FirstName", "LastName"]);

//await testrepo.UpdateAsync(item2, ["FirstName", "LastName"]);

//var res =  await uow.CompleteAsync();
//var items = await testrepo.GetByIdAsync("0003");

Console.WriteLine("Item added successfully!");


