using Karya.Core.Web.Abstracts.Controllers;
using Karya.Test.Web.Api.Commands;
using Karya.Test.Web.Api.Data.Service;
using Karya.Test.Web.Api.DTOs;
using Karya.Test.Web.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Karya.Core.Interfaces.Identities;

namespace Karya.Test.Web.Api.Controllers;

public class AuthController : BaseController<User, string>
{
    public AuthController(IMediator mediator, ITokenService tokenService,ICurrentUser currentUser) : base( mediator, new UserService(tokenService, currentUser)) { 
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(UserLoginDto userLogin)
    {
        if (userLogin.TanentId is null ) userLogin.TanentId = "COMP01";
        var result = await _mediator.Send( new LoginCommand(userLogin, (UserService)_service, "anonim"));
        return ApiActionResult(result);
    }
}

[Authorize]
public class UserController : BaseCrudController<User, string,UserSDto, UserLDto,UserADto,UserUDto>
{
    public UserController(IMediator mediator, ITokenService tokenService,ICurrentUser currentUser) 
        : base(mediator, new UserService(tokenService, currentUser))
    {
    }
    
}