using Karya.Core.Indentity.Domains.Entities;
using Karya.Core.Indentity.DTOs;
using Karya.Core.Interfaces.Identities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Controllers;

/// <summary>
/// Rol grubu-rol atamaları. Composite key (RoleGroupId+RoleId) korunur; atama/kaldırma
/// ve rol grubuna göre listeleme sağlanır. Yalnızca Sistem Admin erişebilir.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AppRoleGroupRoleController : ControllerBase
{
    private readonly DbContext _context;
    private readonly ICurrentUser _currentUser;

    public AppRoleGroupRoleController(DbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    private DbSet<AppRoleGroupRole> Set => _context.Set<AppRoleGroupRole>();

    /// <summary>Bir rol grubunun rollerini listeler.</summary>
    [HttpGet("by-role-group/{roleGroupId}")]
    public async Task<ActionResult> ByRoleGroup(Guid roleGroupId)
    {
        var items = await Set.AsNoTracking()
            .Where(x => x.RoleGroupId == roleGroupId)
            .Select(x => new AppRoleGroupRoleAssignDto { RoleGroupId = x.RoleGroupId, RoleId = x.RoleId })
            .ToListAsync();
        return Ok(items);
    }

    /// <summary>Rol grubuna rol ekler (zaten varsa yok sayar).</summary>
    [HttpPost]
    public async Task<ActionResult> Assign([FromBody] AppRoleGroupRoleAssignDto dto)
    {
        var exists = await Set.AnyAsync(x => x.RoleGroupId == dto.RoleGroupId && x.RoleId == dto.RoleId);
        if (!exists)
        {
            Set.Add(new AppRoleGroupRole { RoleGroupId = dto.RoleGroupId, RoleId = dto.RoleId });
            await _context.SaveChangesAsync();
        }
        return Ok();
    }

    /// <summary>Rol grubundan rolü kaldırır.</summary>
    [HttpDelete]
    public async Task<ActionResult> Unassign([FromBody] AppRoleGroupRoleAssignDto dto)
    {
        var entity = await Set.FirstOrDefaultAsync(x => x.RoleGroupId == dto.RoleGroupId && x.RoleId == dto.RoleId);
        if (entity is null)
            return NotFound();

        Set.Remove(entity);
        await _context.SaveChangesAsync();
        return Ok();
    }
}
