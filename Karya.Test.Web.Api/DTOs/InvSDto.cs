using Karya.Core.Interfaces.DTOs;

namespace Karya.Test.Web.Api.DTOs;

public class InvSDto : IByKeyDto
{
    public string Id { get; set; } = null!;
    public string? Name { get; set; } = null!;
    public string? Brand { get; set; } = null!;
    public string? CategoryId { get; set; } = null!;
    public string? MainCategoryId { get; set; } = null!;
}

public class InvDetailSDto : IByKeyDto
{
    public int Id { get; set; } = 0;
    public string InventoryId { get; set; } = null!;
    public string Note { get; set; } = null!;
    public string? CategoryId { get; set; } = null!;
    public string? MainCategoryId { get; set; } = null!;
}

public class InvCategorySDto : IByKeyDto
{
    public string Id { get; set; } = null!;
    public string? Name { get; set; } = null!;
    public string? MainCategoryId { get; set; } = null!;
}

public class InvMainCategorySDto : IByKeyDto
{
    public string Id { get; set; } = null!;
    public string? Name { get; set; } = null!;
}