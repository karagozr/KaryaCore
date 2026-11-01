using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Interfaces.Identities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Identity.Controllers;

/// <summary>
/// Kullan�c�-rol atamalar�. Composite key (UserId+RoleId) korunur; atama/kald�rma
/// ve kullan�c�ya g�re listeleme sa�lan�r. Yaln�zca Sistem Admin eri�ebilir.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public abstract class AppUserRoleController : ControllerBase
{
    private readonly DbContext _context;
    private readonly ICurrentUser _currentUser;

    public AppUserRoleController(DbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    private DbSet<AppUserRole> Set => _context.Set<AppUserRole>();

    /// <summary>Bir kullan�c�n�n rol atamalar�n� listeler.</summary>
    [HttpGet("by-user/{userId}")]
    public async Task<ActionResult> ByUser(Guid userId)
    {
        var items = await Set.AsNoTracking()
            .Where(x => x.UserId == userId && x.TenantId == _currentUser.TenantId)
            .Select(x => new AppUserRoleAssignDto { UserId = x.UserId, RoleId = x.RoleId })
            .ToListAsync();
        return Ok(items);
    }

    /// <summary>Kullan�c�ya rol atar (zaten varsa yok sayar).</summary>
    [HttpPost]
    public async Task<ActionResult> Assign([FromBody] AppUserRoleAssignDto dto)
    {
        var exists = await Set.AnyAsync(x => x.UserId == dto.UserId && x.RoleId == dto.RoleId && x.TenantId == _currentUser.TenantId);
        if (!exists)
        {
            Set.Add(new AppUserRole { UserId = dto.UserId, RoleId = dto.RoleId, TenantId = _currentUser.TenantId });
            await _context.SaveChangesAsync();
        }
        return Ok();
    }

    /// <summary>Kullan�c�dan rol� kald�r�r.</summary>
    [HttpDelete]
    public async Task<ActionResult> Unassign([FromBody] AppUserRoleAssignDto dto)
    {
        var entity = await Set.FirstOrDefaultAsync(x => x.UserId == dto.UserId && x.RoleId == dto.RoleId && x.TenantId == _currentUser.TenantId);
        if (entity is null)
            return NotFound();

        Set.Remove(entity);
        await _context.SaveChangesAsync();
        return Ok();
    }
}

