# 02-upgrade-all-projects: Upgrade all projects to .NET 10

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
