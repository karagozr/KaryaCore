using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace Karya.Core.Web.Infrastructure.OpenApi;

/// <summary>
/// OpenAPI dökümanına Bearer (JWT) güvenlik şemasını ekler ve tüm operasyonlara
/// güvenlik gereksinimi uygular. Swashbuckle AddSecurityDefinition/Requirement
/// yerine .NET 9 yerleşik OpenAPI transformer'ı olarak çalışır.
/// </summary>
public sealed class BearerSecurityDocumentTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var scheme = new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Token gir: Bearer {token}",
            Reference = new OpenApiReference
            {
                Id = "Bearer",
                Type = ReferenceType.SecurityScheme
            }
        };

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, OpenApiSecurityScheme>();
        document.Components.SecuritySchemes["Bearer"] = scheme;

        var requirement = new OpenApiSecurityRequirement
        {
            [scheme] = new List<string>()
        };

        if (document.Paths is not null)
        {
            foreach (var operation in document.Paths.Values.SelectMany(path => path.Operations.Values))
            {
                operation.Security ??= new List<OpenApiSecurityRequirement>();
                operation.Security.Add(requirement);
            }
        }

        return Task.CompletedTask;
    }
}
