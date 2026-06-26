using Microsoft.Data.SqlClient;
 
namespace Karya.Core.Helpers.Repository;

public sealed class SqlErrorHandlerHelper
{
    public static string GetUserFriendlyErrorMessage(SqlException? ex)
    {
        if (ex == null)
            return "Unknown database error.";

        switch (ex.Number)
        {
            case -1:
                return "Connection timeout. The server was not found or was not accessible.";

            case 2:
                return "A connection-level error occurred while opening a connection to the server.";

            case 53:
                return "An error occurred while establishing a connection to the server. Check your network configuration.";

            case 2601: // Unique Index Violation
                return "A record with the same unique value already exists in the system.";

            case 2627: // Primary Key / Unique Constraint Violation
                return "Duplicate entry detected. This primary or unique key already exists.";

            case 547: // Foreign Key / Check Constraint Violation
                return "Database operation failed due to a constraint violation. Please check related data dependencies or invalid inputs.";

            case 1205: // Deadlock
                return "The transaction was deadlocked on a resource with another process. Please retry your operation.";

            case 18456: // Login Failed
                return "Database login failed. Invalid username or password.";

            case 4060: // Cannot open database
                return "Cannot open the requested database. The database may be offline or the user does not have permission.";

            default:
                // Bilinmeyen veya nadir bir hata kodu geldiğinde genel mesaj dönülür
                return $"An unexpected database error occurred. (Error Code: {ex.Number})";
        }
    }
}
