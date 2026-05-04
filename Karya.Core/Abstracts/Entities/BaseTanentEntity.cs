using Karya.Core.Interfaces.Entities.Tanent;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karya.Core.Abstracts.Entities;

[PrimaryKey(nameof(TanentId), nameof(Id))]
public abstract class BaseTanentEntity<TId> : BaseEntity<TId>, IBaseTanentEntity<TId, string>
{
    [Column(Order = 1)]
    public string TanentId { get; set; }
}

public abstract class BaseTanentEntity : BaseTanentEntity<Guid>
{

}
