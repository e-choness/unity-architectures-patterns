using System;
using UnityEngine;

namespace ServerLocator.Scripts
{
    public class Service
    {
        
    }
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

        void Register(Type type, Service service)
        {
            
        }

        void Register<T>(Service service) where T : Type
        {
            
        }

        void Get<T>(out Service service) where T : Type
        {
            throw new NotImplementedException("Do something with service and output it.");
        }
    }
}