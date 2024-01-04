using DependencyInjection.Scripts.Interfaces;
using UnityEngine;

namespace DependencyInjection.Scripts.Services
{
    public class ServiceB : IServiceB
    {
        public void Initialize(string message = null)
        {
            Debug.Log($"ServiceB.Initialize({message})");
        }
    }
}