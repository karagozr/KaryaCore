using Karya.Core.Interfaces.Entities.Tanent;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karya.Core.Abstracts.Entities;

[PrimaryKey(nameof(TenantId), nameof(Id))]
public abstract class BaseTenantEntity<TId> : BaseEntity<TId>, IBaseTenantEntity<TId, string>
{
    [Required]
    [MinLength(8)]
    [MaxLength(10)]
    [Column(Order = 1)]
    public string TenantId { get; set; }
}

public abstract class BaseTanentEntity : BaseTenantEntity<Guid>
{

}
