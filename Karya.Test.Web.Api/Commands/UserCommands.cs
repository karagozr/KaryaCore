using Karya.Core.App.Interfaces.Commands;
using Karya.Core.Results;
using Karya.Test.Web.Api.Data.Service;
using Karya.Test.Web.Api.DTOs;

namespace Karya.Test.Web.Api.Commands;

public record LoginCommand(UserLoginDto dto, UserService Service, string Permission = "") : IExecutableCrudRequest<BaseResult<string>> 
{
    public Task<BaseResult<string>> ExecuteAsync(CancellationToken ct = default) => Service.Login(dto);
}
