using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Karya.Core.Results;

public record ServiceMessages
{
    public static string Required(string filedName) => $"[{filedName}] is required.";
    public static string NotFound(string entityName, string fields, string values) => $"{fields} {values} value could not be found in the [{entityName}].";
    public static string Created(string entityName) =>  $"Entity [{entityName}] has been created.";
    public static string Error(string entityName, string? message) => $"ERROR for [{entityName}]  : {message}";
    public static string Updated(string entityName) => $"Entity [{entityName}] has been updated.";
    public static string Deleted(string entityName) => $"Entity [{entityName}] has been deleted.";

}
