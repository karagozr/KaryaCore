# Upgrade Options — KaryaCore

Assessment: 5 projects, all net9.0 → net10.0, 11 package upgrades, 12 API issues (6 binary, 3 source, 3 behavioral), high-risk IdentityModel migration

## Strategy

### Upgrade Strategy
Recommended All-at-Once for 5 projects with moderate complexity. The solution is small enough that a single atomic upgrade minimizes overhead while the dependency graph is shallow enough (3 tiers) to validate effectively.

| Value | Description |
|-------|-------------|
| **All-at-Once** (selected) | Upgrade all 5 projects simultaneously in a single atomic pass |
| Top-Down | Upgrade entry-point applications first with multi-targeting libraries |

## Compatibility

### Unsupported API Handling
12 API compatibility issues detected: 6 binary incompatible (high priority), 3 source incompatible, 3 behavioral changes. Primary concern is IdentityModel migration affecting Karya.Core.Indentity and Karya.Test.Web.Api.

| Value | Description |
|-------|-------------|
| **Fix During Upgrade** (selected) | Address API issues as they are encountered during project upgrade |
| Research First | Identify all API issues upfront, document migration paths, then upgrade |

## Modernization

### Nullable Reference Types
Target is net10.0 and nullable reference types are not currently enabled. Enabling NRTs improves null-safety and takes advantage of C# 8+ features.

| Value | Description |
|-------|-------------|
| Enable NRTs | Turn on nullable reference types and resolve warnings as part of the upgrade |
| **Skip for Now** (selected) | Focus on framework upgrade only, defer nullable migration to a future task |
