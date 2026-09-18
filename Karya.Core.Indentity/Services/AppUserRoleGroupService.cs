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

/// <summary>Kullanıcıyı rol grubuna atama servisi.</summary>
public class AppUserRoleGroupService : BaseService<AppUserRoleGroupRepository, AppUserRoleGroup, Guid>
{
    public AppUserRoleGroupService(DbContext context, ICurrentUser currentUser) : base(new IdentityUnitOfWork(context, currentUser)) { }

    public override async Task<BaseResult<LoadResult>> Select<TDto>(DataSourceLoadOptionsBase filterDataOptions)
    {
        var query = Query()
            .Select(x => new AppUserRoleGroupLDto
            {
                Id = x.Id,
                RoleGroupId = x.RoleGroupId,
                RoleGroupName = x.RoleGroup.Name,
                UserId = x.UserId,
                UserName = x.User.UserName
            });

        var result = await DataSourceLoader.LoadAsync(query, filterDataOptions);

        return BaseResult<LoadResult>.Success("200", null, result);
    }
}
