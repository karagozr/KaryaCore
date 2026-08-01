using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Karya.Core.App.Interfaces.Commands;
using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.DTOs;
using Karya.Core.Results;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Features.Commands;

/// <summary>
/// OpenIddict token işlemleri için MediatR command'ları. Token kayıtları runtime
/// verisidir; yalnızca listeleme, tekil getirme ve silme desteklenir. Yetki kontrolü
/// <c>Permission</c> üzerinden pipeline'da uygulanır.
/// </summary>
public record SelectTokensCommand(
    DataSourceLoadOptionsBase LoadOptions,
    DbContext Context,
    string Permission = ""
) : IExecutableCrudRequest<BaseResult<LoadResult>>
{
    public async Task<BaseResult<LoadResult>> ExecuteAsync(CancellationToken ct = default)
    {
        var query = Context.Set<AppToken>().AsNoTracking()
            .Select(t => new AppTokenLDto
            {
                Id = t.Id,
                ApplicationId = t.Application != null ? t.Application.Id : (Guid?)null,
                AuthorizationId = t.Authorization != null ? t.Authorization.Id : (Guid?)null,
                Subject = t.Subject,
                Status = t.Status,
                Type = t.Type,
                ReferenceId = t.ReferenceId,
                CreationDate = t.CreationDate,
                ExpirationDate = t.ExpirationDate,
                RedemptionDate = t.RedemptionDate
            });

        var res = await DataSourceLoader.LoadAsync(query, LoadOptions);
        return BaseResult<LoadResult>.Success("200", null, res);
    }
}

public record ByKeyTokenCommand(
    Guid Id,
    DbContext Context,
    string Permission = ""
) : IExecutableCrudRequest<BaseResult<AppTokenLDto>>
{
    public async Task<BaseResult<AppTokenLDto>> ExecuteAsync(CancellationToken ct = default)
    {
        var dto = await Context.Set<AppToken>().AsNoTracking()
            .Where(t => t.Id == Id)
            .Select(t => new AppTokenLDto
            {
                Id = t.Id,
                ApplicationId = t.Application != null ? t.Application.Id : (Guid?)null,
                AuthorizationId = t.Authorization != null ? t.Authorization.Id : (Guid?)null,
                Subject = t.Subject,
                Status = t.Status,
                Type = t.Type,
                ReferenceId = t.ReferenceId,
                CreationDate = t.CreationDate,
                ExpirationDate = t.ExpirationDate,
                RedemptionDate = t.RedemptionDate
            })
            .FirstOrDefaultAsync(ct);

        if (dto is null)
            return BaseResult<AppTokenLDto>.Error("404", $"'{Id}' bulunamadı.", null);

        return BaseResult<AppTokenLDto>.Success("200", null, dto);
    }
}

public record DeleteTokenCommand(
    Guid Id,
    DbContext Context,
    string Permission = ""
) : IExecutableCrudRequest<BaseResult>
{
    public async Task<BaseResult> ExecuteAsync(CancellationToken ct = default)
    {
        var entity = await Context.Set<AppToken>().FirstOrDefaultAsync(t => t.Id == Id, ct);
        if (entity is null)
            return BaseResult.Error("404", $"'{Id}' bulunamadı.");

        Context.Remove(entity);
        await Context.SaveChangesAsync(ct);
        return BaseResult.Success("200", null);
    }
}
