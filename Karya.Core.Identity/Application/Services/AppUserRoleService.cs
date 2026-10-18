using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Identity.Infrastructure;
using Karya.Core.Identity.Infrastructure.Persistence;
using Karya.Core.Identity.Infrastructure.Persistence.Repositories;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Identity.Application.Services;

/// <summary>Kullanıcıya direkt rol/yetki atama servisi.</summary>
public class AppUserRoleService : BaseService<AppUserRoleRepository, AppUserRole, Guid>
{
    public AppUserRoleService(IdentityUnitOfWork uow) : base(uow) { }
}
