using MediatR;
using Karya.Core.App.Interfaces.Commands;
using Karya.Core.App.Interfaces.Services;


namespace Karya.Core.App.Commons;

public class AuthorizationBehavior<TRequest, TResponse>(IPermissionService permissionService, ICurrentUserService currentUser)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IServiceCommand
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        if (!string.IsNullOrEmpty(request.Permission))
        {
            var hasPermission = await permissionService.HasPermissionAsync(currentUser.UserId, request.Permission);
            if (!hasPermission)
                throw new UnauthorizedAccessException($"'{request.Permission}' yetkisi bulunmuyor.");
        }

        return await next();
    }
}