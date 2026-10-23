using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Interfaces.Identities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Identity.Controllers;

/// <summary>
/// Kullanýcý-tenant üyelikleri. Composite key (UserId+TenantId) korunur; atama/kaldýrma
/// ve kullanýcýya göre listeleme saðlanýr. Yalnýzca Sistem Admin eriþebilir.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AppUserTenantController : ControllerBase
{
    private readonly DbContext _context;
    private readonly ICurrentUser _currentUser;

    public AppUserTenantController(DbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    private DbSet<AppUserTenant> Set => _context.Set<AppUserTenant>();

    /// <summary>Bir kullanýcýnýn tenant üyeliklerini listeler.</summary>
    [HttpGet("by-user/{userId}")]
    public async Task<ActionResult> ByUser(Guid userId)
    {
        var items = await Set.AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(x => new AppUserTenantAssignDto { UserId = x.UserId, TenantId = x.TenantId })
            .ToListAsync();
        return Ok(items);
    }

    /// <summary>Kullanýcýya tenant üyeliði ekler (zaten varsa yok sayar).</summary>
    [HttpPost]
    public async Task<ActionResult> Assign([FromBody] AppUserTenantAssignDto dto)
    {
        var exists = await Set.AnyAsync(x => x.UserId == dto.UserId && x.TenantId == dto.TenantId);
        if (!exists)
        {
            Set.Add(new AppUserTenant { UserId = dto.UserId, TenantId = dto.TenantId });
            await _context.SaveChangesAsync();
        }
        return Ok();
    }

    /// <summary>Kullanýcýdan tenant üyeliðini kaldýrýr.</summary>
    [HttpDelete]
    public async Task<ActionResult> Unassign([FromBody] AppUserTenantAssignDto dto)
    {
        var entity = await Set.FirstOrDefaultAsync(x => x.UserId == dto.UserId && x.TenantId == dto.TenantId);
        if (entity is null)
            return NotFound();

        Set.Remove(entity);
        await _context.SaveChangesAsync();
        return Ok();
    }
}
