using UnityEngine;

namespace DependencyInjection
{
    public class ServiceB
    {
        public void Initialize(string message = null)
        {
            Debug.Log($"ServiceB.Initialize({message})");
        }
    }
}