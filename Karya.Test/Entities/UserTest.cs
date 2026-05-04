using Karya.Core.Abstracts.Entities;
using Karya.Core.Interfaces.Entities;
using Karya.Core.Interfaces.Entities.Tanent;
using System.ComponentModel.DataAnnotations;

namespace Karya.Test.Entities;

public class UserTest : BaseTanentEntity<string>, IVersionable,ISoftDelete
{
    [StringLength(10)]
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Version { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
}