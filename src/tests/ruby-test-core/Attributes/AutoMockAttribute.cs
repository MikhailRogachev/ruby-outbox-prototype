using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace ruby_test_core.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AutoMockAttribute : AutoDataAttribute
{
    public AutoMockAttribute() : base(() => new Fixture().Customize(new AutoMoqCustomization()))
    {
    }
}
