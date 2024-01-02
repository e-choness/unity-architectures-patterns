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
                Register(provider);
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
            
            // Inject into fields
            var injectableFields = type.GetFields(ProviderBindingFlags)
                .Where(member=>Attribute.IsDefined(member, typeof(InjectAttribute)));
            
            foreach (var injectableField in injectableFields)
            {
                if (injectableField.GetValue(instance) != null)
                {
                    Debug.LogWarning($"[Injector] Field '{injectableField.Name}' of class '{type.Name}' is already set.");
                    continue;
                }
                var fieldType = injectableField.FieldType;
                var resolvedInstance = Resolve(fieldType);
                if (resolvedInstance == null)
                {
                    throw new Exception($"Failed to inject field{fieldType.Name} into {type.Name}");
                }
                
                injectableField.SetValue(instance, resolvedInstance);
                Debug.Log($"Field injected {fieldType.Name} into {type.Name}");
            }

            // Inject into methods
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
            
            // Inject into properties
            var injectableProperties = type.GetProperties(ProviderBindingFlags)
                .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
            foreach (var injectableProperty in injectableProperties)
            {
                var propertyType = injectableProperty.PropertyType;
                var resolvedInstance = Resolve(propertyType);
                if (resolvedInstance == null)
                {
                    throw new Exception(
                        $"Failed to inject dependency into property '{injectableProperty.Name}' of class '{type.Name}'.");
                }
                
                injectableProperty.SetValue(instance, resolvedInstance);
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


        public void Register<T>(T provider)
        {
            _registry[typeof(T)] = provider;
        }
        
        private void Register(IDependencyProvider provider)
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

        public void ValidateDependencies()
        {
            var monoBehaviours = FindMonoBehaviours();
            var providers = monoBehaviours.OfType<IDependencyProvider>();
            var providedDependencies = GetProvidedDependencies(providers);

            var invalidDependencies = monoBehaviours
                .SelectMany(mb => mb.GetType().GetFields(ProviderBindingFlags), (mb, field) => new { mb, field })
                .Where(t => Attribute.IsDefined(t.field, typeof(InjectAttribute)))
                .Where(t => !providedDependencies.Contains(t.field.FieldType) && t.field.GetValue(t.mb) == null)
                .Select(t => $"[Validation] {t.mb.GetType().Name} is missing dependency {t.field.FieldType.Name} on GameObject {t.mb.gameObject.name}.");

            var invalidDependencyList = invalidDependencies.ToList();

            if (!invalidDependencyList.Any())
            {
                Debug.Log("[Validation] All dependencies are valid.");
            }
            else
            {
                Debug.LogError($"[Validation] {invalidDependencyList.Count} dependencies are invalid.");
                foreach (var invalidDependency in invalidDependencyList)
                {
                    Debug.LogError(invalidDependency);
                }
            }
        }

        HashSet<Type> GetProvidedDependencies(IEnumerable<IDependencyProvider> providers)
        {
            var providedDependencies = new HashSet<Type>();
            foreach (var provider in providers)
            {
                var methods = provider.GetType().GetMethods(ProviderBindingFlags);

                foreach (var method in methods)
                {
                    if (!Attribute.IsDefined(method, typeof(ProvideAttribute))) continue;

                    var returnType = method.ReturnType;
                    providedDependencies.Add(returnType);
                }
            }

            return providedDependencies;
        }

        public void ClearDependencies()
        {
            foreach (var monoBehaviour in FindMonoBehaviours())
            {
                var type = monoBehaviour.GetType();
                var injectableFields = type.GetFields(ProviderBindingFlags)
                    .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

                foreach (var injectableField in injectableFields)
                {
                    injectableField.SetValue(monoBehaviour, null);
                }
            }
            
            Debug.Log("[Injector] All dependencies cleared.");
        }
        
        static MonoBehaviour[] FindMonoBehaviours()
        {
            return FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.InstanceID);
        }
    }
}