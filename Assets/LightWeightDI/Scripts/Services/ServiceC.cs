using UnityEngine;

namespace DependencyInjection
{
    public class ServiceC : IServiceC
    {
        public void Initialize(string message = null)
        {
            Debug.Log($"ServiceC.Initialize{message}");
        }
    }
}