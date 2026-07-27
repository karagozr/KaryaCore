using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Karya.Core.App.Interfaces.Commands;
using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.DTOs;
using Karya.Core.Results;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Features.Commands;

/// <summary>
/// OpenIddict authorization (yetkilendirme) işlemleri için MediatR command'ları.
/// Authorization kayıtları runtime verisidir; yalnızca listeleme, tekil getirme ve
/// silme desteklenir. Yetki kontrolü <c>Permission</c> üzerinden pipeline'da uygulanır.
/// </summary>
public record SelectAuthorizationsCommand(
    DataSourceLoadOptionsBase LoadOptions,
    DbContext Context,
    string Permission = ""
) : IExecutableCrudRequest<BaseResult<LoadResult>>
{
    public async Task<BaseResult<LoadResult>> ExecuteAsync(CancellationToken ct = default)
    {
        var query = Context.Set<AppAuthorization>().AsNoTracking()
            .Select(a => new AppAuthorizationLDto
            {
                Id = a.Id,
                ApplicationId = a.Application != null ? a.Application.Id : (Guid?)null,
                Subject = a.Subject,
                Status = a.Status,
                Type = a.Type,
                CreationDate = a.CreationDate
            });

        var res = await DataSourceLoader.LoadAsync(query, LoadOptions);
        return BaseResult<LoadResult>.Success("200", null, res);
    }
}

public record ByKeyAuthorizationCommand(
    Guid Id,
    DbContext Context,
    string Permission = ""
) : IExecutableCrudRequest<BaseResult<AppAuthorizationLDto>>
{
    public async Task<BaseResult<AppAuthorizationLDto>> ExecuteAsync(CancellationToken ct = default)
    {
        var dto = await Context.Set<AppAuthorization>().AsNoTracking()
            .Where(a => a.Id == Id)
            .Select(a => new AppAuthorizationLDto
            {
                Id = a.Id,
                ApplicationId = a.Application != null ? a.Application.Id : (Guid?)null,
                Subject = a.Subject,
                Status = a.Status,
                Type = a.Type,
                CreationDate = a.CreationDate
            })
            .FirstOrDefaultAsync(ct);

        if (dto is null)
            return BaseResult<AppAuthorizationLDto>.Error("404", $"'{Id}' bulunamadı.", null);

        return BaseResult<AppAuthorizationLDto>.Success("200", null, dto);
    }
}

public record DeleteAuthorizationCommand(
    Guid Id,
    DbContext Context,
    string Permission = ""
) : IExecutableCrudRequest<BaseResult>
{
    public async Task<BaseResult> ExecuteAsync(CancellationToken ct = default)
    {
        var entity = await Context.Set<AppAuthorization>().FirstOrDefaultAsync(a => a.Id == Id, ct);
        if (entity is null)
            return BaseResult.Error("404", $"'{Id}' bulunamadı.");

        Context.Remove(entity);
        await Context.SaveChangesAsync(ct);
        return BaseResult.Success("200", null);
    }
}
