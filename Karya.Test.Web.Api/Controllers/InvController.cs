using Karya.Core.App.Features.Commands;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Web.Abstracts.Controllers;
using Karya.Test.Web.Api.Data.Service;
using Karya.Test.Web.Api.DTOs;
using Karya.TestApi.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Karya.Test.Web.Api.Controllers;

public class InvController : BaseCrudController<Inventory, string, InvSDto, InvLDto, InvADto, InvUDto>
{
    public InvController(IMediator mediator,ICurrentUser currentUser) : base( mediator, new InvService(currentUser)) { }

    [HttpGet("all")]
    public async Task<ActionResult> GetAll()
    {
        var result = await _mediator.Send(
            new SelectCommand<Inventory, string, InvLDto>(x => x.Id != "01", _service, "Inventory.Read"));
        return ApiActionResult(result);
    }
}
