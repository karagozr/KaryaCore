namespace Karya.Core.Interfaces.Results;

public interface IBaseResult
{
    bool IsSuccess { get; }
    string Code { get; }
    string Message { get; }
    Dictionary<string, string>? Errors { get; }
}

public interface IBaseResult<T> : IBaseResult
{
    T? Data { get; }
}


