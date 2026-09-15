using Karya.Core.Results;
using System.Security.Claims;

namespace Karya.Core.Indentity.Services
{
    public interface IAppAuthService
    {
        Task<ClaimsPrincipal?> LoginAsync(string userName, string password, string tenantId);

        Task<BaseResult<bool>> ForgotPasswordAsync(string email);
    }
}
