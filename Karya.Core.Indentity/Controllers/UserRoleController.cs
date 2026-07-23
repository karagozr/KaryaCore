using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.DTOs;
using Karya.Core.Interfaces.Identities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Controllers;

/// <summary>
/// Kullanıcı-rol atamaları. Composite key (UserId+RoleId) korunur; atama/kaldırma
/// ve kullanıcıya göre listeleme sağlanır. Yalnızca Sistem Admin erişebilir.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserRoleController : ControllerBase
{
    private readonly DbContext _context;
    private readonly ICurrentUser _currentUser;

    public UserRoleController(DbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    private DbSet<AppUserRole> Set => _context.Set<AppUserRole>();

    /// <summary>Bir kullanıcının rol atamalarını listeler.</summary>
    [HttpGet("by-user/{userId}")]
    public async Task<ActionResult> ByUser(Guid userId)
    {
        var items = await Set.AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(x => new AppUserRoleAssignDto { UserId = x.UserId, RoleId = x.RoleId })
            .ToListAsync();
        return Ok(items);
    }

    /// <summary>Kullanıcıya rol atar (zaten varsa yok sayar).</summary>
    [HttpPost]
    public async Task<ActionResult> Assign([FromBody] AppUserRoleAssignDto dto)
    {
        var exists = await Set.AnyAsync(x => x.UserId == dto.UserId && x.RoleId == dto.RoleId);
        if (!exists)
        {
            Set.Add(new AppUserRole { UserId = dto.UserId, RoleId = dto.RoleId });
            await _context.SaveChangesAsync();
        }
        return Ok();
    }

    /// <summary>Kullanıcıdan rolü kaldırır.</summary>
    [HttpDelete]
    public async Task<ActionResult> Unassign([FromBody] AppUserRoleAssignDto dto)
    {
        var entity = await Set.FirstOrDefaultAsync(x => x.UserId == dto.UserId && x.RoleId == dto.RoleId);
        if (entity is null)
            return NotFound();

        Set.Remove(entity);
        await _context.SaveChangesAsync();
        return Ok();
    }
}

