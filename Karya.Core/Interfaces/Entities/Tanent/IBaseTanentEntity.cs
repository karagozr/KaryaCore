using System.ComponentModel.DataAnnotations;

namespace Karya.Core.Interfaces.Entities.Tanent;

public interface IBaseTanentEntity<TId, TTanetId>:IBaseEntity<TId>
{
    [Key]
    TTanetId TanentId { get; set; }
}

public interface IBaseTanentEntity<TTanentId> : IBaseTanentEntity<Guid, TTanentId>
{

}

public interface IBaseTanentEntity : IBaseTanentEntity<Guid, string>
{

}
