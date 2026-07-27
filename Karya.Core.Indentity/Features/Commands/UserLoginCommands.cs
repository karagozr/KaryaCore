using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Karya.Core.App.Interfaces.Commands;
using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.DTOs;
using Karya.Core.Results;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Features.Commands;

/// <summary>
/// Kullanıcı harici giriş (external login) işlemleri için MediatR command'ları.
/// Composite key (LoginProvider + ProviderKey) korunur; listeleme ve silme desteklenir.
/// Yetki kontrolü <c>Permission</c> üzerinden pipeline'da uygulanır.
/// </summary>
public record SelectUserLoginsCommand(
    DataSourceLoadOptionsBase LoadOptions,
    DbContext Context,
    string Permission = ""
) : IExecutableCrudRequest<BaseResult<LoadResult>>
{
    public async Task<BaseResult<LoadResult>> ExecuteAsync(CancellationToken ct = default)
    {
        var query = Context.Set<AppUserLogin>().AsNoTracking()
            .Select(l => new AppUserLoginLDto
            {
                LoginProvider = l.LoginProvider,
                ProviderKey = l.ProviderKey,
                ProviderDisplayName = l.ProviderDisplayName,
                UserId = l.UserId
            });

        var res = await DataSourceLoader.LoadAsync(query, LoadOptions);
        return BaseResult<LoadResult>.Success("200", null, res);
    }
}

public record DeleteUserLoginCommand(
    string LoginProvider,
    string ProviderKey,
    DbContext Context,
    string Permission = ""
) : IExecutableCrudRequest<BaseResult>
{
    public async Task<BaseResult> ExecuteAsync(CancellationToken ct = default)
    {
        var entity = await Context.Set<AppUserLogin>()
            .FirstOrDefaultAsync(l => l.LoginProvider == LoginProvider && l.ProviderKey == ProviderKey, ct);
        if (entity is null)
            return BaseResult.Error("404", "Kayıt bulunamadı.");

        Context.Remove(entity);
        await Context.SaveChangesAsync(ct);
        return BaseResult.Success("200", null);
    }
}
