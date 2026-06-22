using Karya.Core.Interfaces.DTOs;
using System.Reflection;
using System.Text.Json;

namespace Karya.Core.Helpers.Generals;

public static class UpdateDtoHelper
{
    public static T ToObject<T>(this Dictionary<string, object> source) where T : IUpdateDto, new()
    {
        var target = new T();
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var item in source)
        {
            var property = Array.Find(properties, p => p.Name.Equals(item.Key, StringComparison.OrdinalIgnoreCase));

            if (property != null && property.CanWrite)
            {
                var incomingValue = item.Value;

                if (incomingValue == null)
                {
                    property.SetValue(target, null);
                    continue;
                }

                var targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                if (incomingValue is JsonElement jsonElement)
                {
                    if (jsonElement.ValueKind == JsonValueKind.Null)
                    {
                        property.SetValue(target, null);
                        continue;
                    }

                    var convertedJsonValue = JsonSerializer.Deserialize(jsonElement.GetRawText(), property.PropertyType);
                    property.SetValue(target, convertedJsonValue);
                }
                else
                {
                    var convertedValue = Convert.ChangeType(incomingValue, targetType);
                    property.SetValue(target, convertedValue);
                }
            }
        }

        return target;
    }

    public static string[] GetFieldsToUpdate<TUpdateDto, TEntity>(Dictionary<string, object> incomingData)
    {
        if (incomingData == null || !incomingData.Any())
        {
            return Array.Empty<string>();
        }

        var dtoProperties = typeof(TUpdateDto).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var entityProperties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var updatedFields = new List<string>();

        foreach (var key in incomingData.Keys)
        {
            var dtoProp = dtoProperties.FirstOrDefault(p => p.Name.Equals(key, StringComparison.OrdinalIgnoreCase));

            var entityProp = entityProperties.FirstOrDefault(p => p.Name.Equals(key, StringComparison.OrdinalIgnoreCase));

            if (dtoProp != null && entityProp != null && entityProp.CanWrite)
            {
                updatedFields.Add(entityProp.Name);
            }
        }

        return updatedFields.ToArray();
    }
}

