using System.ComponentModel.DataAnnotations;

namespace Karya.Core.Interfaces.Entities.Tanent;

public interface IBaseTenantEntity<TId, TTanetId>:IBaseEntity<TId>
{
    [Key]
    TTanetId TenantId { get; set; }
}

public interface IBaseTenantEntity<TTanentId> : IBaseTenantEntity<Guid, TTanentId>
{

}

public interface IBaseTenantEntity : IBaseTenantEntity<Guid, string>
{

}
