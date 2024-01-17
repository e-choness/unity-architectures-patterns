using DependencyInjection.Scripts.Attributes;
using DependencyInjection.Scripts.Interfaces;
using Singleton.Scripts;
using UnityEngine;

namespace DependencyInjection.Scripts.Components
{
    public class EnvironmentSystem : Singleton<EnvironmentSystem>, IDependencyProvider, IEnvironmentSystem
    {
        [Provide]
        public IEnvironmentSystem ProvideEnvironmentSystem()
        {
            return this;
        }

        public void Initialize()
        {
            Debug.Log("EnvironmentSystem.Initialize()");
        }
    }
}