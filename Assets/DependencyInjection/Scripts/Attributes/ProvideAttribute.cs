using System;

namespace DependencyInjection.Scripts.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ProvideAttribute : Attribute
    {
    }
}