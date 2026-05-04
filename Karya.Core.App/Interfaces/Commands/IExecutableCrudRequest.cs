using MediatR;

namespace Karya.Core.App.Interfaces.Commands;

public interface IExecutableCrudRequest<TResponse>:IRequest<TResponse>, IServiceCommand
{
    Task<TResponse> ExecuteAsync(CancellationToken ct = default);
}
