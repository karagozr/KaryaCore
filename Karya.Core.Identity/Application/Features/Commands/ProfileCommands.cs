using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Karya.Core.App.Interfaces.Commands;
using Karya.Core.Identity.Application.Services;
using Karya.Core.Results;

namespace Karya.Core.Identity.Application.Features.Commands;

/// <summary>
/// Oturumdaki kullanıcının profil işlemleri için MediatR command'ları.
/// Kullanıcı kendi profiline eriştiği için varsayılan olarak yetki (Permission) aranmaz.
/// </summary>
public record GetBasicProfileCommand(
    IAppProfileService Service,
    string Permission = ""
) : IExecutableCrudRequest<BaseResult<AppProfileDto>>
{
    public Task<BaseResult<AppProfileDto>> ExecuteAsync(CancellationToken ct = default)
        => Service.GetBasicAsync();
}

public record GetProfileCommand(
    IAppProfileService Service,
    string Permission = ""
) : IExecutableCrudRequest<BaseResult<AppProfileUserDto>>
{
    public Task<BaseResult<AppProfileUserDto>> ExecuteAsync(CancellationToken ct = default)
        => Service.GetAsync();
}

public record UpdateProfileCommand(
    Dictionary<string, object> UpdateData,
    IAppProfileService Service,
    string Permission = ""
) : IExecutableCrudRequest<BaseResult<AppProfileDto>>
{
    public Task<BaseResult<AppProfileDto>> ExecuteAsync(CancellationToken ct = default)
        => Service.UpdateAsync(UpdateData);
}

public record GetProfileClaimsCommand(
    DataSourceLoadOptionsBase LoadOptions,
    IAppProfileService Service,
    string Permission = ""
) : IExecutableCrudRequest<BaseResult<LoadResult>>
{
    public Task<BaseResult<LoadResult>> ExecuteAsync(CancellationToken ct = default)
        => Service.GetClaimsAsync(LoadOptions);
}

public record AddProfileClaimCommand(
    AppProfileClaimDto Dto,
    IAppProfileService Service,
    string Permission = ""
) : IExecutableCrudRequest<BaseResult<AppProfileClaimDto>>
{
    public Task<BaseResult<AppProfileClaimDto>> ExecuteAsync(CancellationToken ct = default)
        => Service.AddClaimAsync(Dto);
}

public record GetProfileClaimCommand(
    string Key,
    IAppProfileService Service,
    string Permission = ""
) : IExecutableCrudRequest<BaseResult<AppProfileClaimDto>>
{
    public Task<BaseResult<AppProfileClaimDto>> ExecuteAsync(CancellationToken ct = default)
        => Service.GetClaimAsync(Key);
}

public record UpdateProfileClaimCommand(
    string Key,
    Dictionary<string, object> UpdateData,
    IAppProfileService Service,
    string Permission = ""
) : IExecutableCrudRequest<BaseResult<AppProfileClaimDto>>
{
    public Task<BaseResult<AppProfileClaimDto>> ExecuteAsync(CancellationToken ct = default)
        => Service.UpdateClaimAsync(Key, UpdateData);
}

public record DeleteProfileClaimCommand(
    string Key,
    IAppProfileService Service,
    string Permission = ""
) : IExecutableCrudRequest<BaseResult>
{
    public Task<BaseResult> ExecuteAsync(CancellationToken ct = default)
        => Service.DeleteClaimAsync(Key);
}
