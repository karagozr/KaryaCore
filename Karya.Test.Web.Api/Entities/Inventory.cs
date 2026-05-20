using Karya.Core.Abstracts.Entities;

namespace Karya.TestApi.Entities;

public class Inventory:BaseTenantEntity<string>
{
    public string Name { get; set; } = null!;
    public string Brand { get; set; } = null!;
    public string CategoryId { get; set; } = null!;
    public InventoryCategory? Category { get; set; }

}

public class InventoryCategory : BaseTenantEntity<string>
{
    public string Name { get; set; } = null!;

}