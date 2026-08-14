using Karya.Core.Indentity.Domains.Entities;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Indentity.Services
{
    public class AppUserTenantService
    {
        private readonly DbContext _context;

        public AppUserTenantService(DbContext context)
        {
            _context = context;
        }

        public async Task AssignAsync(Guid userId, string tenantId)
        {
            var set = _context.Set<AppUserTenant>();

            var exists = await set.AnyAsync(x => x.UserId == userId && x.TenantId == tenantId);

            if (exists)
                return;

            await set.AddAsync(new AppUserTenant
            {
                UserId = userId,
                TenantId = tenantId
            });

            await _context.SaveChangesAsync();
        }
    }
}
