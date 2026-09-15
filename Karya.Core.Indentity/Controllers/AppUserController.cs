using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.DTOs;
using Karya.Core.Indentity.Services;
using Karya.Core.Results;
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

    [HttpPost("reset-password")]
    public Task<BaseResult<bool>> ResetPassword(string email, string token, string newPassword)
    {
        return _appUserService.ResetPasswordAsync(email, token, newPassword);
    }
}
