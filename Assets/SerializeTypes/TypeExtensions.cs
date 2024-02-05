using System;
using System.Linq;

public static class TypeExtensions
{
    public static Type ResolveGenericType(Type type)
    {
        if (type is not { IsGenericType: true }) return type;

        var genericType = type.GetGenericTypeDefinition();
        return genericType != type ? genericType : type;
    }

    private static bool HasAnyInterfaces(Type type, Type interfaceType)
    {
        return type.GetInterfaces().Any(i => ResolveGenericType(i) == interfaceType);
    }

    /// <summary>
    /// Checks if a given type inherits or implements a specific base type
    /// </summary>
    /// <param name="type">The type which needs to be checked.</param>
    /// <param name="baseType">The base type or interface expected to be inherited or implemented.</param>
    /// <returns>Return true if 'type' inherits or implements 'baseType'. False otherwise.</returns>
    public static bool InheritsOrImplements(this Type type, Type baseType)
    {
        type = ResolveGenericType(type);
        baseType = ResolveGenericType(baseType);

        while (type != typeof(object))
        {
            if (baseType == type || HasAnyInterfaces(type, baseType)) return true;

            type = ResolveGenericType(type.BaseType);

            if (type == null) return false;
        }

        return false;
    }
}
