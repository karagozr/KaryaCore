using Karya.Core.App.Interfaces.Commands;
using Karya.Core.Interfaces.Results;
using Karya.Test.Web.Api.Data.Service;
using Karya.Test.Web.Api.DTOs;

namespace Karya.Test.Web.Api.Commands;

public record LoginCommand(UserLoginDto dto, UserService Service, string Permission = "") : IExecutableCrudRequest<IBaseResult<string>> 
{
    public Task<IBaseResult<string>> ExecuteAsync(CancellationToken ct = default) => Service.Login(dto);
}
