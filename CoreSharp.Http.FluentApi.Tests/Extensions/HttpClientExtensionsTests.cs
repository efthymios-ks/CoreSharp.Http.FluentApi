using CoreSharp.Http.FluentApi.Extensions;
using CoreSharp.Http.FluentApi.Steps;

namespace CoreSharp.Http.FluentApi.Tests.Extensions;

public sealed class HttpClientExtensionsTests
{
    // Methods
    [Fact]
    public void Request_WhenHttpClientIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        using HttpClient httpClient = null!;

        // Act
        void Action()
            => httpClient.Request();

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void Request_WhenCalled_ShouldReturnIRequest()
    {
        // Arrange
        using var httpClient = new HttpClient();

        // Act
        var request = httpClient.Request();

        // Assert
        Assert.NotNull(request);
        Assert.IsType<Request>(request);
    }
}
