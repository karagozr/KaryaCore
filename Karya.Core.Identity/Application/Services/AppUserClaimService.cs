using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Identity.Infrastructure;
using Karya.Core.Identity.Infrastructure.Persistence;
using Karya.Core.Identity.Infrastructure.Persistence.Repositories;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Identity.Application.Services;

/// <summary>Kullanıcı claim yönetimi servisi.</summary>
public class AppUserClaimService : BaseService<AppUserClaimRepository, AppUserClaim, int>
{
    public AppUserClaimService(IdentityUnitOfWork uow) : base(uow) { }
}
