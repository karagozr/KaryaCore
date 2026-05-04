namespace Karya.Core.Interfaces.Entities.Tanent;

public interface ICardTanentEntity : ICardEntity, IBaseTanentEntity<string, string>
{

}

public interface ICardTanentEntity<TTanentId> : ICardEntity, IBaseTanentEntity<string, TTanentId>
{

}

public interface ICardTanentDetailEntity<TId, TTanentId> : IBaseTanentEntity<TId, TTanentId>
{
    int RowNum { get; set; }
}

public interface ICardTanentDetailEntity<TId> : ICardTanentDetailEntity<TId, string>
{

}