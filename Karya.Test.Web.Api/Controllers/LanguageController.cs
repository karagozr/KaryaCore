using Karya.Test.Web.Api.Data;
using Karya.Test.Web.Api.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Karya.Test.Web.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LanguageController : ControllerBase
{
    private readonly AppDbContext _db;

    public LanguageController(AppDbContext db) => _db = db;

    /// <summary>Returns the client-facing language pack (Client + Both) as a code -> text map.</summary>
    [AllowAnonymous]
    [HttpGet("{lang}")]
    public async Task<IActionResult> Pack(string lang)
    {
        var code = (lang ?? "tr").Trim().ToLowerInvariant();

        var pack = await _db.LocalizationResources
            .AsNoTracking()
            .Where(r => r.LanguageCode == code &&
                        (r.Scope == LocalizationScope.Client || r.Scope == LocalizationScope.Both))
            .ToDictionaryAsync(r => r.Code, r => r.Value);

        return Ok(pack);
    }
}
