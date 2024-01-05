using System;
using System.Net.NetworkInformation;

namespace ServerLocator.Scripts
{
    public class ServiceManager
    {
        void Register(Type type, Service service)
        {
            
        }

        void Register<T>(Service service) where T : Type
        {
            
        }

        void TryGet<T>(out Service service) where T : Type
        {
            throw new NotImplementedException("Do something to try to get service and output reference.");
        }

        Service Get<T>()
        {
            throw new NotImplementedException("Implement get service by type.");
        }
    }
}