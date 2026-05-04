namespace Karya.Core.Interfaces.Entities
{
    public interface IVersionable
    {
        string Version { get; set; }
        DateTimeOffset CreatedAt { get; set; }
        string? CreatedBy { get; set; }
        DateTimeOffset? UpdatedAt { get; set; }
        string? UpdatedBy { get; set; }
    }
}
