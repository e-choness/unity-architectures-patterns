using DependencyInjection.Scripts.Interfaces;
using UnityEngine;

namespace DependencyInjection.Scripts.Services
{
    public class ServiceA : IServiceA
    {
        public void Initialize(string message = null)
        {
            Debug.Log($"ServiceA.Initialize({message})");
        }
    }
}