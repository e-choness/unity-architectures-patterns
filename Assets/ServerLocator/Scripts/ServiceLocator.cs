using System;
using UnityEngine;

namespace ServerLocator.Scripts
{
    public class ServiceLocator
    {
        void Global(){}

        void ForSceneOf(MonoBehaviour monoBehaviour)
        {
        }

        void For(MonoBehaviour monoBehaviour)
        {
        }

        internal void ConfigureAsGlobal()
        {
        }

        internal void ConfigureForScene()
        {
            
        }

        void Register(Type type, object service)
        {
            
        }

        void Register<T>(T service) where T : class
        {
            
        }

        void Get<T>(out T service) where T : class
        {
            throw new NotImplementedException("Do something with service and output it.");
        }
    }
}