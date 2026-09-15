using Karya.Core.Indentity.Providers;

namespace Karya.Core.Indentity.Services
{
    public interface ICustomRoleProvider
    {
        IEnumerable<RoleDefinition> GetRoles();
    }
}
