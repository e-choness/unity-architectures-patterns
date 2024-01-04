using DependencyInjection.Scripts.Interfaces;
using UnityEngine;

namespace DependencyInjection.Scripts.Services
{
    public class ServiceC : IServiceC
    {
        public void Initialize(string message = null)
        {
            Debug.Log($"ServiceC.Initialize{message}");
        }
    }
}