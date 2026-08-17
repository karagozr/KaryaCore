using Karya.Core.App.Interfaces.Profiles;

namespace Karya.Core.App.Interfaces.Services;

public interface IUserProfileService
{
    Task<IUserProfileSection?> GetProfileSectionAsync(Guid userId, string tenantId, string? erpUsername, string? erpPersonId);
}