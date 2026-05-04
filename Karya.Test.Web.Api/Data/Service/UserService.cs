using Karya.Core.Interfaces.Identities;
using Karya.Core.Interfaces.Results;
using Karya.Core.Results;
using Karya.Core.Services;
using Karya.Test.Web.Api.DTOs;
using Karya.Test.Web.Api.Entities;

namespace Karya.Test.Web.Api.Data.Service;

public class UserService : BaseService<UserRepository, User, string>
{
    ITokenService _tokenService;
    public UserService(ITokenService tokenService, ICurrentUser currentUser) : base(new DevUnitOfWork(currentUser))
    {
        _tokenService = tokenService;
    }

    public async Task<IBaseResult<string>> Login(UserLoginDto userLoginDto)
    {
        var user = await _uow.Repo<UserRepository>().GetSingleAsync(x => x.Id == userLoginDto.UserId && x.Password == userLoginDto.Password);

        if (user==null)
            return Result<string>.Error(null,"400","username or password is not correct");
        

        var token = _tokenService.CreateToken(new UserAuthInfo
        {
            UserId = user.Id,
            TanentId = userLoginDto.TanentId,
        });


        return Result<string>.Success(token);
    }
}

public class AuthService : BaseService<UserRepository, User, string>
{
    ITokenService _tokenService;
    public AuthService(ITokenService tokenService, ICurrentUser currentUser) : base(new DevUnitOfWork(currentUser))
    {
        _tokenService = tokenService;
    }

    public async Task<IBaseResult<string>> Login(UserLoginDto userLoginDto)
    {
        var user = await _uow.Repo<UserRepository>().GetSingleAsync(x => x.Id == userLoginDto.UserId && x.Password == userLoginDto.Password);

        if (user == null)
            return Result<string>.Error(null, "400", "username or password is not correct");


        var token = _tokenService.CreateToken(new UserAuthInfo
        {
            UserId = user.Id,
            TanentId = userLoginDto.TanentId,
        });


        return Result<string>.Success(token);
    }
}


