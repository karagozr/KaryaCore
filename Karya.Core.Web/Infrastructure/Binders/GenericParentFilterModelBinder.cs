using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Karya.Core.Web.Infrastructure.Binders;

public class GenericParentFilterModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null) throw new ArgumentNullException(nameof(bindingContext));

        var valueProviderResult = bindingContext.ValueProvider.GetValue("parent");
        if (valueProviderResult == ValueProviderResult.None) return Task.CompletedTask;

        string? rawValue = valueProviderResult.FirstValue;
        if (string.IsNullOrEmpty(rawValue)) return Task.CompletedTask;

        // URL Kodlamalarını (%7B -> {, %22 -> " vb.) düz metne dönüştür
        rawValue = WebUtility.UrlDecode(rawValue);

        var model = Activator.CreateInstance(bindingContext.ModelType);
        if (model == null) return Task.CompletedTask;

        // Süslü parantez içindeki JSON benzeri "key":"value" yapılarını güvenle yakalayan Regex
        var regex = new Regex(@"[""']?([a-zA-Z0-9_]+)[""']?\s*[:=]\s*[""']?([^,""'}]*)[""']?");
        var matches = regex.Matches(rawValue);

        foreach (Match match in matches)
        {
            if (match.Groups.Count == 3)
            {
                string key = match.Groups[1].Value;
                string val = match.Groups[2].Value;

                var property = bindingContext.ModelType.GetProperty(key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (property != null && property.CanWrite)
                {
                    var underlyingType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    object? convertedValue;

                    if (underlyingType == typeof(Guid))
                        convertedValue = Guid.Parse(val);
                    else
                        convertedValue = Convert.ChangeType(val, underlyingType);

                    property.SetValue(model, convertedValue);
                }
            }
        }

        // Implicit validation mekanizmasını ezerek 400 Bad Request fırlatılmasını engelle
        bindingContext.ValidationState[model] = new Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidationStateEntry
        {
            SuppressValidation = true
        };

        bindingContext.Result = ModelBindingResult.Success(model);
        return Task.CompletedTask;
    }
}