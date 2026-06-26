namespace Karya.Test.Web.Api.Localization;

/// <summary>
/// A single translation row: the text for a message <see cref="Code"/> in a
/// given <see cref="LanguageCode"/>. Adding a new language = inserting rows with
/// the same codes and a new language code (no code change).
/// </summary>
public class LocalizationResource
{
    public int Id { get; set; }

    /// <summary>Message key, e.g. "NOT_FOUND" (see Karya.Core MessageCodes).</summary>
    public string Code { get; set; } = default!;

    /// <summary>Language code, e.g. "tr" or "en".</summary>
    public string LanguageCode { get; set; } = default!;

    /// <summary>Localized text, may contain {0}, {1}, ... placeholders.</summary>
    public string Value { get; set; } = default!;
}
