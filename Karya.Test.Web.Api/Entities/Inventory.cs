using Karya.Core.Abstracts.Entities;
using Karya.Core.Common.Attributes.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karya.TestApi.Entities;

public class Inventory : BaseTenantEntity<string>
{
    public string Name { get; set; } = null!;
    public string Brand { get; set; } = null!;

    public string CategoryId { get; set; } = null!;
    public string? MainCategoryId { get; set; }

    [TenantForeignKey(nameof(CategoryId))]
    public InventoryCategory? Category { get; set; }

    [TenantForeignKey(nameof(MainCategoryId))]
    public InventoryMainCategory? MainCategory { get; set; }

    public virtual List<InventoryDetail> InventoryDetails { get; set; } = new();
}

public class InventoryDetail : BaseTenantEntity<int>
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public override int Id { get; set; }

    public string InventoryId { get; set; } = null!;
    public string Note { get; set; } = null!;
    public string? CategoryId { get; set; }
    public string? MainCategoryId { get; set; }

    [TenantForeignKey(nameof(InventoryId))]
    public Inventory Inventory { get; set; } = null!;

    [TenantForeignKey(nameof(CategoryId))]
    public InventoryCategory? Category { get; set; }

    [TenantForeignKey(nameof(MainCategoryId))]
    public InventoryMainCategory? MainCategory { get; set; }
}

public class InventoryCategory : BaseTenantEntity<string>
{
    public string Name { get; set; } = null!;
    public string? MainCategoryId { get; set; }

    [TenantForeignKey(nameof(MainCategoryId))]
    public InventoryMainCategory? MainCategory { get; set; }
}

public class InventoryMainCategory : BaseTenantEntity<string>
{
    public string Name { get; set; } = null!;
}