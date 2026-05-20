using Karya.Core.App.Commons;
using Karya.Core.App.Features.Handlers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Karya.Core.App;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;

    public static void AddCoreAppRegistiration(this IServiceCollection services)
    {
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly);
            cfg.AddOpenBehavior(typeof(ExceptionBehavior<,>));
        });
   


        services.AddTransient(typeof(IRequestHandler<,>), typeof(CrudHandler<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
    }
}