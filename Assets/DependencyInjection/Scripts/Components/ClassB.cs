using DependencyInjection.Scripts.Attributes;
using DependencyInjection.Scripts.Interfaces;
using UnityEngine;

namespace DependencyInjection.Scripts.Components
{
    public class ClassB : MonoBehaviour
    {
        [Inject] private IServiceA _serviceA;
        private IServiceB _serviceB;

        private IFactoryA _factoryA;

        [Inject]
        public void Init(IServiceB serviceB, IFactoryA factoryA)
        {
            _serviceB = serviceB;
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