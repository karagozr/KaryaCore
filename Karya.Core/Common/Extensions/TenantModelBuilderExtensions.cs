using Karya.Core.Common.Attributes.Data;
using Karya.Core.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Karya.Core.Common.Extensions;

public static class TenantModelBuilderExtensions
{
    public static ModelBuilder ConfigureTenantForeignKeys(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;

            if (!typeof(IEntity).IsAssignableFrom(clrType))
                continue;

            if (clrType.GetProperty("TenantId") == null)
                continue;

            // [TenantForeignKey] is placed on scalar FK properties (e.g. PersonId).
            // We need to find the corresponding navigation property to configure the relationship.
            var fkScalarProperties = clrType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.GetCustomAttribute<TenantForeignKeyAttribute>() != null);

            foreach (var fkScalarProp in fkScalarProperties)
            {
                // fkScalarProp.Name e.g. "PersonId", "AccountManagerPersonId"
                var fkPropertyName = fkScalarProp.Name;

                // Find the navigation property by convention: remove trailing "Id"
                // e.g. "PersonId" -> "Person", "AccountManagerPersonId" -> "AccountManagerPerson"
                var navName = fkPropertyName.EndsWith("Id", StringComparison.Ordinal)
                    ? fkPropertyName[..^2]
                    : fkPropertyName;

                var navProp = clrType.GetProperty(navName, BindingFlags.Public | BindingFlags.Instance);
                if (navProp == null)
                    continue;

                var principalType = navProp.PropertyType;

                // Skip value types, strings, and collections
                if (principalType == typeof(string) || principalType.IsValueType)
                    continue;
                if (principalType.IsGenericType &&
                    principalType.GetGenericTypeDefinition() == typeof(ICollection<>))
                    continue;

                try
                {
                    var entityBuilder = modelBuilder.Entity(clrType);

                    // 1) [InverseProperty] varsa o collection'ı kullan
                    var inversePropertyName = navProp
                        .GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.InversePropertyAttribute>()
                        ?.Property;

                    // 2) [InverseProperty] yoksa principal type üzerinde bu dependent type'ı
                    //    döndüren tek ICollection<T> property'yi convention ile bul
                    if (inversePropertyName == null)
                    {
                        var collectionType = typeof(ICollection<>).MakeGenericType(clrType);
                        var candidates = principalType
                            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                            .Where(p => collectionType.IsAssignableFrom(p.PropertyType))
                            .ToList();

                        // Sadece tek eşleşme varsa güvenle kullan; birden fazlaysa belirsizlik
                        // olacağından WithMany() bırak (EF Core ilişkiyi anonim kurar)
                        if (candidates.Count == 1)
                            inversePropertyName = candidates[0].Name;
                    }

                    var hasOne = entityBuilder.HasOne(principalType, navProp.Name);

                    var withMany = inversePropertyName != null
                        ? hasOne.WithMany(inversePropertyName)
                        : hasOne.WithMany();

                    withMany
                        .HasForeignKey("TenantId", fkPropertyName)
                        .HasPrincipalKey("TenantId", "Id")
                        .OnDelete(DeleteBehavior.NoAction);
                }
                catch
                {
                    // If the relationship is already configured via Fluent API, skip
                }
            }
        }

        return modelBuilder;
    }
}
