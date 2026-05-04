using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Karya.Core.App.Commons;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        logger.LogInformation("[{Request}] başladı", typeof(TRequest).Name);
        var sw = Stopwatch.StartNew();
        var response = await next();
        logger.LogInformation("[{Request}] tamamlandı — {Ms}ms", typeof(TRequest).Name, sw.ElapsedMilliseconds);
        return response;
    }
}