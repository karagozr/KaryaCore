using Karya.Core.Interfaces.Results;
using Microsoft.AspNetCore.Mvc;

namespace Karya.Core.Web.Returns.Api;

public class ApiResult : ActionResult, IActionResult
{
    protected readonly bool IsSuccess;
    protected readonly string Code;
    protected readonly string Message;
    protected readonly Dictionary<string, string>? Errors;


    public ApiResult(IBaseResult result)
    {
        Code = result.Code;
        Message = result.Message;
        Errors = result.Errors;
        IsSuccess = result.IsSuccess;

    }

    override public async Task ExecuteResultAsync(ActionContext context)
    {
        var objectResult = new ObjectResult(Message)
        {
            StatusCode = Convert.ToInt16(Code),
            Value = new
            {
                IsSuccess,
                Message,
                Errors
            },
        };
        await objectResult.ExecuteResultAsync(context);
    }
}

public class ApiResult<T> : ApiResult
{

    private readonly T? Data;
    public ApiResult(IBaseResult<T> result) : base(result) => Data = result.Data;

    public override async Task ExecuteResultAsync(ActionContext context)
    {
        var objectResult = new ObjectResult(Message)
        {
            StatusCode = Convert.ToInt16(Code),
            Value = new
            {
                IsSuccess,
                Message,
                Errors,
                Data

            },

        };

        await objectResult.ExecuteResultAsync(context);
    }

}