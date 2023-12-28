using CoreSharp.Http.FluentApi.Steps.Interfaces.Results;

namespace CoreSharp.Http.FluentApi.Steps.Interfaces.Methods.SafeMethods;

/// <inheritdoc cref="ISafeMethodWithResultFromXml{TResult}"/>
public interface ISafeMethodWithResultFromXmlAndCache<TResult> :
    ISafeMethodWithResultFromXml<TResult>,
    ICachedResult<ISafeMethodWithResultFromXmlAndCache<TResult>>
    where TResult : class
{
}
