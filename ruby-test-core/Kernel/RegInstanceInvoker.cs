using AutoFixture.Kernel;
using System.Reflection;

namespace ruby_test_core.Kernel;

public class RegInstanceInvoker : ISpecimenBuilder
{
    public MethodInfo MethodInfo { get; }
    private readonly object[]? _methodParameters;

    public RegInstanceInvoker(MethodInfo methodInfo, object[]? methodParameters)
    {
        ArgumentNullException.ThrowIfNull(methodInfo, nameof(methodInfo));
        MethodInfo = methodInfo;
        _methodParameters = methodParameters;
    }

    public object Create(object request, ISpecimenContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        return MethodInfo.Invoke(null, _methodParameters)!;
    }
}
