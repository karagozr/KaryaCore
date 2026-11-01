using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;

namespace Karya.Core.Identity.Infrastructure;

/// Kütüphanenin varsayılan controller'larını ekler. Uygulamada aynı abstract controller'dan
/// türeyen bir controller varsa varsayılanı eklemez; böylece uygulamanın controller'ı kullanılır.
internal sealed class CoreIdentityControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
{
    private const string DefaultsNamespace = "Karya.Core.Identity.Controllers.Defaults";

    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
    {
        var defaults = AssemblyReference.Assembly.GetTypes()
            .Where(t => t.Namespace == DefaultsNamespace && t.IsClass && !t.IsAbstract)
            .Select(t => t.GetTypeInfo());

        foreach (var controller in defaults)
        {
            var baseType = controller.BaseType!;

            var overridden = feature.Controllers.Any(c => c != controller && baseType.IsAssignableFrom(c));

            if (!overridden && !feature.Controllers.Contains(controller))
                feature.Controllers.Add(controller);
        }
    }
}
