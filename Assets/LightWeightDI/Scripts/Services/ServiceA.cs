using UnityEngine;

namespace DependencyInjection
{
    public class ServiceA
    {
        public void Initialize(string message = null)
        {
            Debug.Log($"ServiceA.Initialize({message})");
        }
    }
}