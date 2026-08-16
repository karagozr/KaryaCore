using System.ComponentModel.DataAnnotations.Schema;

namespace Karya.Core.Common.Attributes.Data;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class TenantForeignKeyAttribute : ForeignKeyAttribute
{
    public TenantForeignKeyAttribute(string fkPropertyName)
        : base($"TenantId,{fkPropertyName}")
    {
    }
}
