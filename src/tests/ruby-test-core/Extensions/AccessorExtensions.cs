using System.Reflection;

namespace ruby_test_core.Extensions;

public static class AccessorExtensions
{
    public static MethodInfo? GetMethodInfo(this Type type, string methodName)
    {
        MethodInfo? method = null;
        for (var reflectionType = type; reflectionType != null; reflectionType = reflectionType.BaseType)
        {
            method = reflectionType.GetRuntimeMethod(methodName, Array.Empty<Type>());
            if (method != null)
                break;
        }
        return method;
    }
    public static MethodInfo? GetMethodInfo(this Type type, string methodName, object[] parameters, out Type returnType)
    {
        MethodInfo? method = GetMethodInfoByTestClassName(type, methodName, parameters);
        returnType = method!.ReturnType;
        return method;
    }

    public static MethodInfo? GetMethodInfo(this Type type, string methodName, Type[] parametersType)
    {
        MethodInfo? method = null;

        for (var reflectionType = type; reflectionType != null; reflectionType = reflectionType.BaseType)
        {
            method = reflectionType.GetRuntimeMethod(methodName, parametersType);
            if (method != null)
                break;
        }

        return method;
    }

    private static MethodInfo? GetMethodInfoByTestClassName(Type type, string methodName, object[] parameters)
    {
        MethodInfo? method = null;
        var parameterTypes = parameters == null || parameters.Length == 0 ? Array.Empty<Type>() : parameters.Select(p => p?.GetType()).ToArray();
        for (var reflectionType = type; reflectionType != null; reflectionType = reflectionType.BaseType)
        {
            method = reflectionType.GetRuntimeMethod(methodName, parameterTypes!);
            if (method != null)
                break;
        }

        return method;
    }

    public static Type[] GetParametersType(object[]? objects)
    {
        if (objects == null || objects.Length == 0)
            return Array.Empty<Type>();

        return objects.Select(p => p.GetType()).ToArray();
    }
}