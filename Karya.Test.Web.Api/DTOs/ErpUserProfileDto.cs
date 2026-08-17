using Karya.Core.App.Interfaces.Profiles;

namespace Karya.Test.Web.Api.DTOs
{
    public class ErpUserProfileDto : IUserProfileSection
    {
        public Guid UserId { get; set; }
        public string TenantId { get; set; } = null!;
        public string? Title { get; set; }
        public string? Department { get; set; }
        public string? Description { get; set; }
        public string? BaseTheme { get; set; }
    }
}
