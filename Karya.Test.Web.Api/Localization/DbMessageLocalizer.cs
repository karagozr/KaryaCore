using Karya.Core.Interfaces.Localization;
using Karya.Test.Web.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Karya.Test.Web.Api.Localization;

/// <summary>
/// Database-backed implementation of <see cref="IMessageLocalizer"/>. Translations
/// for each language are loaded once and cached in memory; adding a language only
/// requires inserting rows in the LocalizationResources table.
/// </summary>
public class DbMessageLocalizer : IMessageLocalizer
{
    private const string DefaultLanguage = "en";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);

    private readonly AppDbContext _db;
    private readonly IMemoryCache _cache;

    public DbMessageLocalizer(AppDbContext db, IMemoryCache cache)
    {
        _db = db;
        _cache = cache;
    }

    public string Get(string code, string languageId, params object[] args)
    {
        if (string.IsNullOrWhiteSpace(code))
            return string.Empty;

        var language = string.IsNullOrWhiteSpace(languageId)
            ? DefaultLanguage
            : languageId.Trim().ToLowerInvariant();

        // requested language -> default language -> the code itself
        if (!GetLanguageMap(language).TryGetValue(code, out var text) &&
            !GetLanguageMap(DefaultLanguage).TryGetValue(code, out text))
        {
            text = code;
        }

        return (args is { Length: > 0 }) ? SafeFormat(text, args) : text;
    }

    private IReadOnlyDictionary<string, string> GetLanguageMap(string language)
    {
        return _cache.GetOrCreate($"localization::{language}", entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheDuration;
            try
            {
                return _db.LocalizationResources
                    .AsNoTracking()
                    .Where(r => r.LanguageCode == language)
                    .ToDictionary(r => r.Code, r => r.Value);
            }
            catch
            {
                // Table not migrated yet / DB unavailable: fall back gracefully.
                return new Dictionary<string, string>();
            }
        })!;
    }

    private static string SafeFormat(string text, object[] args)
    {
        try
        {
            return string.Format(text, args);
        }
        catch (FormatException)
        {
            return text;
        }
    }
}
