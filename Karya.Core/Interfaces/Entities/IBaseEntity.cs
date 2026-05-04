using System.ComponentModel.DataAnnotations;

namespace Karya.Core.Interfaces.Entities;

public interface IEntity
{
}
public interface IBaseEntity<TId>: IEntity
{
    [Key]
    TId Id { get; set; }
}

public interface IBaseEntity:IBaseEntity<Guid>
{
    [Key]
    Guid Id { get; set; }
}
