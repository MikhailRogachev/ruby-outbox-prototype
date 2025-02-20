using AutoFixture;
using AutoFixture.Xunit2;
using ruby_test_core.Customizations;
using ruby_test_core.Extensions;
using System.Reflection;

namespace ruby_test_core.Attributes;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
public class RegInstanceAttribute : CustomizeAttribute
{
    private readonly string _methodName;
    private readonly object[]? _methodParameters;

    public RegInstanceAttribute(string methodName)
    {
        ArgumentNullException.ThrowIfNull(methodName, nameof(methodName));
        _methodName = methodName;
    }

    public RegInstanceAttribute(string methodInfo, object[]? values)
    {
        _methodName = methodInfo;
        _methodParameters = values;
    }

    public override ICustomization GetCustomization(ParameterInfo parameter)
    {
        var cls = parameter.Member.DeclaringType!;
        var types = AccessorExtensions.GetParametersType(_methodParameters);
        var methodInfo = cls.GetMethodInfo(_methodName, types)!;

        return new RegInstance(methodInfo, _methodParameters, methodInfo.ReturnType);
    }
}
