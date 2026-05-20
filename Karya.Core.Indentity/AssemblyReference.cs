using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.Infrastructure;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Karya.Core.Indentity;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;

    public static void AddCoreIdentityRegistiration<TIdentityContext>(this IServiceCollection services) where TIdentityContext : IdentityDbContext
    {
        services.AddIdentity<AppUser,AppRole>().AddEntityFrameworkStores<TIdentityContext>();

        // 2. OpenIddict Kaydı
        services.AddOpenIddict()
            .AddCore(options => {
                options.UseEntityFrameworkCore().UseDbContext<TIdentityContext>();
            })
            .AddServer(options => {
                options.SetTokenEndpointUris("/connect/token");
                options.AllowPasswordFlow();
                options.AddDevelopmentEncryptionCertificate().AddDevelopmentSigningCertificate();
                options.UseAspNetCore().EnableTokenEndpointPassthrough();
            })
            .AddValidation(options => {
                options.UseLocalServer();
                options.UseAspNetCore();
            });
    }
}
