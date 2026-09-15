using Karya.Core.Interfaces.Services;
using Karya.Core.Results;
using System.Security.Claims;

namespace Karya.Core.Indentity.Services
{
    public interface IAppAuthService:IBaseService
    {
        Task<ClaimsPrincipal?> LoginAsync(string userName, string password, string tenantId);

        Task<BaseResult<bool>> ForgotPasswordAsync(string email);
    }
}
