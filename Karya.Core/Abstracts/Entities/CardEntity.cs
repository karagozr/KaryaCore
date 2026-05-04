using Karya.Core.Interfaces.Entities;

namespace Karya.Core.Abstracts.Entities;

public abstract class CardEntity : ICardEntity
{
    public abstract bool IsActive { get; set; }
    public abstract DateTimeOffset ValidFrom { get; set; }
    public abstract DateTimeOffset ValidUntil { get; set; }
    public abstract string Id { get; set; }
}
