using Karya.Core.Interfaces.DTOs;

namespace Karya.Test.Web.Api.DTOs;

public class InvADto:IInsertDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Brand { get; set; } = null!;
    public string? CategoryId { get; set; } = null!;
}


public class InvCategoryADto : IInsertDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
}
