using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace DependencyInjection
{
    public class Injector : Singleton<Injector>
    {
        private const BindingFlags ProviderBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        private readonly Dictionary<Type, object> _registry = new();

        protected override void Awake()
        {
            base.Awake();
            
            // Find all modules implementing IDependencyProvider
            var providers = FindMonoBehaviours().OfType<IDependencyProvider>();
            foreach (var provider in providers)
            {
                RegisterProvider(provider);
            }
            
            // Find all injectable objects and inject their dependencies
            var injectables = FindMonoBehaviours().Where(IsInjectable);
            foreach (var injectable in injectables)
            {
                Inject(injectable);
            }
        }

        private void Inject(object instance)
        {
            var type = instance.GetType();
            var injectableFields = type.GetFields(ProviderBindingFlags)
                .Where(member=>Attribute.IsDefined(member, typeof(InjectAttribute)));

            foreach (var injectableField in injectableFields)
            {
                var fieldType = injectableField.FieldType;
                var resolvedInstance = Resolve(fieldType);
                if (resolvedInstance == null)
                {
                    throw new Exception($"Failed to inject field{fieldType.Name} into {type.Name}");
                }
                
                injectableField.SetValue(instance, resolvedInstance);
                Debug.Log($"Field injected {fieldType.Name} into {type.Name}");
            }

            var injectableMethods = type.GetMethods(ProviderBindingFlags)
                .Where(memeber => Attribute.IsDefined(memeber, typeof(InjectAttribute)));

            foreach (var injectableMethod in injectableMethods)
            {
                var requiredParameters = injectableMethod.GetParameters()
                    .Select(parameter => parameter.ParameterType)
                    .ToArray();
                var resolvedInstances = requiredParameters.Select(Resolve).ToArray();
                if (resolvedInstances.Any(resolvedInstances => resolvedInstances == null))
                {
                    throw new Exception($"Failed to inject method {type.Name}.{injectableMethod.Name}");
                }

                injectableMethod.Invoke(instance, resolvedInstances);
                Debug.Log($"Method injected {type.Name}.{injectableMethod.Name}");
            }
        }

        private object Resolve(Type type)
        {
            _registry.TryGetValue(type, out var resolvedInstance);
            return resolvedInstance;
        }
        
        // Find injectable fields inside a MonoBehaviour instance
        static bool IsInjectable(MonoBehaviour obj){
            var members = obj.GetType().GetMethods(ProviderBindingFlags);
            return members.Any(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
        }


        private void RegisterProvider(IDependencyProvider provider)
        {
            var methods = provider.GetType().GetMethods(ProviderBindingFlags);

            foreach (var method in methods)
            {
                if(!Attribute.IsDefined(method, typeof(ProvideAttribute))) continue;

                var returnType = method.ReturnType;
                var providerInstance = method.Invoke(provider, null);
                if (providerInstance != null)
                {
                    _registry.Add(returnType, providerInstance);
                    Debug.Log($"Register {returnType.Name} from {provider.GetType().Name}");
                }
                else
                {
                    throw new Exception($"Provider {provider.GetType().Name} return null for {returnType.Name}");
                }
            }
        }
        
        static MonoBehaviour[] FindMonoBehaviours()
        {
            return FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.InstanceID);
        }
    }
}