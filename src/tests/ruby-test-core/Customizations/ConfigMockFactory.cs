using Microsoft.Extensions.Options;
using Moq;

namespace ruby_test_core.Customizations;

public static class ConfigMockFactory
{
    public static Mock<IOptions<T>> For<T>(T config) where T : class, new()
    {
        var configMock = new Mock<IOptions<T>>();
        configMock.Setup(x => x.Value).Returns(config);
        return configMock;
    }
}
