using Karya.Core.Interfaces.DTOs;

namespace Karya.Test.Web.Api.DTOs;

public class InvLDto : ISelectDto
{
    public string? Id { get; set; }=null!;
    public string? Name { get; set; } = null!;
    public string? Brand { get; set; } = null!;
    public string? CategoryName { get; set; } = null!;
    public string? CategoryId { get; set; } = null!;

    public string? MainCategoryName { get; set; } = null!;
    public string? MainCategoryId { get; set; } = null!;
}

public class InvDetailLDto : ISelectDto
{
    public int Id { get; set; }=0;
    public string InventoryId { get; set; } = null!;
    public string Note { get; set; } = null!;
    public string? CategoryName { get; set; } = null!;
    public string? CategoryId { get; set; } = null!;

    public string? MainCategoryName { get; set; } = null!;
    public string? MainCategoryId { get; set; } = null!;
}

public class InvCategoryLDto : ISelectDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? MainCategoryName { get; set; } = null!;
    public string? MainCategoryId { get; set; } = null!;
}

public class InvMainCategoryLDto : ISelectDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
}