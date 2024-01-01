namespace DependencyInjection
{
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