# 01-prerequisites: Verify SDK and tooling readiness

Verify that .NET 10 SDK is installed and configured correctly. Check that global.json (if present) is compatible with .NET 10 toolchain. Validate that all development tooling (IDE, CLI) can target net10.0.

**Assessment context**: All 5 projects are SDK-style and use modern .NET, so toolchain compatibility is straightforward. No legacy project formats requiring conversion.

**Done when**: .NET 10 SDK installed and verified via `dotnet --list-sdks`, global.json compatibility confirmed (or absent), solution can be opened without SDK version warnings.
