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
    }
}