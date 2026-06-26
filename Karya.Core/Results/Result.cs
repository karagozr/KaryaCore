using Karya.Core.Interfaces.Results;

namespace Karya.Core.Results;

public enum ResultType
{
    Success,
    Error,
    Warning
}

public record BaseResult 
{
    public bool IsSuccess { get; init; }

    public string Code { get; init; }

    public string Message { get; init; } = string.Empty;

    /// <summary>
    /// Semantic message key (see <see cref="MessageCodes"/>). When set, the
    /// response edge resolves it to localized text using the request language.
    /// </summary>
    public string? MessageCode { get; init; }

    /// <summary>
    /// Arguments applied to the localized text placeholders ({0}, {1}, ...).
    /// </summary>
    public object[]? MessageArgs { get; init; }

    public Dictionary<string, string>? Errors { get; init; }
    public Dictionary<string, string>? Infos { get; init; }
    public Dictionary<string, string>? Warnings { get; init; }

    protected BaseResult(bool isSuccess, string code, string? message = null)
    {
        IsSuccess   = isSuccess;
        Code        = code;
        Message     = message ?? string.Empty;
    }

    protected BaseResult(ResultType resultType, bool isSuccess, string code, string? message = null, Dictionary<string, string>? values=null):this(isSuccess, code, message)
    {
        if (resultType == ResultType.Success)
            Infos = values;
        else if (resultType == ResultType.Error)
            Errors = values;
        else
            Warnings = values;
    }

    public static BaseResult Success()
    {
        return new(ResultType.Success, true, "200");
    }

    public static BaseResult Success(string code)
    {
        return new(ResultType.Success, true, code);
    }

    public static BaseResult Success(string code, string? message, Dictionary<string, string>? infos = null)
    {
        return new(ResultType.Success,true, code, message);
    } 
        

    public static BaseResult Error(string errorCode, string? errorMessage, Dictionary<string, string>? errors = null)
    {
        return new(ResultType.Error, false, errorCode, errorMessage);
    } 

    public static BaseResult Warning(string code, string? message, Dictionary<string, string>? infos = null)
        => new(ResultType.Warning,true, code, message,infos);

    // --- Coded (localizable) factories ---------------------------------------
    // 'code' is the HTTP status; 'messageCode' is the translation key resolved
    // at the response edge using the request language.

    public static BaseResult SuccessCoded(string code, string messageCode, params object[] args)
        => new(ResultType.Success, true, code) { MessageCode = messageCode, MessageArgs = args };

    public static BaseResult ErrorCoded(string code, string messageCode, params object[] args)
        => new(ResultType.Error, false, code) { MessageCode = messageCode, MessageArgs = args };

    public static BaseResult WarningCoded(string code, string messageCode, params object[] args)
        => new(ResultType.Warning, true, code) { MessageCode = messageCode, MessageArgs = args };

}

public record BaseResult<T> : BaseResult
{
    public T? Data { get; init; }
    public BaseResult(BaseResult result,T? data):base(result)
    {
        Data = data;
    }
    protected BaseResult(bool isSuccess, string code, T? data, string? message = null) : base(isSuccess, code, message)
    {
        Data = data;
    }
    protected BaseResult(ResultType resultType, bool isSuccess, string code, T? data, string? message = null, Dictionary<string, string>? values=null) 
        : base(resultType,isSuccess, code ,message,values)
    {
        Data = data;
    }
 
    public static BaseResult<T> Success(string code, string? message, T? data, Dictionary<string, string>? errors = null)
        => new(ResultType.Success, true, code, data, message, errors);

    public static BaseResult<T> Error(string code, string? message, T? data, Dictionary<string, string>? errors = null)
        => new(ResultType.Error, false, code, data, message, errors);

    public static BaseResult<T> Warning(string code, string? message, T? data, Dictionary<string, string>? warnings = null)
        => new(ResultType.Warning, true, code, data, message, warnings);

    // --- Coded (localizable) factories ---------------------------------------

    public static BaseResult<T> SuccessCoded(string code, string messageCode, T? data, params object[] args)
        => new(ResultType.Success, true, code, data) { MessageCode = messageCode, MessageArgs = args };

    public static BaseResult<T> ErrorCoded(string code, string messageCode, T? data, params object[] args)
        => new(ResultType.Error, false, code, data) { MessageCode = messageCode, MessageArgs = args };

    public static BaseResult<T> WarningCoded(string code, string messageCode, T? data, params object[] args)
        => new(ResultType.Warning, true, code, data) { MessageCode = messageCode, MessageArgs = args };


}
