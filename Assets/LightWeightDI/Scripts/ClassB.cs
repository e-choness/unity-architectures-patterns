using System;
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

        private void Start()
        {
            _serviceA.Initialize("ServiceA initialized from ClassB.");
            _serviceB.Initialize("ServiceB initialized from ClassB.");
        }
    }
}