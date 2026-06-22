using Karya.Core.Interfaces.Identities;
using Karya.Core.Web.Abstracts.Controllers;
using Karya.Test.Web.Api.Data.Service;
using Karya.Test.Web.Api.DTOs;
using Karya.TestApi.Entities;
using MediatR;

namespace Karya.Test.Web.Api.Controllers;

public class InvCategoryController : BaseCrudController<InventoryCategory,string,InvCategorySDto, InvCategoryLDto, InvCategoryADto, InvCategoryUDto>
{
    public InvCategoryController(IMediator mediator,ICurrentUser currentUser) : base(mediator, new InvCategoryService(currentUser)) { }

}

public class InvMainCategoryController : BaseCrudController<InventoryMainCategory, string, InvMainCategorySDto, InvMainCategoryLDto, InvMainCategoryADto, InvMainCategoryUDto>
{
    public InvMainCategoryController(IMediator mediator, ICurrentUser currentUser) : base(mediator, new InvMainCategoryService(currentUser)) { }
}

