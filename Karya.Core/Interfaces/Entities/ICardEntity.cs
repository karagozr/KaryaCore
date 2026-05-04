namespace Karya.Core.Interfaces.Entities;

public interface ICardEntity : IBaseEntity<string>
{
    bool IsActive { get; set; }
    DateTimeOffset ValidFrom { get; set; }
    DateTimeOffset ValidUntil { get; set; }
}

public interface ICardDetailEntity<TId> : IBaseEntity<TId>
{
    int RowNum { get; set; }
}

