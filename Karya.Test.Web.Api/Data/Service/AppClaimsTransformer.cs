using System.Security.Claims;
using Karya.Core.Indentity.Services;
using Microsoft.AspNetCore.Authentication;
using OpenIddict.Abstractions;

namespace Karya.Test.Web.Api.Data.Service;

/// <summary>
/// Enriches the authenticated principal on every request by loading the user's
/// effective roles and claims from the database (direct + role group based),
/// instead of relying on claims embedded in the access token.
/// </summary>
public class AppClaimsTransformer : IClaimsTransformation
{
    private const string TransformedMarker = "__db_claims_loaded";

    private readonly IUserClaimsService _userClaimsService;

    public AppClaimsTransformer(IUserClaimsService userClaimsService)
    {
        _userClaimsService = userClaimsService;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.Identity is not ClaimsIdentity identity || !identity.IsAuthenticated)
            return principal;

        // IClaimsTransformation can run multiple times per request; guard against duplicates.
        if (identity.HasClaim(c => c.Type == TransformedMarker))
            return principal;

        var userIdValue =
            principal.FindFirstValue(OpenIddictConstants.Claims.Subject)
            ?? principal.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdValue, out var userId))
            return principal;

        var claims = await _userClaimsService.GetUserClaimsAsync(userId);

        foreach (var claim in claims)
        {
            // Map role claims to the identity's role claim type so that
            // IsInRole / [Authorize(Roles = "...")] work regardless of the
            // token's original role claim type.
            var claimType = claim.Type == ClaimTypes.Role ? identity.RoleClaimType : claim.Type;

            if (!identity.HasClaim(claimType, claim.Value))
                identity.AddClaim(new Claim(claimType, claim.Value));
        }

        identity.AddClaim(new Claim(TransformedMarker, "1"));

        return principal;
    }
}
