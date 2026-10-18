using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Identity.Application.DTOs;
using Karya.Core.Identity.Infrastructure;
using Karya.Core.Identity.Infrastructure.Persistence;
using Karya.Core.Identity.Infrastructure.Persistence.Repositories;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Results;
using Karya.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Identity.Application.Services;

/// <summary>Kullanıcıyı rol grubuna atama servisi.</summary>
public class AppUserRoleGroupService : BaseService<AppUserRoleGroupRepository, AppUserRoleGroup, Guid>
{
    public AppUserRoleGroupService(IdentityUnitOfWork uow) : base(uow) { }

    public override async Task<BaseResult<LoadResult>> Select<TDto>(DataSourceLoadOptionsBase filterDataOptions)
    {
        var query = Query()
            .Select(x => new AppUserRoleGroupLDto
            {
                Id = x.Id,
                RoleGroupId = x.RoleGroupId,
                RoleGroupName = x.RoleGroup != null ? x.RoleGroup.Name : string.Empty,
                UserId = x.UserId,
                UserName = x.User != null ? x.User.UserName ?? string.Empty : string.Empty,
                TenantId = x.TenantId,
            });

        var result = await DataSourceLoader.LoadAsync(query, filterDataOptions);

        return BaseResult<LoadResult>.Success("200", null, result);
    }
}
