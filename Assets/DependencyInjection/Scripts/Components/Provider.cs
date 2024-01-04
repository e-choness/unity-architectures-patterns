using DependencyInjection.Scripts.Attributes;
using DependencyInjection.Scripts.Interfaces;
using DependencyInjection.Scripts.Services;
using UnityEngine;

namespace DependencyInjection.Scripts.Components
{
    public class Provider : MonoBehaviour, IDependencyProvider
    {
        [Provide]
        public IServiceA ProvideServiceA()
        {
            return new ServiceA();
        }

        [Provide]
        public IServiceB ProvideServiceB()
        {
            return new ServiceB();
        }

        [Provide]
        public IServiceC ProvideServiceC()
        {
            return new ServiceC();
        }

        [Provide]
        public IFactoryA ProvideFactoryA()
        {
            return new FactoryA();
        }
    }
}