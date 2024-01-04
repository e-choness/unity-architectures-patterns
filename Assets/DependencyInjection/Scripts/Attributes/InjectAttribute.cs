using System;

namespace DependencyInjection.Scripts.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property)]
    public sealed class InjectAttribute : Attribute
    {
    }
}