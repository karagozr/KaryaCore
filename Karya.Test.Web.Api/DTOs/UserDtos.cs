using Karya.Core.Interfaces.DTOs;

namespace Karya.Test.Web.Api.DTOs;

public class UserLoginDto:IBaseDto
{
    public string TanentId { get; set; }
    public string UserId { get; set; }
    public string Password { get; set; }
}

public class UserSDto : ISingleDto
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public class UserLDto : ISelectDto
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public class UserADto : IInsertDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
}

public class UserUDto : IUpdateDto
{
    public string Name { get; set; }
    public string Password { get; set; }
}

