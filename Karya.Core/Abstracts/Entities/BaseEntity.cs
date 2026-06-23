using Karya.Core.Interfaces.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karya.Core.Abstracts.Entities;

public abstract class BaseEntity<TId> : IBaseEntity<TId>
{
    [Key, Column(Order = 0)]
    public virtual TId Id { get; set; }
}

public abstract class BaseEntity : BaseEntity<Guid>
{
    protected BaseEntity() => Id = Guid.NewGuid();
}





