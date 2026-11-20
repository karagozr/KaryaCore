using Karya.Core.Identity.Domains.Entities;
using Karya.Core.Results;
using Microsoft.EntityFrameworkCore;

namespace Karya.Core.Identity.Application.Services
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
            await AssignResultAsync(userId, tenantId);
        }

        public async Task<BaseResult> AssignResultAsync(Guid userId, string tenantId, CancellationToken ct = default)
        {
            var set = _context.Set<AppUserTenant>();

            var userExists = await _context.Set<AppUser>().AnyAsync(x => x.Id == userId, ct);
            if (!userExists)
                return BaseResult.Error("404", "User not found.");

            var tenantIdNormalized = tenantId.Trim();
            var tenantExists = await _context.Set<AppTenant>()
                .AnyAsync(x => x.Id == tenantIdNormalized && x.IsActive, ct);
            if (!tenantExists)
                return BaseResult.Error("404", "Active tenant not found.");

            var exists = await set.AnyAsync(
                x => x.UserId == userId && x.TenantId == tenantIdNormalized,
                ct);

            if (exists)
                return BaseResult.Success("200", "Tenant is already assigned to the user.");

            await set.AddAsync(new AppUserTenant
            {
                UserId = userId,
                TenantId = tenantIdNormalized
            }, ct);

            await _context.SaveChangesAsync(ct);
            return BaseResult.Success("201", "Tenant assigned to the user.");
        }

        public async Task<BaseResult> RemoveAsync(Guid userId, string tenantId, CancellationToken ct = default)
        {
            var tenantIdNormalized = tenantId.Trim();
            var membership = await _context.Set<AppUserTenant>().FirstOrDefaultAsync(
                x => x.UserId == userId && x.TenantId == tenantIdNormalized,
                ct);

            if (membership is null)
                return BaseResult.Error("404", "User tenant membership not found.");

            var user = await _context.Set<AppUser>().FirstOrDefaultAsync(x => x.Id == userId, ct);
            if (user is not null && string.Equals(user.TenantId, tenantIdNormalized, StringComparison.Ordinal))
                return BaseResult.Error("409", "The user's primary tenant cannot be removed.");

            _context.Remove(membership);
            await _context.SaveChangesAsync(ct);
            return BaseResult.Success("200", "Tenant removed from the user.");
        }
    }
}
