using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Karya.Core.App.Interfaces.Commands;
using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.DTOs;
using Karya.Core.Results;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Features.Commands;

/// <summary>
/// Kullanıcı token (IdentityUserToken) işlemleri için MediatR command'ları.
/// Composite key (UserId + LoginProvider + Name) korunur; listeleme ve silme
/// desteklenir. Token değeri güvenlik gereği listelenmez. Yetki kontrolü
/// <c>Permission</c> üzerinden pipeline'da uygulanır.
/// </summary>
public record SelectUserTokensCommand(
    DataSourceLoadOptionsBase LoadOptions,
    DbContext Context,
    string Permission = ""
) : IExecutableCrudRequest<BaseResult<LoadResult>>
{
    public async Task<BaseResult<LoadResult>> ExecuteAsync(CancellationToken ct = default)
    {
        var query = Context.Set<AppUserToken>().AsNoTracking()
            .Select(t => new AppUserTokenLDto
            {
                UserId = t.UserId,
                LoginProvider = t.LoginProvider,
                Name = t.Name
            });

        var res = await DataSourceLoader.LoadAsync(query, LoadOptions);
        return BaseResult<LoadResult>.Success("200", null, res);
    }
}

public record DeleteUserTokenCommand(
    Guid UserId,
    string LoginProvider,
    string Name,
    DbContext Context,
    string Permission = ""
) : IExecutableCrudRequest<BaseResult>
{
    public async Task<BaseResult> ExecuteAsync(CancellationToken ct = default)
    {
        var entity = await Context.Set<AppUserToken>()
            .FirstOrDefaultAsync(t => t.UserId == UserId && t.LoginProvider == LoginProvider && t.Name == Name, ct);
        if (entity is null)
            return BaseResult.Error("404", "Kayıt bulunamadı.");

        Context.Remove(entity);
        await Context.SaveChangesAsync(ct);
        return BaseResult.Success("200", null);
    }
}
