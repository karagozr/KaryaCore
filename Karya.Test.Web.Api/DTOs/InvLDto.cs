using Karya.Core.Interfaces.DTOs;

namespace Karya.Test.Web.Api.DTOs;

public class InvLDto : ISelectDto
{
    public string? Id { get; set; }=null!;
    public string? Name { get; set; } = null!;
    public string? Brand { get; set; } = null!;
    public string? CategoryId { get; set; } = null!;
}


public class InvCategoryLDto : ISelectDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
}