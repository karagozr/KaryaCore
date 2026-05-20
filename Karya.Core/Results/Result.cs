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

    
}
