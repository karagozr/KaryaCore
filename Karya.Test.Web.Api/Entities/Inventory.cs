using Karya.Core.Abstracts.Entities;
using Karya.Core.Common.Attributes.Data;
using System.ComponentModel.DataAnnotations.Schema;

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

    public virtual List<InventoryDetail> InventoryDetails { get; set; } = new List<InventoryDetail>();

}

public class InventoryDetail : BaseTenantEntity<int>
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public override int Id { get; set; }

    public string InventoryId { get; set; }

    public string Note { get; set; } = null!;

    public string? CategoryId { get; set; } = null!;
    public string? MainCategoryId { get; set; } = null!;

    [TenantForeignKeyAttribute($"{nameof(InventoryId)}")]
    public Inventory Inventory { get; set; }

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