using Karya.Core.Results;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Karya.Core.App.Commons;

public class ExceptionBehavior<TRequest, TResponse>(ILogger<ExceptionBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : BaseResult
{


    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        try
        {
            return await next();
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Unauthorized access for {Request}", typeof(TRequest).Name);

            return BaseResult.ErrorCoded<TResponse>(
                "403",
                MessageCodes.Unauthorized);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception for {Request}", typeof(TRequest).Name);

            return BaseResult.ErrorCoded<TResponse>(
                "500",
                MessageCodes.ServerError);
        }
    }
}