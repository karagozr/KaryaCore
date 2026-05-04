using Karya.Core.App.Interfaces.Commands;
using MediatR;

namespace Karya.Core.App.Features.Handlers;

public class CrudHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
{
    public Task<TResponse> Handle(TRequest request, CancellationToken ct)
    {
        if (request is IExecutableCrudRequest<TResponse> executableRequest)
            return executableRequest.ExecuteAsync(ct);

        throw new InvalidOperationException(
            $"'{typeof(TRequest).Name}' için handler bulunamadı. " +
            $"IExecutableCrudRequest<{typeof(TResponse).Name}> implement edin veya specific handler kaydedin.");
    }
}