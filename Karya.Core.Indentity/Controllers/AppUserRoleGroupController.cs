using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.DTOs;
using Karya.Core.Interfaces.Identities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Controllers;

/// <summary>
/// Kullanıcı-rol grubu atamaları. Composite key (UserId+RoleGroupId) korunur;
/// atama/kaldırma ve kullanıcıya göre listeleme sağlanır. Yalnızca Sistem Admin erişebilir.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AppUserRoleGroupController : ControllerBase
{
    private readonly DbContext _context;
    private readonly ICurrentUser _currentUser;

    public AppUserRoleGroupController(DbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    private DbSet<AppUserRoleGroup> Set => _context.Set<AppUserRoleGroup>();

    /// <summary>Bir kullanıcının rol grubu üyeliklerini listeler.</summary>
    [HttpGet("by-user/{userId}")]
    public async Task<ActionResult> ByUser(Guid userId)
    {
        var items = await Set.AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(x => new AppUserRoleGroupAssignDto { UserId = x.UserId, RoleGroupId = x.RoleGroupId })
            .ToListAsync();
        return Ok(items);
    }

    /// <summary>Kullanıcıyı rol grubuna ekler (zaten varsa yok sayar).</summary>
    [HttpPost]
    public async Task<ActionResult> Assign([FromBody] AppUserRoleGroupAssignDto dto)
    {
        var exists = await Set.AnyAsync(x => x.UserId == dto.UserId && x.RoleGroupId == dto.RoleGroupId);
        if (!exists)
        {
            Set.Add(new AppUserRoleGroup { UserId = dto.UserId, RoleGroupId = dto.RoleGroupId });
            await _context.SaveChangesAsync();
        }
        return Ok();
    }

    /// <summary>Kullanıcıyı rol grubundan çıkarır.</summary>
    [HttpDelete]
    public async Task<ActionResult> Unassign([FromBody] AppUserRoleGroupAssignDto dto)
    {
        var entity = await Set.FirstOrDefaultAsync(x => x.UserId == dto.UserId && x.RoleGroupId == dto.RoleGroupId);
        if (entity is null)
            return NotFound();

        Set.Remove(entity);
        await _context.SaveChangesAsync();
        return Ok();
    }
}

