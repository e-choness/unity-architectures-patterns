using UnityEngine;

namespace DependencyInjection
{
    public class Provider : MonoBehaviour, IDependencyProvider
    {
        [Provide]
        public ServiceA ProvideServiceA()
        {
            return new ServiceA();
        }

        [Provide]
        public ServiceB ProvideServiceB()
        {
            return new ServiceB();
        }

        [Provide]
        public FactoryA ProvideFactoryA()
        {
            return new FactoryA();
        }
    }

    public class ServiceA
    {
        public void Initialize(string message = null)
        {
            Debug.Log($"ServiceA.Initialize({message})");
        }
    }

    public class ServiceB
    {
        public void Initialize(string message = null)
        {
            Debug.Log($"ServiceB.Initialize({message})");
        }
    }

    public class FactoryA
    {
        private ServiceA _cachedServiceA;
        public ServiceA CreateServiceA()
        {
            if (_cachedServiceA == null)
            {
                _cachedServiceA = new();
            }

            return _cachedServiceA;
        }
    }
}