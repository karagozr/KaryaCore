using Karya.Core.App.Features.Commands;
using Karya.Core.Interfaces.DTOs;
using Karya.Core.Interfaces.Filters;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Interfaces.Services;
using Karya.Core.Web.Abstracts.Controllers;
using Karya.Core.Web.Helpers;
using Karya.Test.Web.Api.Data.Service;
using Karya.Test.Web.Api.DTOs;
using Karya.TestApi.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Karya.Test.Web.Api.Controllers;

public class InvController : BaseCrudController<Inventory, string, InvSDto, InvLDto, InvADto, InvUDto>
{
    public InvController(IMediator mediator,ICurrentUser currentUser) : base( mediator, new InvService(currentUser)) { }

    
}

public class InvParentFilter : IParentFilter
{
    public string? FirmId { get; set; }
    public string? InventoryId { get; set; }
}

public class InvDetailController : BaseCrudDetailController<InventoryDetail, int, InvParentFilter, InvDetailSDto, InvDetailLDto, InvDetailADto, InvDetailUDto>
{
    public InvDetailController(IMediator mediator, ICurrentUser currentUser) : base(mediator, new InvDetailService(currentUser)) { }


}