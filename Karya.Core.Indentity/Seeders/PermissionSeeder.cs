using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.Infrastructure.Migrations;
using Karya.Core.Web.Abstracts.Controllers;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using System.Security.Claims;

namespace Karya.Core.Indentity.Seeders
{
    public class PermissionSeeder : IDatabaseSeeder
    {
        private readonly RoleManager<AppRole> _roleManager;

        public PermissionSeeder(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            const string adminRoleName = "Admin";
            var adminRole = await _roleManager.FindByNameAsync(adminRoleName);

            if (adminRole is null) return;

            var permissions = GetPermissions();
            var existingClaims = await _roleManager.GetClaimsAsync(adminRole);

            foreach (var permission in permissions)
            {
                var exists = existingClaims.Any(x => x.Type == "Permission" && x.Value == permission);

                if (exists) continue;

                await _roleManager.AddClaimAsync(adminRole, new Claim("Permission", permission));
            }
        }

        private static List<string> GetPermissions()
        {
            var permissions = new List<string>();
            var assembly = Assembly.GetEntryAssembly();

            if (assembly is null) return permissions;

            var controllerTypes = assembly.GetTypes().Where(x => !x.IsAbstract && IsCrudController(x));

            foreach (var controllerType in controllerTypes)
            {
                var entityType = GetEntityType(controllerType);

                if (entityType is null) continue;

                permissions.Add($"{entityType.Name}.Read");
                permissions.Add($"{entityType.Name}.Create");
                permissions.Add($"{entityType.Name}.Update");
                permissions.Add($"{entityType.Name}.Delete");
            }

            return permissions.Distinct().ToList();
        }

        private static bool IsCrudController(Type type)
        {
            return GetCrudBaseType(type) is not null;
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

                    if (genericType == typeof(BaseCrudController<,,,,,>) || genericType == typeof(BaseCrudDetailController<,,,,,,>))
                    {
                        return current;
                    }
                }
                current = current.BaseType;
            }

            return null;
        }
    }
}
