using Karya.Core.Interfaces.Entities;

namespace Karya.Core.Helpers.Repository;

public static class RepositoryLogHelper
{
   
    public static void AtCreate(IVersionable entity, string userId)
    {
        entity.CreatedAt = DateTimeOffset.UtcNow;
        entity.CreatedBy = userId;
        entity.Version = DateTimeOffset.UtcNow.ToString("yyyyMMddHHmmss");
    }

    public static void AtUpdate(IVersionable entity, string userId)
    {
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        entity.UpdatedBy = userId;
        entity.Version = DateTimeOffset.UtcNow.ToString("yyyyMMddHHmmss");
    }
    public static void AtDelete(ISoftDelete entity, string userId)
    {
        entity.IsDeleted = true;
        entity.DeletedBy = userId;
        entity.DeletedAt = DateTimeOffset.UtcNow;
    }

    public static void VersionControl(IVersionable? old, IVersionable? entity)
    {
        if (entity?.Version == null)
            return;
        
        if (old == null)
            throw new Exception("Entity not found. The entity may have been deleted by another process.");

        if (old.Version != entity.Version)
            throw new Exception("Version mismatch. The entity has been modified by another process.");
    }
}
