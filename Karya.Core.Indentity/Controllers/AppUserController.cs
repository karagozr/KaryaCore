using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.DTOs;
using Karya.Core.Indentity.Services;
using Karya.Core.Web.Abstracts.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Karya.Core.Indentity.Controllers;


[Authorize]
public abstract class AppUserController : BaseCrudController<AppUser, Guid, AppUserSDto, AppUserLDto, AppUserADto, AppUserUDto>
{
    private readonly IAppUserService _appUserService;

    public AppUserController(IMediator mediator, IAppUserService appUserService)
        : base(mediator, appUserService)
    {
        _appUserService = appUserService;
    }

    [HttpPost("update-password")]
    public virtual async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var result = await _appUserService.ChangePasswordAsync(dto.CurrentPassword, dto.NewPassword);
        return ApiActionResult(result);
    }
}
