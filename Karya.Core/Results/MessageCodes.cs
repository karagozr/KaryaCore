namespace Karya.Core.Results;

/// <summary>
/// Semantic message keys (the "pattern" each message belongs to). These are
/// decoupled from the HTTP status <see cref="BaseResult.Code"/>. Each code maps
/// to a per-language text in the localization store. Adding a new language means
/// only adding the texts for these codes - no code change required.
/// </summary>
public static class MessageCodes
{
    public const string Success = "SUCCESS";
    public const string Created = "CREATED";
    public const string Updated = "UPDATED";
    public const string Deleted = "DELETED";
    public const string NotFound = "NOT_FOUND";
    public const string Required = "REQUIRED";
    public const string ValidationError = "VALIDATION_ERROR";
    public const string Unauthorized = "UNAUTHORIZED";
    public const string ServerError = "SERVER_ERROR";

    // Database errors (mapped from SQL error numbers). Each code is a complete,
    // self-contained message - no extra text is passed alongside it.
    public const string DbError = "DB_ERROR";
    public const string DbConnectionError = "DB_CONNECTION_ERROR";
    public const string DbDuplicate = "DB_DUPLICATE";
    public const string DbConstraint = "DB_CONSTRAINT";
    public const string DbDeadlock = "DB_DEADLOCK";
    public const string DbLoginFailed = "DB_LOGIN_FAILED";
    public const string DbCannotOpen = "DB_CANNOT_OPEN";

    public static readonly IReadOnlyDictionary<string, string> English = new Dictionary<string, string>
    {
        [Success] = "Operation completed successfully.",
        [Created] = "Record created successfully.",
        [Updated] = "Record updated successfully.",
        [Deleted] = "Record deleted successfully.",
        [NotFound] = "Value {1} = {2} could not be found in [{0}].",
        [Required] = "[{0}] is required.",
        [ValidationError] = "Validation error.",
        [Unauthorized] = "You are not authorized for this operation.",
        [ServerError] = "An unexpected server error occurred.",
        [DbError] = "A database error occurred.",
        [DbConnectionError] = "Could not connect to the database.",
        [DbDuplicate] = "A record with the same value already exists.",
        [DbConstraint] = "The operation could not be completed due to related data.",
        [DbDeadlock] = "The operation was deadlocked, please try again.",
        [DbLoginFailed] = "Database login failed.",
        [DbCannotOpen] = "The database could not be opened."
    };

    /// <summary>
    /// Resolves a message code to a text using the built-in (English) catalog and
    /// applies the given <paramref name="args"/> to its placeholders. Used as a
    /// fallback when no <c>IMessageLocalizer</c> is registered so responses never
    /// come back with an empty message.
    /// </summary>
    public static string Resolve(string code, params object[]? args)
    {
        if (string.IsNullOrEmpty(code))
            return string.Empty;

        if (!English.TryGetValue(code, out var text))
            return code;

        if (args is null || args.Length == 0)
            return text;

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
