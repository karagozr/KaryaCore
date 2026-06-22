using Karya.Core.Abstracts.Entities;
using Karya.Core.Common.Attributes.Data;

namespace Karya.TestApi.Entities;

public class Inventory:BaseTenantEntity<string>
{
    public string Name { get; set; } = null!;
    public string Brand { get; set; } = null!;
    public string CategoryId { get; set; } = null!;
    public string? MainCategoryId { get; set; } = null!;

    [TenantForeignKeyAttribute($"{nameof(CategoryId)}")]
    public InventoryCategory? Category { get; set; }

    [TenantForeignKeyAttribute($"{nameof(MainCategoryId)}")]
    public InventoryMainCategory? MainCategory { get; set; }

}

public class InventoryCategory : BaseTenantEntity<string>
{
    public string Name { get; set; } = null!;

    public string? MainCategoryId { get; set; } = null!;
    
    [TenantForeignKeyAttribute($"{nameof(MainCategoryId)}")]
    public InventoryMainCategory? MainCategory { get; set; }

}

public class InventoryMainCategory : BaseTenantEntity<string>
{
    public string Name { get; set; } = null!;

}