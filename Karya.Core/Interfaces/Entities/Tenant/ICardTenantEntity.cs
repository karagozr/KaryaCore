namespace Karya.Core.Interfaces.Entities.Tanent;

public interface ICardTenantEntity : ICardEntity, IBaseTenantEntity<string, string>
{

}

public interface ICardTanentEntity<TTanentId> : ICardEntity, IBaseTenantEntity<string, TTanentId>
{

}

public interface ICardTanentDetailEntity<TId, TTanentId> : IBaseTenantEntity<TId, TTanentId>
{
    int RowNum { get; set; }
}

public interface ICardTanentDetailEntity<TId> : ICardTanentDetailEntity<TId, string>
{

}