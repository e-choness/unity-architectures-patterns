using DependencyInjection.Scripts.Interfaces;

namespace DependencyInjection.Scripts.Services
{
    public class FactoryA : IFactoryA
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