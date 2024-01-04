using DependencyInjection.Scripts.Attributes;
using DependencyInjection.Scripts.Interfaces;
using UnityEngine;

namespace DependencyInjection.Scripts.Components
{
    public class ClassA : MonoBehaviour
    {
        [Inject] private IServiceA _serviceA;
        private IServiceB _serviceB;
        [Inject]
        public void Init(IServiceB serviceB)
        {
            _serviceB = serviceB;
        }
        [Inject] public IServiceC ServiceC { get; private set; }
        
        [Inject] private IEnvironmentSystem _environmentSystem;
        
        private void Start()
        {
            _serviceA.Initialize("ServiceA initialized from ClassA.");
            _serviceB.Initialize("ServiceB initialized from ClassA.");
            ServiceC.Initialize("ServiceC initialized from ClassA");
            _environmentSystem.Initialize();
        }
    }
}