namespace Karya.Core.Interfaces.Identities;

public interface ICurrentUser
{
    string UserId { get; }
    string TanentId { get; }
}
