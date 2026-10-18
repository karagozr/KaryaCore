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

public class AppRoleGroupRoleService : BaseDetailService<AppRoleGroupRoleRepository, AppRoleGroupRole, Guid, AppRoleGroupRoleParentFilter>
{
    public AppRoleGroupRoleService(IdentityUnitOfWork uow) : base(uow) { }

    public override async Task<BaseResult<LoadResult>> Select<TDto>(AppRoleGroupRoleParentFilter parentFilter, DataSourceLoadOptionsBase filterDataOptions)
    {
        if (typeof(TDto) == typeof(AppRoleGroupRoleLDto))
        {
            var query = Query(parentFilter)
                .Select(x => new AppRoleGroupRoleLDto
                {
                    Id = x.Id,
                    RoleGroupId = x.RoleGroupId,
                    RoleId = x.RoleId,
                    RoleName = x.Role != null ? x.Role.Name : null,
                    TenantId = x.TenantId
                });

            var res = await DataSourceLoader.LoadAsync(query, filterDataOptions);

            return BaseResult<LoadResult>.Success("200", null, res);
        }

        return await base.Select<TDto>(parentFilter, filterDataOptions);
    }
}