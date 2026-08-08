# .NET 10 Version Upgrade

## Preferences
- **Flow Mode**: Automatic
- **Target Framework**: net10.0 (.NET 10 - LTS, support ends November 2028)

## Source Control
- **Source Branch**: master
- **Working Branch**: upgrade-dotnet-10
- **Commit Strategy**: After Each Task
- **Branch Sync**: Auto (Merge)

## Upgrade Options
**Source**: .github/upgrades/scenarios/dotnet-version-upgrade/upgrade-options.md

### Strategy
- Upgrade Strategy: All-at-Once

### Compatibility
- Unsupported API Handling: Fix During Upgrade

### Modernization
- Nullable Reference Types: Skip for Now

## Strategy
**Selected**: All-at-Once
**Rationale**: 5 projects all on .NET 9, moderate complexity with clear dependency structure. Small enough for atomic upgrade with effective validation.

### Execution Constraints
- Single atomic upgrade — all projects updated together
- Validate full solution build after upgrade (0 errors, 0 warnings)
- Fix API compatibility issues as encountered during upgrade
- Commit after each completed task
