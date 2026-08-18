namespace Karya.Core.Common.Attributes.Data;

[Flags]
public enum DtoTargets
{
    ____ = 0,
    ___U = 1,
    __I_ = 2,
    __IU = ___U | __I_,
    _B__ = 4,
    _B_U = _B__ | ___U,
    _BI_ = _B__ | __I_,
    _BIU = _B__ | __I_ | ___U,
    S___ = 8,
    S__U = S___ | ___U,
    S_I_ = S___ | __I_,
    S_IU = S___ | __I_ | ___U,
    SB__ = S___ | _B__,
    SB_U = S___ | _B__ | ___U,
    SBI_ = S___ | _B__ | __I_,
    SBIU = S___ | _B__ | __I_ | ___U
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class DtoMemberAttribute : Attribute
{
    public DtoTargets Targets { get; }
    public string? DisplayName { get; set; }

    public DtoMemberAttribute(DtoTargets targets, string? displayName = null)
    {
        Targets = targets;
        DisplayName = displayName;
    }

    public bool InSelect => Targets.HasFlag(DtoTargets.S___);

    public bool InSingle => Targets.HasFlag(DtoTargets._B__);

    public bool InInsert => Targets.HasFlag(DtoTargets.__I_);

    public bool InUpdate => Targets.HasFlag(DtoTargets.___U);
}
