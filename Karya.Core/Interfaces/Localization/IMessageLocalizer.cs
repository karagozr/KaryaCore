namespace Karya.Core.Interfaces.Localization;

/// <summary>
/// Resolves a message code into a localized text for the requested language.
/// The implementation owns the translation storage (e.g. database) so the
/// core layer stays persistence-agnostic.
/// </summary>
public interface IMessageLocalizer
{
    /// <summary>
    /// Returns the localized text for <paramref name="code"/> in <paramref name="languageId"/>.
    /// Falls back to the default language and finally to the code itself when no translation exists.
    /// <paramref name="args"/> are applied to placeholders ({0}, {1}, ...) in the text.
    /// </summary>
    string Get(string code, string languageId, params object[] args);
}
