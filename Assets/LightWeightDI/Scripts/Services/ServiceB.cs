using UnityEngine;

namespace DependencyInjection
{
    public class ServiceB : IServiceB
    {
        public void Initialize(string message = null)
        {
            Debug.Log($"ServiceB.Initialize({message})");
        }
    }
}