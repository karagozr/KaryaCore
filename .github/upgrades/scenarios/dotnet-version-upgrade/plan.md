# .NET 10 Version Upgrade Plan

## Overview

**Target**: Upgrade KaryaCore solution from .NET 9 to .NET 10
**Scope**: 5 projects (~9,258 LOC), all SDK-style, all currently on net9.0

### Selected Strategy
**All-At-Once** — All projects upgraded simultaneously in a single operation.
**Rationale**: 5 projects, all on .NET 9, moderate complexity with clear dependency structure. The solution is small enough for atomic upgrade while dependency graph (3 tiers) allows effective validation.

## Tasks

### 01-prerequisites: Verify SDK and tooling readiness

Verify that .NET 10 SDK is installed and configured correctly. Check that global.json (if present) is compatible with .NET 10 toolchain. Validate that all development tooling (IDE, CLI) can target net10.0.

**Assessment context**: All 5 projects are SDK-style and use modern .NET, so toolchain compatibility is straightforward. No legacy project formats requiring conversion.

**Done when**: .NET 10 SDK installed and verified via `dotnet --list-sdks`, global.json compatibility confirmed (or absent), solution can be opened without SDK version warnings.

---

### 02-upgrade-all-projects: Upgrade all projects to .NET 10

Update target framework to net10.0 for all 5 projects, upgrade all NuGet packages to .NET 10-compatible versions, and fix resulting compilation issues. This is an atomic operation covering all projects simultaneously.

**Scope**: 
- Karya.Core (foundation library, 45 files, 1556 LOC)
- Karya.Core.App (business logic, 19 files, 329 LOC)
- Karya.Core.Web (web infrastructure, 10 files, 632 LOC)
- Karya.Core.Indentity (identity/auth, 67 files, 2145 LOC)
- Karya.Test.Web.Api (ASP.NET Core API, 51 files, 4596 LOC)

**Assessment context**:
- **11 packages need upgrade**: Microsoft.AspNetCore.* packages (9.0.x → 10.0.10), Microsoft.EntityFrameworkCore.* (9.0.14 → 10.0.10), Newtonsoft.Json (13.0.3 → 13.0.4), System.Formats.Asn1, System.Security.Cryptography.Xml
- **2 packages included in framework**: System.Reflection.Emit and System.Reflection.Emit.Lightweight can be removed (functionality now in framework)
- **12 API compatibility issues**: 6 binary incompatible (JwtSecurityTokenHandler, ConfigurationBinder.Get), 3 source incompatible (TimeSpan.FromDays, IdentityEntityFrameworkBuilderExtensions), 3 behavioral changes (JsonSerializer.Deserialize, Uri constructor)
- **High-risk area**: IdentityModel migration affecting Karya.Core.Indentity and Karya.Test.Web.Api (5 JWT-related APIs need attention)

**Research starting points**:
- JWT token generation in Karya.Test.Web.Api — check JwtSecurityTokenHandler usage patterns
- Identity configuration in Karya.Core.Indentity — verify AddEntityFrameworkStores compatibility
- Configuration binding in startup code — check ConfigurationBinder.Get calls
- TimeSpan usage in Karya.Core.Indentity — verify integer overload changes

**Done when**: All 5 projects target net10.0, all package references updated to .NET 10-compatible versions, solution builds with 0 errors and 0 warnings, all API compatibility issues resolved.

---

### 03-final-validation: Validate upgrade success

Build the full solution, run all tests, and document any deferred modernization recommendations.

**Scope**: Complete solution validation across all 5 projects. Verify that test project (Karya.Test.Web.Api) functions correctly with upgraded dependencies.

**Done when**: Solution builds successfully, all existing tests pass, runtime behavior verified (if tests exist), any deferred modernization items (nullable reference types, System.Web package removals) documented in upgrade summary.
