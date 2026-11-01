
namespace Karya.Core.Identity.Application.Services
{
    public interface ICustomRoleProvider
    {
        IEnumerable<RoleDefinition> GetRoles();
    }
}
