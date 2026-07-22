using System.Security.Claims;

namespace Karya.Core.Indentity.Services;

/// <summary>
/// Resolves a user's effective roles and claims from the database
/// (direct role assignments + role group memberships), instead of
/// embedding them inside the access token.
/// </summary>
public interface IUserClaimsService
{
    /// <summary>
    /// Returns the distinct set of role names the user effectively has,
    /// combining direct role assignments and roles inherited from role groups.
    /// </summary>
    Task<IReadOnlyList<string>> GetEffectiveRolesAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the effective claims for the user: role claims (as <see cref="ClaimTypes.Role"/>),
    /// role-based claims and direct user claims.
    /// </summary>
    Task<IReadOnlyList<Claim>> GetUserClaimsAsync(Guid userId, CancellationToken cancellationToken = default);
}
