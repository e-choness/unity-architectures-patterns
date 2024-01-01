using UnityEngine;

namespace DependencyInjection
{
    public class ClassA : MonoBehaviour
    {
        private ServiceA _serviceA;

        [Inject]
        public void Init(ServiceA serviceA)
        {
            _serviceA = serviceA;
        }

        private void Start()
        {
            _serviceA.Initialize("ServiceA initialized from ClassA.");
        }
    }
}