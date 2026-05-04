using Karya.Core.Abstracts.Entities;

namespace Karya.TestApi.Entities;

public class Inventory:BaseTanentEntity<string>
{
    public string Name { get; set; } = null!;
    public string Brand { get; set; } = null!;
    public string CategoryId { get; set; } = null!;
    public InventoryCategory? Category { get; set; }

}

public class InventoryCategory : BaseTanentEntity<string>
{
    public string Name { get; set; } = null!;

}