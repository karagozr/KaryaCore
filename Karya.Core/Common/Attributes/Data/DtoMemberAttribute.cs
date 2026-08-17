namespace Karya.Core.Common.Attributes.Data;


[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class DtoMemberAttribute : Attribute
{
    public bool InSelect { get; set; }

    public bool InSingle { get; set; }

    public bool InInsert { get; set; }

    public bool InUpdate { get; set; }

    public bool All
    {
        get => InSelect && InSingle && InInsert && InUpdate;
        set
        {
            InSelect = value;
            InSingle = value;
            InInsert = value;
            InUpdate = value;
        }
    }

    public DtoMemberAttribute(
        bool inSelect = false,
        bool inSingle = false,
        bool inInsert = false,
        bool inUpdate = false)
    {
        InSelect = inSelect;
        InSingle = inSingle;
        InInsert = inInsert;
        InUpdate = inUpdate;
    }
}
