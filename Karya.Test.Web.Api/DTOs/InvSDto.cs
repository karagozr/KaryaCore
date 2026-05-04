using Karya.Core.Interfaces.DTOs;

namespace Karya.Test.Web.Api.DTOs;

public class InvSDto : ISingleDto
{
    public string Id { get; set; } = null!;
    public string? Name { get; set; } = null!;
    public string? Brand { get; set; } = null!;
    public string? CategoryId { get; set; } = null!;
}

public class InvCategorySDto : ISingleDto
{
    public string Id { get; set; } = null!;
    public string? Name { get; set; } = null!;
}

