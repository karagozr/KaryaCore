using MediatR;
using Microsoft.Extensions.Logging;

namespace Karya.Core.App.Commons;

public class ExceptionBehavior<TRequest, TResponse>(ILogger<ExceptionBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        try
        {
            return await next();
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Yetki hatası — {Request}", typeof(TRequest).Name);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Beklenmedik hata — {Request}", typeof(TRequest).Name);
            throw;
        }
    }
}