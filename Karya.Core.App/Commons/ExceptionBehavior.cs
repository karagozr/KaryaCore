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
                MessageCodes.Unauthorized,
                ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception for {Request}", typeof(TRequest).Name);

            var details = BuildExceptionDetails(ex);

            return BaseResult.Error<TResponse>(
                "500",
                details.Message,
                details.Errors);
        }
    }

    private static (string Message, Dictionary<string, string> Errors) BuildExceptionDetails(Exception ex)
    {
        var errors = new Dictionary<string, string>();
        var messageBuilder = new System.Text.StringBuilder();

        var current = ex;
        var level = 0;
        while (current is not null)
        {
            var prefix = level == 0 ? "Exception" : $"InnerException[{level}]";

            if (messageBuilder.Length > 0)
                messageBuilder.Append(" | ");
            messageBuilder.Append($"{current.GetType().Name}: {current.Message}");

            errors[$"{prefix}.Type"] = current.GetType().FullName ?? current.GetType().Name;
            errors[$"{prefix}.Message"] = current.Message;

            if (!string.IsNullOrWhiteSpace(current.StackTrace))
                errors[$"{prefix}.StackTrace"] = current.StackTrace;

            if (current.Source is not null)
                errors[$"{prefix}.Source"] = current.Source;

            if (current.TargetSite is not null)
                errors[$"{prefix}.TargetSite"] = current.TargetSite.ToString() ?? string.Empty;

            current = current.InnerException;
            level++;
        }

        return (messageBuilder.ToString(), errors);
    }
}