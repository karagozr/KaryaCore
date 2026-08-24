using Karya.Core.Web.Abstracts.Controllers;
using System.Reflection;

namespace Karya.Core.Indentity.Providers;

public static class RoleProvider
{
    private static readonly string[] Actions = ["Read", "Create", "Update", "Delete"];

    public static List<RoleDefinition> GetRoles()
    {
        var roles = AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => !x.IsDynamic)
            .SelectMany(GetLoadableTypes)
            .Where(x => !x.IsAbstract)
            .Select(GetEntityType)
            .Where(x => x is not null)
            .Distinct()
            .SelectMany(entityType => Actions.Select(action => new RoleDefinition(
                $"{entityType!.Name}.{action}",
                $"{entityType.Name} {action}"
            )))
            .GroupBy(x => x.Name, StringComparer.OrdinalIgnoreCase)
            .Select(x => x.First())
            .OrderBy(x => x.Name)
            .ToList();

        return roles;
    }

    private static Type? GetEntityType(Type type)
    {
        var baseType = GetCrudBaseType(type);
        return baseType?.GetGenericArguments()[0];
    }

    private static Type? GetCrudBaseType(Type type)
    {
        var current = type;

        while (current is not null)
        {
            if (current.IsGenericType)
            {
                var genericType = current.GetGenericTypeDefinition();

                if (genericType == typeof(BaseCrudController<,,,,,>) ||
                    genericType == typeof(BaseCrudDetailController<,,,,,,>))
                    return current;
            }

            current = current.BaseType;
        }

        return null;
    }

    private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            return ex.Types.Where(x => x is not null).Cast<Type>();
        }
        catch
        {
            return [];
        }
    }
}

public record RoleDefinition(string Name, string Description);