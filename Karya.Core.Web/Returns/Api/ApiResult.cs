using Karya.Core.Interfaces.Identities;
using Karya.Core.Interfaces.Localization;
using Karya.Core.Results;
using Microsoft.AspNetCore.Mvc;

namespace Karya.Core.Web.Returns.Api;

public class ApiResult : ActionResult, IActionResult
{
    protected readonly bool IsSuccess;
    protected readonly string Code;
    protected readonly string Message;
    protected readonly string? MessageCode;
    protected readonly object[]? MessageArgs;
    protected readonly Dictionary<string, string>? Errors;


    public ApiResult(BaseResult result)
    {
        Code = result.Code;
        Message = result.Message;
        MessageCode = result.MessageCode;
        MessageArgs = result.MessageArgs;
        Errors = result.Errors;
        IsSuccess = result.IsSuccess;

    }

    /// <summary>
    /// Produces the final message: when a MessageCode is present it is localized
    /// using the request language (resolved from the DI container); otherwise the
    /// raw Message is returned as-is (backward compatible).
    /// </summary>
    protected string ResolveMessage(ActionContext context)
    {
        if (string.IsNullOrEmpty(MessageCode))
            return Message;

        var services = context.HttpContext.RequestServices;
        var localizer = services.GetService(typeof(IMessageLocalizer)) as IMessageLocalizer;
        if (localizer == null)
            return Message;

        var currentUser = services.GetService(typeof(ICurrentUser)) as ICurrentUser;
        var language = currentUser?.LanguageId ?? "tr";

        return localizer.Get(MessageCode, language, MessageArgs ?? Array.Empty<object>());
    }

    override public async Task ExecuteResultAsync(ActionContext context)
    {
        var message = ResolveMessage(context);
        var objectResult = new ObjectResult(message)
        {
            StatusCode = Convert.ToInt16(Code),
            Value = new
            {
                IsSuccess,
                Message = message,
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
        var message = ResolveMessage(context);
        var objectResult = new ObjectResult(message)
        {
            StatusCode = Convert.ToInt16(Code),
            Value = new
            {
                IsSuccess,
                Message = message,
                Errors,
                Data

            },

        };

        await objectResult.ExecuteResultAsync(context);
    }

}