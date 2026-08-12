namespace Karya.Core.Results;

public enum ResultType { Success, Error, Warning }

public record BaseResult
{
    public bool IsSuccess { get; init; }
    public string Code { get; init; } = "200";
    public string Message { get; init; } = string.Empty;
    public string? MessageCode { get; init; }
    public object[]? MessageArgs { get; init; }
    public Dictionary<string, string>? Errors { get; init; }
    public Dictionary<string, string>? Infos { get; init; }
    public Dictionary<string, string>? Warnings { get; init; }

    protected BaseResult(ResultType type, string code, string? message = null, Dictionary<string, string>? values = null)
    {
        IsSuccess = type != ResultType.Error;
        Code = code;
        Message = message ?? string.Empty;
        Errors = type == ResultType.Error ? values : null;
        Infos = type == ResultType.Success ? values : null;
        Warnings = type == ResultType.Warning ? values : null;
    }

    public static BaseResult Success(string code = "200", string? message = null, Dictionary<string, string>? infos = null)
        => new(ResultType.Success, code, message, infos);
    public static BaseResult Error(string code, string? message, Dictionary<string, string>? errors = null)
        => new(ResultType.Error, code, message, errors);
    public static BaseResult Warning(string code, string? message, Dictionary<string, string>? warnings = null)
        => new(ResultType.Warning, code, message, warnings);
    public static BaseResult SuccessCoded(string code, string messageCode, params object[] args)
        => new(ResultType.Success, code) { MessageCode = messageCode, MessageArgs = args };
    public static BaseResult ErrorCoded(string code, string messageCode, params object[] args)
        => new(ResultType.Error, code) { MessageCode = messageCode, MessageArgs = args };
    public static BaseResult WarningCoded(string code, string messageCode, params object[] args)
        => new(ResultType.Warning, code) { MessageCode = messageCode, MessageArgs = args };

    public static TResponse ErrorCoded<TResponse>(string code, string messageCode, params object[] args) where TResponse : BaseResult
    {
        var result = ErrorCoded(code, messageCode, args);

        if (typeof(TResponse) == typeof(BaseResult))
            return (TResponse)result;

        if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(BaseResult<>))
        {
            var dataType = typeof(TResponse).GetGenericArguments()[0];
            var defaultData = dataType.IsValueType ? Activator.CreateInstance(dataType) : null;

            return (TResponse)Activator.CreateInstance(typeof(TResponse), result, defaultData)!;
        }

        throw new InvalidOperationException($"Unsupported response type: {typeof(TResponse).Name}");
    }
}

public record BaseResult<T> : BaseResult
{
    public T? Data { get; init; }

    public BaseResult(BaseResult result, T? data) : base(result)
    {
        Data = data;
    }

    protected BaseResult(ResultType type, string code, T? data, string? message = null, Dictionary<string, string>? values = null) : base(type, code, message, values)
    {
        Data = data;
    }

    public static BaseResult<T> Success(string code, string? message, T? data, Dictionary<string, string>? infos = null)
        => new(ResultType.Success, code, data, message, infos);
    public static BaseResult<T> Error(string code, string? message, T? data, Dictionary<string, string>? errors = null)
        => new(ResultType.Error, code, data, message, errors);
    public static BaseResult<T> Warning(string code, string? message, T? data, Dictionary<string, string>? warnings = null)
        => new(ResultType.Warning, code, data, message, warnings);
    public static BaseResult<T> SuccessCoded(string code, string messageCode, T? data, params object[] args)
        => new(ResultType.Success, code, data) { MessageCode = messageCode, MessageArgs = args };
    public static BaseResult<T> ErrorCoded(string code, string messageCode, T? data, params object[] args)
        => new(ResultType.Error, code, data) { MessageCode = messageCode, MessageArgs = args };
    public static BaseResult<T> WarningCoded(string code, string messageCode, T? data, params object[] args)
        => new(ResultType.Warning, code, data) { MessageCode = messageCode, MessageArgs = args };
}