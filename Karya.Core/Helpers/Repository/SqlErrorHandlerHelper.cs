using Microsoft.Data.SqlClient;
using Karya.Core.Results;

namespace Karya.Core.Helpers.Repository;

public sealed class SqlErrorHandlerHelper
{
    /// <summary>
    /// Maps a SQL error to a localizable message code. The code itself is the
    /// message (resolved per-language at the edge); no raw text is returned.
    /// </summary>
    public static string GetMessageCode(SqlException? ex)
    {
        if (ex == null)
            return MessageCodes.DbError;

        return ex.Number switch
        {
            -1 or 2 or 53 => MessageCodes.DbConnectionError,
            2601 or 2627  => MessageCodes.DbDuplicate,
            547           => MessageCodes.DbConstraint,
            1205          => MessageCodes.DbDeadlock,
            18456         => MessageCodes.DbLoginFailed,
            4060          => MessageCodes.DbCannotOpen,
            _             => MessageCodes.DbError,
        };
    }
}
