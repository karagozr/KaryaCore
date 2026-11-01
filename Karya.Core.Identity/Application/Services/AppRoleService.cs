using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Identity.Infrastructure;
using Karya.Core.Identity.Infrastructure.Persistence;
using Karya.Core.Identity.Infrastructure.Persistence.Repositories;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Identity.Application.Services;

/// <summary>Rol/yetki yönetimi servisi.</summary>
public class AppRoleService : BaseService<AppRoleRepository, AppRole, Guid>
{
    public AppRoleService(IdentityUnitOfWork uow) : base(uow) { }
}
