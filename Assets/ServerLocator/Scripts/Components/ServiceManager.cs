using System;
using System.Collections.Generic;
using UnityEngine;

namespace ServerLocator.Scripts.Components
{
    public class ServiceManager
    {
        private readonly Dictionary<Type, object> _services = new();
        public IEnumerable<object> RegisteredServices => _services.Values;

        #region Register Services

        public ServiceManager Register(Type type, object service)
        {
            if (!type.IsInstanceOfType(service))
            {
                throw new ArgumentException("Type of service does not match type of service interface.",
                    nameof(service));
            }

            if (!_services.TryAdd(type, service))
            {
                Debug.LogError($"ServiceManager.Register: Service of type {type.FullName} is already registered.");
            }
            
            return this;
        }

        public ServiceManager Register<T>(T service) where T : class
        {
            var type = typeof(T);

            if (!_services.TryAdd(type, service))
            {
                Debug.LogError($"ServiceManager.Register: Service of type {type.FullName} is already registered.");
            }
            
            return this;
        }

        #endregion

        #region Get Services

        public bool TryGet<T>(out T service) where T : class
        {
            var type = typeof(T);
            
            if (_services.TryGetValue(type, out var obj))
            {
                service = obj as T;
                return true;
            }

            service = null;
            return false;
        }

        public bool TryGet(Type type, object service)
        {
            if (!type.IsInstanceOfType(service))
            {
                throw new ArgumentException("Type of service does not match type of service interface.",
                    nameof(service));
            }

            if (!_services.ContainsKey(type))
            {
                Debug.LogError($"ServiceManager.Register: Service of type {type.FullName} is not registered.");
                return false;
            }
            
            return true;
        }

        public T Get<T>() where T : class
        {
            var type = typeof(T);

            if (_services.TryGetValue(type, out var service))
            {
                return service as T;
            }
            throw new ArgumentException(
                $"ServiceManager.Get: Service of type {type.FullName} does not match service interface.",
                nameof(service));
        }

        #endregion
        
    }
}