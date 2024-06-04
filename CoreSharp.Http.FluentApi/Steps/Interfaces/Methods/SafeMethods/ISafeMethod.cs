namespace CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;

/// <summary>
/// An HTTP method is safe if it doesn't alter the state of the server.
/// HTTP methods that are safe: GET, HEAD, OPTIONS and TRACE.
/// </summary>
public interface ISafeMethod : IMethod
{
}
