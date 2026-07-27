using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Reflection;

namespace Karya.Core.Web.Infrastructure.OpenApi;

/// <summary>
/// DevExtreme detail controller'larındaki <c>parent</c> route parametresi için
/// örnek OData parent-filtre şablonu üretir. Swashbuckle IOperationFilter yerine
/// .NET 9 yerleşik OpenAPI transformer'ı olarak çalışır.
/// </summary>
public sealed class ParentRouteOperationTransformer : IOpenApiOperationTransformer
{
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        // 1. "parent" parametresini yakala
        var parentParam = operation.Parameters?
            .FirstOrDefault(p => p.Name.Equals("parent", StringComparison.OrdinalIgnoreCase));

        if (parentParam is null)
            return Task.CompletedTask;

        // 2. Somut controller tipini bul
        var controllerType = context.Description.ActionDescriptor is ControllerActionDescriptor cad
            ? cad.ControllerTypeInfo.AsType()
            : null;

        if (controllerType is null)
            return Task.CompletedTask;

        // 3. 7 generic argümanlı base tipini (BaseCrudDetailController) bul
        Type? genericBaseType = null;
        var currentType = controllerType;
        while (currentType is not null && currentType != typeof(object))
        {
            if (currentType.IsGenericType &&
                currentType.GetGenericTypeDefinition().GetGenericArguments().Length == 7)
            {
                genericBaseType = currentType;
                break;
            }
            currentType = currentType.BaseType;
        }

        if (genericBaseType is null)
            return Task.CompletedTask;

        // 4. TParentFilter tipinin property'lerinden örnek şablon oluştur
        var parentFilterType = genericBaseType.GetGenericArguments()[2];
        var properties = parentFilterType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        string rawJsonTemplate = "{\"id\":\"value\"}";

        if (properties.Length > 0)
        {
            var parts = properties.Select(p =>
            {
                if (string.IsNullOrEmpty(p.Name)) return string.Empty;

                string camelCaseName = p.Name.Substring(0, 1).ToLower(CultureInfo.InvariantCulture) + p.Name.Substring(1);
                string mockVal = p.PropertyType == typeof(int) || p.PropertyType == typeof(long) ? "1" : "01";
                return $"\"{camelCaseName}\":\"{mockVal}\"";
            }).Where(s => !string.IsNullOrEmpty(s));

            rawJsonTemplate = $"{{{string.Join(",", parts)}}}";
        }

        // 5. Parametreyi güncelle
        var sample = new OpenApiString(rawJsonTemplate);
        parentParam.Schema = new OpenApiSchema
        {
            Type = "string",
            Default = sample
        };
        parentParam.Required = true;
        parentParam.In = ParameterLocation.Path;
        parentParam.Example = sample;
        parentParam.Description = "OData Parent Filtre Yapısı.";

        return Task.CompletedTask;
    }
}
