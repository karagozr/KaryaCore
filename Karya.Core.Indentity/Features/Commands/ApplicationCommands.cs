using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Karya.Core.App.Interfaces.Commands;
using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.DTOs;
using Karya.Core.Results;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;

namespace Karya.Core.Indentity.Features.Commands;

/// <summary>
/// OpenIddict uygulama (client) işlemleri için MediatR command'ları.
/// Yetki kontrolü <c>Permission</c> üzerinden AuthorizationBehavior pipeline'ında
/// uygulanır; OpenIddict manager her command'a parametre olarak geçirilir.
/// </summary>
public record SelectApplicationsCommand(
    DataSourceLoadOptionsBase LoadOptions,
    DbContext Context,
    string Permission = ""
) : IExecutableCrudRequest<BaseResult<LoadResult>>
{
    public async Task<BaseResult<LoadResult>> ExecuteAsync(CancellationToken ct = default)
    {
        var query = Context.Set<AppApplication>().AsNoTracking()
            .Select(a => new AppApplicationLDto
            {
                Id = a.Id.ToString(),
                ClientId = a.ClientId,
                DisplayName = a.DisplayName,
                ClientType = a.ClientType
            });

        var res = await DataSourceLoader.LoadAsync(query, LoadOptions);
        return BaseResult<LoadResult>.Success("200", null, res);
    }
}

public record ByClientIdApplicationCommand(
    string ClientId,
    IOpenIddictApplicationManager Manager,
    string Permission = ""
) : IExecutableCrudRequest<BaseResult<AppApplicationLDto>>
{
    public async Task<BaseResult<AppApplicationLDto>> ExecuteAsync(CancellationToken ct = default)
    {
        var app = await Manager.FindByClientIdAsync(ClientId, ct);
        if (app is null)
            return BaseResult<AppApplicationLDto>.Error("404", $"'{ClientId}' bulunamadı.", null);

        var dto = new AppApplicationLDto
        {
            Id = await Manager.GetIdAsync(app, ct),
            ClientId = await Manager.GetClientIdAsync(app, ct),
            DisplayName = await Manager.GetDisplayNameAsync(app, ct),
            ClientType = await Manager.GetClientTypeAsync(app, ct)
        };
        return BaseResult<AppApplicationLDto>.Success("200", null, dto);
    }
}

public record InsertApplicationCommand(
    AppApplicationADto Dto,
    IOpenIddictApplicationManager Manager,
    string Permission = ""
) : IExecutableCrudRequest<BaseResult>
{
    public async Task<BaseResult> ExecuteAsync(CancellationToken ct = default)
    {
        var existing = await Manager.FindByClientIdAsync(Dto.ClientId, ct);
        if (existing is not null)
            return BaseResult.Error("409", $"'{Dto.ClientId}' zaten mevcut.");

        var descriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = Dto.ClientId,
            ClientSecret = Dto.ClientSecret,
            DisplayName = Dto.DisplayName,
            ClientType = Dto.ClientType
        };

        foreach (var uri in Dto.RedirectUris)
            descriptor.RedirectUris.Add(new Uri(uri));

        foreach (var permission in Dto.Permissions)
            descriptor.Permissions.Add(permission);

        await Manager.CreateAsync(descriptor, ct);
        return BaseResult.Success("201", null);
    }
}

public record DeleteApplicationCommand(
    string ClientId,
    IOpenIddictApplicationManager Manager,
    string Permission = ""
) : IExecutableCrudRequest<BaseResult>
{
    public async Task<BaseResult> ExecuteAsync(CancellationToken ct = default)
    {
        var app = await Manager.FindByClientIdAsync(ClientId, ct);
        if (app is null)
            return BaseResult.Error("404", $"'{ClientId}' bulunamadı.");

        await Manager.DeleteAsync(app, ct);
        return BaseResult.Success("200", null);
    }
}
