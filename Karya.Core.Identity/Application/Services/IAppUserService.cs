using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Interfaces.Services;
using Karya.Core.Results;

namespace Karya.Core.Identity.Application.Services
{
    public interface IAppUserService : IBaseService<AppUser, Guid>
    {
        Task<BaseResult<bool>> ChangePasswordAsync(string currentPassword, string newPassword);
    }
}
