
using Karya.Core.Interfaces.Results;

namespace Karya.Core.Results;
public record Result : IBaseResult
{
    public bool IsSuccess { get; init; }

    public string Code { get; init; }

    public string Message { get; init; } = string.Empty;

    public Dictionary<string, string>? Errors { get; init; } = new();

    protected Result(bool isSuccess, string code, string? message = null)
    {
        IsSuccess = isSuccess;
        Code = code;
        Message = message ?? string.Empty;
    }

    public static Result Success(string code = "100") => new(true, code, null);

    public static Result Error(string errorCode, string errorMessage) => new(false, errorCode, errorMessage);

    public static Result Error(Dictionary<string, string> errors) => new(false, "400", "Has a multiple errors")
    {
        Errors = errors
    };
}

public record Result<T> : Result, IBaseResult<T> 
{
    public T? Data { get; init; }

    protected Result(bool isSuccess, string code, T data, string? message = null) : base(isSuccess, code, message)
    {
        Data = data;
    }

    public static Result<T> Success(T data, string code = "200") => new(true, code, data, null);
    public static Result<T> Error(T data, string errorCode, string errorMessage) => new(false, errorCode, data, errorMessage);
    public static Result<T> Error(T data, Dictionary<string, string> errors)
    {
        return new Result<T>(false, "400", data, "Has a multiple errors")
        {
            Errors = errors
        };
    }
}
