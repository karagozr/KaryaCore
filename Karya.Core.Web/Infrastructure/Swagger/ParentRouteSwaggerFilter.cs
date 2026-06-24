using Microsoft.OpenApi;
using Newtonsoft.Json.Schema;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Globalization;
using System.Reflection;
using System.Text.Json.Nodes;

namespace Karya.Core.Web.Infrastructure.Swagger;

public class ParentRouteSwaggerFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // 1. Parametreyi yakala
        var parentParam = operation.Parameters
            .FirstOrDefault(p => p.Name.Equals("parent", StringComparison.OrdinalIgnoreCase));

        if (parentParam == null) return;

        // 2. Çalışma zamanındaki asıl somut controller tipini güvenle bul
        var controllerType = context.ApiDescription.ActionDescriptor is Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor cad
            ? cad.ControllerTypeInfo.AsType()
            : context.MethodInfo.DeclaringType;

        if (controllerType == null) return;

        Type? genericBaseType = null;
        var currentType = controllerType;

        while (currentType != null && currentType != typeof(object))
        {
            if (currentType.IsGenericType)
            {
                var genericDefinition = currentType.GetGenericTypeDefinition();
                if (genericDefinition.GetGenericArguments().Length == 7)
                {
                    genericBaseType = currentType;
                    break;
                }
            }
            currentType = currentType.BaseType;
        }

        if (genericBaseType == null) return;

        // 3. TParentFilter tipini al ve property'lerini tarayarak şablon oluştur
        var parentFilterType = genericBaseType.GetGenericArguments()[2];
        var properties = parentFilterType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        // Varsayılan fallback şablonu
        string rawJsonTemplate = "{\"id\":\"value\"}";

        if (properties.Any())
        {
            var parts = properties.Select(p =>
            {
                if (string.IsNullOrEmpty(p.Name)) return string.Empty;

                // Türkçe 'ı' harfi hatasını engellemek için InvariantCulture ile camelCase yapıyoruz
                string camelCaseName = p.Name.Substring(0, 1).ToLower(CultureInfo.InvariantCulture) + p.Name.Substring(1);

                // Tip kontrolüne göre örnek değer ata
                string mockVal = p.PropertyType == typeof(int) || p.PropertyType == typeof(long) ? "1" : "01";

                // Çıktı: "firmId":"01"
                return $"\"{camelCaseName}\":\"{mockVal}\"";
            }).Where(s => !string.IsNullOrEmpty(s));

            // İstediğiniz tam format: {firmId:"01",inventoryId:"01"}
            rawJsonTemplate = $"{{{string.Join(",", parts)}}}";
        }

        // 4. OpenAPI Nesnelerini güvenli cast ile ezme adımı
        if (parentParam is OpenApiParameter concreteParam)
        {
            var customSchema = new OpenApiSchema
            {
                Type = Microsoft.OpenApi.JsonSchemaType.String // Swagger'a bunun bir path dizesi olduğunu bildir
            };

            // ÖNEMLİ: Kaçış karakterlerini (\) engellemek için JsonNode.Parse kullanıyoruz.
            // Bu sayede Swagger ham bir string nesnesi üretir ve tırnak işaretlerini bozmaz.
            var cleanJsonNode = JsonNode.Parse($"\"{rawJsonTemplate.Replace("\"", "\\\"")}\"");

            customSchema.Default = cleanJsonNode;
            concreteParam.Schema = customSchema;

            concreteParam.Required = true;
            concreteParam.In = ParameterLocation.Path;
            concreteParam.Example = cleanJsonNode;
            concreteParam.Description = "OData Parent Filtre Yapısı.";
        }
    }
}