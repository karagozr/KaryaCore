using Karya.Core.Identity.Application.Features.Commands;
using Karya.Core.Identity.Application.Services;
using Karya.Core.Web.Abstracts.Controllers;
using Karya.Core.Web.Helpers;
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

    [HttpGet("basic")]
    public virtual async Task<IActionResult> Basic()
    {
        var result = await _mediator.Send(new GetBasicProfileCommand(ProfileService));
        return ApiActionResult(result);
    }

    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        var result = await _mediator.Send(new GetProfileCommand(ProfileService));
        return ApiActionResult(result);
    }

    [HttpPut]
    public virtual async Task<IActionResult> Update([FromBody] Dictionary<string, object> updateData)
    {
        var result = await _mediator.Send(new UpdateProfileCommand(updateData, ProfileService));
        return ApiActionResult(result);
    }

    [HttpGet("claims/{key}")]
    public virtual async Task<ActionResult> ClaimByKey(string key)
    {
        var result = await _mediator.Send(new GetProfileClaimCommand(key, ProfileService));
        return ApiActionResult(result);
    }

    [HttpGet("claims")]
    public virtual async Task<ActionResult> ClaimSelect(DataSourceLoadOptions options)
    {
        var result = await _mediator.Send(new GetProfileClaimsCommand(options, ProfileService));
        return ApiActionResult(result);
    }

    [HttpPost("claims")]
    public virtual async Task<ActionResult> ClaimInsert([FromBody] AppProfileClaimDto dto)
    {
        var result = await _mediator.Send(new AddProfileClaimCommand(dto, ProfileService));
        return ApiActionResult(result);
    }

    [HttpPut("claims/{key}")]
    public virtual async Task<ActionResult> ClaimUpdate(string key, [FromBody] Dictionary<string, object> updateData)
    {
        var result = await _mediator.Send(new UpdateProfileClaimCommand(key, updateData, ProfileService));
        return ApiActionResult(result);
    }

    [HttpDelete("claims/{key}")]
    public virtual async Task<ActionResult> ClaimDelete(string key)
    {
        var result = await _mediator.Send(new DeleteProfileClaimCommand(key, ProfileService));
        return ApiActionResult(result);
    }
}
