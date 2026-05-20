using MediatR;
using Microsoft.Extensions.Logging;
using Karya.Core.Results;


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
            return (TResponse)BaseResult.Error("403", ex.Message);
        }
        catch (Exception ex)
        {
            var typeOfTresponse = typeof(TResponse);
            
            return (TResponse)BaseResult.Error("500",ex.Message); 

        }
    }
}