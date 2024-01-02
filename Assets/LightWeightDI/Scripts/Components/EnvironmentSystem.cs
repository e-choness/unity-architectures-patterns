using UnityEngine;

namespace DependencyInjection
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