using System.Security.Claims;

namespace Karya.Core.Indentity.Services
{
    public interface IAppAuthService
    {
        Task<ClaimsPrincipal?> LoginAsync(string userName, string password, string tenantId);

        Task<bool> ForgotPasswordAsync(string email);

        Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
    }
}
