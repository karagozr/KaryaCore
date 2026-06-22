namespace Karya.Core.Common.Attributes.Data;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class TenantForeignKeyAttribute : Attribute
{
    public string CategoryIdPropertyName { get; }

    public TenantForeignKeyAttribute(string categoryIdPropertyName)
    {
        CategoryIdPropertyName = categoryIdPropertyName;
    }
}