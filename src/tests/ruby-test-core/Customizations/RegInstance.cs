using AutoFixture;
using AutoFixture.Kernel;
using ruby_test_core.Kernel;
using System.Reflection;

namespace ruby_test_core.Customizations;

public class RegInstance : ICustomization
{
    private readonly MethodInfo _methodInfo;
    private readonly object[]? _methodParameters;
    private readonly Type _expectedType;

    public RegInstance(MethodInfo methodInfo, object[]? parameters, Type expectedType)
    {
        ArgumentNullException.ThrowIfNull(methodInfo, nameof(methodInfo));
        ArgumentNullException.ThrowIfNull(expectedType, nameof(expectedType));

        _methodInfo = methodInfo;
        _methodParameters = parameters;
        _expectedType = expectedType;
    }

    public void Customize(IFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture, nameof(fixture));

        var factory = new RegInstanceInvoker(_methodInfo, _methodParameters);
        var builder = SpecimenBuilderNodeFactory.CreateTypedNode(_expectedType, factory);

        fixture.Customizations.Add(builder);
    }
}

