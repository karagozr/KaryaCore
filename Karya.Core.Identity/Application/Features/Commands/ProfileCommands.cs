using Karya.Core.App.Interfaces.Commands;
using Karya.Core.Identity.Application.Services;
using Karya.Core.Results;

namespace Karya.Core.Identity.Application.Features.Commands;

/// <summary>
/// Oturumdaki kullanıcının profil işlemleri için MediatR command'ları.
/// Kullanıcı kendi profiline eriştiği için varsayılan olarak yetki (Permission) aranmaz.
/// </summary>
public record GetProfileCommand(
    IAppProfileService Service,
    string Permission = ""
) : IExecutableCrudRequest<BaseResult<AppProfileDto>>
{
    public Task<BaseResult<AppProfileDto>> ExecuteAsync(CancellationToken ct = default)
        => Service.GetAsync();
}

public record UpdateProfileCommand(
    AppProfileUDto Dto,
    IAppProfileService Service,
    string Permission = ""
) : IExecutableCrudRequest<BaseResult<AppProfileDto>>
{
    public Task<BaseResult<AppProfileDto>> ExecuteAsync(CancellationToken ct = default)
        => Service.UpdateAsync(Dto);
}
