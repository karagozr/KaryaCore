using System.Security.Claims;
using Karya.Core.Indentity.Services;
using Karya.Test.Web.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Karya.Test.Web.Api.Data.Service;

/// <summary>
/// Database-backed implementation that resolves a user's effective roles and
/// claims from direct role assignments and role group memberships.
/// </summary>
public class UserClaimsService : IUserClaimsService
{
    private readonly AppDbContext _db;

    public UserClaimsService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<string>> GetEffectiveRolesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        // Direct role assignments (AspNetUserRoles).
        var directRoleIds = _db.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.RoleId);

        // Roles inherited from role group memberships.
        var groupRoleIds = _db.UserRoleGroups
            .Where(urg => urg.UserId == userId)
            .SelectMany(urg => _db.RoleGroupRoles
                .Where(rgr => rgr.RoleGroupId == urg.RoleGroupId)
                .Select(rgr => rgr.RoleId));

        var roleNames = await _db.Roles
            .Where(r => directRoleIds.Contains(r.Id) || groupRoleIds.Contains(r.Id))
            .Select(r => r.Name!)
            .Distinct()
            .ToListAsync(cancellationToken);

        return roleNames;
    }

    public async Task<IReadOnlyList<Claim>> GetUserClaimsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var claims = new List<Claim>();

        // Effective role ids (direct + via role groups).
        var directRoleIds = _db.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.RoleId);

        var groupRoleIds = _db.UserRoleGroups
            .Where(urg => urg.UserId == userId)
            .SelectMany(urg => _db.RoleGroupRoles
                .Where(rgr => rgr.RoleGroupId == urg.RoleGroupId)
                .Select(rgr => rgr.RoleId));

        var roleIds = await _db.Roles
            .Where(r => directRoleIds.Contains(r.Id) || groupRoleIds.Contains(r.Id))
            .Select(r => new { r.Id, r.Name })
            .Distinct()
            .ToListAsync(cancellationToken);

        // Role name claims.
        claims.AddRange(roleIds
            .Where(r => r.Name != null)
            .Select(r => new Claim(ClaimTypes.Role, r.Name!)));

        // Claims attached to the effective roles (AspNetRoleClaims).
        var effectiveRoleIds = roleIds.Select(r => r.Id).ToList();
        var roleClaims = await _db.RoleClaims
            .Where(rc => effectiveRoleIds.Contains(rc.RoleId))
            .Select(rc => new { rc.ClaimType, rc.ClaimValue })
            .ToListAsync(cancellationToken);

        claims.AddRange(roleClaims
            .Where(rc => rc.ClaimType != null)
            .Select(rc => new Claim(rc.ClaimType!, rc.ClaimValue ?? string.Empty)));

        // Direct user claims (AspNetUserClaims).
        var userClaims = await _db.UserClaims
            .Where(uc => uc.UserId == userId)
            .Select(uc => new { uc.ClaimType, uc.ClaimValue })
            .ToListAsync(cancellationToken);

        claims.AddRange(userClaims
            .Where(uc => uc.ClaimType != null)
            .Select(uc => new Claim(uc.ClaimType!, uc.ClaimValue ?? string.Empty)));

        return claims;
    }
}
