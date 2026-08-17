using Karya.Core.App.Interfaces.Profiles;
using Karya.Core.App.Interfaces.Services;
using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Interfaces.Identities;
using Microsoft.AspNetCore.Identity;

namespace Karya.Core.Indentity.Services;

public class UserProfileService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ICurrentUser _currentUser;
    private readonly IEnumerable<IUserProfileService> _profileServices;

    public UserProfileService(UserManager<AppUser> userManager, ICurrentUser currentUser, IEnumerable<IUserProfileService> profileServices)
    {
        _userManager = userManager;
        _currentUser = currentUser;
        _profileServices = profileServices;
    }

    public async Task<IUserProfileSection?> GetProfileSectionAsync()
    {
        if (string.IsNullOrWhiteSpace(_currentUser.UserId) || string.IsNullOrWhiteSpace(_currentUser.TenantId))
            return null;

        var user = await _userManager.FindByIdAsync(_currentUser.UserId);

        if (user is null)
            return null;

        var services = _profileServices.FirstOrDefault();

        if (services is null)
            return null;

        return await services.GetProfileSectionAsync(user.Id, _currentUser.TenantId, user.ErpUsername, user.ErpPersonId);
    }
}