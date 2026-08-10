using Karya.Core.Abstracts.Entities;
using Karya.Core.Interfaces.Entities;

namespace Karya.Test.Web.Api.Entities;

public class User : BaseEntity<string>, ISoftDelete
{
    public string Password { get; set; }

    public string Name { get; set; }
    public string Email { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
}
