using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.DTOs;
using Karya.Core.Interfaces.Identities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Controllers;

/// <summary>
/// Kullanıcı-tenant üyelikleri. Composite key (UserId+TenantId) korunur; atama/kaldırma
/// ve kullanıcıya göre listeleme sağlanır. Yalnızca Sistem Admin erişebilir.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserTenantController : ControllerBase
{
    private readonly DbContext _context;
    private readonly ICurrentUser _currentUser;

    public UserTenantController(DbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    private DbSet<AppUserTenant> Set => _context.Set<AppUserTenant>();

    /// <summary>Bir kullanıcının tenant üyeliklerini listeler.</summary>
    [HttpGet("by-user/{userId}")]
    public async Task<ActionResult> ByUser(Guid userId)
    {
        var items = await Set.AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(x => new AppUserTenantAssignDto { UserId = x.UserId, TenantId = x.TenantId })
            .ToListAsync();
        return Ok(items);
    }

    /// <summary>Kullanıcıya tenant üyeliği ekler (zaten varsa yok sayar).</summary>
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

    /// <summary>Kullanıcıdan tenant üyeliğini kaldırır.</summary>
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
