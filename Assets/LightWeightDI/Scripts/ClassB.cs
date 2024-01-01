using UnityEngine;

namespace DependencyInjection
{
    public class ClassB : MonoBehaviour
    {
        [Inject] private ServiceA _serviceA;
        [Inject] private ServiceB _serviceB;

        private FactoryA _factoryA;

        [Inject]
        public void Init(FactoryA factoryA)
        {
            _factoryA = factoryA;
        }
    }
}