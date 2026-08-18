using Karya.Core.Interfaces.DTOs;

namespace Karya.Core.Indentity.DTOs;

public class AppRoleSDto : IByKeyDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}

public class AppRoleLDto : ISelectDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}

public class AppRoleADto : IInsertDto
{
    public string Name { get; set; } = null!;
}

public class AppRoleUDto : IUpdateDto
{
    public string Name { get; set; } = null!;
}
