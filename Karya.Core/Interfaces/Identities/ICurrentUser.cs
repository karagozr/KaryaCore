namespace Karya.Core.Interfaces.Identities;

public interface ICurrentUser
{
    string UserId { get; }
    string TenantId { get; }
    string LanguageId { get; }
}
