using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.DTOs;
using Karya.Core.Indentity.Infrastructure;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Results;
using Karya.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Services;

public class AppRoleGroupRoleService : BaseDetailService<AppRoleGroupRoleRepository, AppRoleGroupRole, Guid, AppRoleGroupRoleParentFilter>
{
    public AppRoleGroupRoleService(DbContext context, ICurrentUser currentUser) : base(new IdentityUnitOfWork(context, currentUser)) { }

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
                });

            var res = await DataSourceLoader.LoadAsync(query, filterDataOptions);

            return BaseResult<LoadResult>.Success("200", null, res);
        }

        return await base.Select<TDto>(parentFilter, filterDataOptions);
    }
}