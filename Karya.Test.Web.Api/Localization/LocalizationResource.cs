namespace Karya.Test.Web.Api.Localization;

/// <summary>
/// Marks who a resource is for, so the client can pull only its own pack.
/// </summary>
public enum LocalizationScope
{
    Server = 0, // backend messages (errors/warnings) resolved at the API edge
    Client = 1, // frontend-only texts (labels, buttons, ...)
    Both   = 2  // shared by server and client
}

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

    /// <summary>Server / Client / Both - lets the client fetch only its own language pack.</summary>
    public LocalizationScope Scope { get; set; } = LocalizationScope.Server;
}
