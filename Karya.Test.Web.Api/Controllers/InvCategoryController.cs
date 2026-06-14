using Azure.Core;
using DevExtreme.AspNet.Data;
using Karya.Core.App.Features.Commands;
using Karya.Core.Interfaces.Entities;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Web.Abstracts.Controllers;
using Karya.Test.Web.Api.Data.Service;
using Karya.Test.Web.Api.DTOs;
using Karya.Test.Web.Api.Helpers;
using Karya.TestApi.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace Karya.Test.Web.Api.Controllers;

public class InvCategoryController : BaseCrudController<InventoryCategory,string,InvCategorySDto, InvCategoryLDto, InvCategoryADto, InvCategoryUDto>
{
    public InvCategoryController(IMediator mediator,ICurrentUser currentUser) : base(mediator, new InvCategoryService(currentUser)) { }

    [HttpGet("select")]
    public async Task<ActionResult> Query([FromQuery]DataSourceLoadOptionsBase loadOptions)
    {

        var cleanOptions = loadOptions.ToCleanOptions<InventoryCategory>();

        var result = await _mediator.Send(
            new SelectCommand<InventoryCategory, string, InvCategoryLDto>(cleanOptions, _service, $"{typeof(InventoryCategory).Name}.Read"));
        return ApiActionResult(result);
    }

}
