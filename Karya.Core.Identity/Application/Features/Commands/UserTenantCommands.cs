using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Karya.Core.App.Interfaces.Commands;
using Karya.Core.Identity.Application.DTOs.Users;
using Karya.Core.Identity.Application.Services;
using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Results;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Identity.Application.Features.Commands;

public static class AppUserTenantPermissions
{
    public const string Read = "AppUserTenant.Read";
    public const string Create = "AppUserTenant.Create";
    public const string Delete = "AppUserTenant.Delete";
}

public sealed class AppUserTenantLDto
{
    public string Id { get; set; } = null!;
    public Guid UserId { get; set; }
    public string TenantId { get; set; } = null!;
    public string? TenantName { get; set; }
}

public record SelectUserTenantsCommand(
    DataSourceLoadOptionsBase LoadOptions,
    DbContext Context,
    Guid UserId,
    string Permission = AppUserTenantPermissions.Read
) : IExecutableCrudRequest<BaseResult<LoadResult>>
{
    public async Task<BaseResult<LoadResult>> ExecuteAsync(CancellationToken ct = default)
    {
        var items = await (
            from membership in Context.Set<AppUserTenant>().AsNoTracking()
            join tenant in Context.Set<AppTenant>().AsNoTracking()
                on membership.TenantId equals tenant.Id into tenantJoin
            from tenant in tenantJoin.DefaultIfEmpty()
            where membership.UserId == UserId
            select new
            {
                UserId = membership.UserId,
                TenantId = membership.TenantId,
                TenantName = tenant != null ? tenant.Name : null
            }).ToListAsync(ct);

        var rows = items.Select(x => new AppUserTenantLDto
        {
            Id = $"{x.UserId}|{x.TenantId}",
            UserId = x.UserId,
            TenantId = x.TenantId,
            TenantName = x.TenantName
        }).ToList();

        var data = DataSourceLoader.Load(rows, LoadOptions);
        return BaseResult<LoadResult>.Success("200", null, data);
    }
}

public record AssignUserTenantCommand(
    AppUserTenantAssignDto Dto,
    AppUserTenantService Service,
    string Permission = AppUserTenantPermissions.Create
) : IExecutableCrudRequest<BaseResult>
{
    public Task<BaseResult> ExecuteAsync(CancellationToken ct = default)
    {
        if (Dto.UserId == Guid.Empty || string.IsNullOrWhiteSpace(Dto.TenantId))
            return Task.FromResult(BaseResult.Error("400", "UserId and TenantId are required."));

        return Service.AssignResultAsync(Dto.UserId, Dto.TenantId, ct);
    }
}

public record RemoveUserTenantCommand(
    Guid UserId,
    string TenantId,
    AppUserTenantService Service,
    string Permission = AppUserTenantPermissions.Delete
) : IExecutableCrudRequest<BaseResult>
{
    public Task<BaseResult> ExecuteAsync(CancellationToken ct = default)
    {
        if (UserId == Guid.Empty || string.IsNullOrWhiteSpace(TenantId))
            return Task.FromResult(BaseResult.Error("400", "UserId and TenantId are required."));

        return Service.RemoveAsync(UserId, TenantId, ct);
    }
}
