namespace Karya.Core.App.Interfaces.Profiles;

public interface IUserProfileSection
{
    Guid UserId { get; set; }
    string TenantId { get; set; }
    string? Title { get; set; }
    string? Department { get; set; }
    string? Description { get; set; }
    string? BaseTheme { get; set; }
}