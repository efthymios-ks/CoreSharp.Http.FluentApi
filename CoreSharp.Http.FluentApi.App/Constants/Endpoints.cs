using System.Diagnostics.CodeAnalysis;

namespace App.Constants;

public static class Endpoints
{
    // Fields  
    [SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "<Pending>")]
    public const string EndpointUrl = "https://jsonplaceholder.typicode.com/";
}
