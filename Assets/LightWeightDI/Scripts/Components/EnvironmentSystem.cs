using UnityEngine;

namespace DependencyInjection
{
    public class EnvironmentSystem : Singleton<EnvironmentSystem>, IDependencyProvider
    {
        [Provide]
        EnvironmentSystem ProvideEnvironmentSystem()
        {
            return this;
        }

        public void Initialize()
        {
            Debug.Log("EnvironmentSystem.Initialize()");
        }
    }
}