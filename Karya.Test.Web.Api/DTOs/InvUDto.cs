using Karya.Core.Interfaces.DTOs;

namespace Karya.Test.Web.Api.DTOs;

public class InvUDto:IUpdateDto
{
    public string? Name { get; set; } = null!;
    public string? Brand { get; set; } = null!;
    public string? CategoryId { get; set; } = null!;
    public string? MainCategoryId { get; set; } = null!;

}

public class InvCategoryUDto : IUpdateDto
{
    public string? Name { get; set; } = null!;
    public string? MainCategoryId { get; set; } = null!;

}

public class InvMainCategoryUDto : IUpdateDto
{
    public string? Name { get; set; } = null!;

}
