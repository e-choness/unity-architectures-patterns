using UnityEngine;

namespace DependencyInjection
{
    public class ClassA : MonoBehaviour
    {
        private IServiceA _serviceA;

        [Inject] private IEnvironmentSystem _environmentSystem;
        
        [Inject]
        public void Init(IServiceA serviceA)
        {
            _serviceA = serviceA;
        }

        private void Start()
        {
            _serviceA.Initialize("ServiceA initialized from ClassA.");
            _environmentSystem.Initialize();
        }
    }
}