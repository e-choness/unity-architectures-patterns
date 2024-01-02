using UnityEngine;

namespace DependencyInjection
{
    public class ClassB : MonoBehaviour
    {
        [Inject] private IServiceA _serviceA;
        [Inject] private IServiceB _serviceB;

        private IFactoryA _factoryA;

        [Inject]
        public void Init(IFactoryA factoryA)
        {
            _factoryA = factoryA;
        }

        private void Start()
        {
            _serviceA.Initialize("ServiceA initialized from ClassB.");
            _serviceB.Initialize("ServiceB initialized from ClassB.");
            _factoryA.CreateServiceA().Initialize("ServiceA initialized from FactoryA in ClassB.");
        }
    }
}