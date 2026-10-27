using Karya.Core.Identity.Application.Features.Commands;
using Karya.Core.Identity.Application.Services;
using Karya.Core.Web.Abstracts.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Karya.Core.Identity.Controllers;

[Authorize]
public abstract class AppProfileController : BaseController
{
    protected readonly IAppProfileService ProfileService;

    public AppProfileController(IMediator mediator, IAppProfileService profileService)
        : base(mediator)
    {
        ProfileService = profileService;
    }

    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        var result = await _mediator.Send(new GetProfileCommand(ProfileService));
        return ApiActionResult(result);
    }

    [HttpPut]
    public virtual async Task<IActionResult> Update([FromBody] AppProfileUDto dto)
    {
        var result = await _mediator.Send(new UpdateProfileCommand(dto, ProfileService));
        return ApiActionResult(result);
    }
}
