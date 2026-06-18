using Azure.Core;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Karya.Core.App.Features.Commands;
using Karya.Core.Interfaces.Entities;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Results;
using Karya.Core.Web.Abstracts.Controllers;
using Karya.Test.Web.Api.Data.Service;
using Karya.Test.Web.Api.DTOs;
using Karya.TestApi.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace Karya.Test.Web.Api.Controllers;

public class InvCategoryController : BaseCrudController<InventoryCategory,string,InvCategorySDto, InvCategoryLDto, InvCategoryADto, InvCategoryUDto>
{
    public InvCategoryController(IMediator mediator,ICurrentUser currentUser) : base(mediator, new InvCategoryService(currentUser)) { }

  

}
