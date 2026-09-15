using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Interfaces.Services;
using Karya.Core.Results;

namespace Karya.Core.Indentity.Services
{
    public interface IAppUserService : IBaseService<AppUser, Guid>
    {
        Task<BaseResult<bool>> ResetPasswordAsync(string email, string token, string newPassword);
    }
}
