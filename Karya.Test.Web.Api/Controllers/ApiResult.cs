using Karya.Core.Results;
using Microsoft.AspNetCore.Mvc;

namespace Karya.Test.Web.Api.Controllers;



public class ApiResult : ActionResult, IActionResult
{
    protected readonly bool IsSuccess;
    protected readonly string Code;
    protected readonly string Message;
    protected readonly Dictionary<string, string>? Errors;


    public ApiResult(BaseResult result)
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
    public ApiResult(BaseResult<T> result) : base(result) => Data = result.Data;

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