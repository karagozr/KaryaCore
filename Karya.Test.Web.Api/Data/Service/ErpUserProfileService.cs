using Karya.Core.App.Interfaces.Profiles;
using Karya.Core.App.Interfaces.Services;

namespace Karya.Test.Web.Api.Data.Service
{
    public class ErpUserProfileService : IUserProfileService
    {
        private readonly HttpClient _httpClient;

        public ErpUserProfileService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<IUserProfileSection?> GetProfileSectionAsync(Guid userId, string tenantId, string? erpUsername, string? erpPersonId)
        {
            throw new NotImplementedException();
        }
    }
}
