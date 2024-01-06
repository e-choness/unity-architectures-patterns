using System;
using System.Collections.Generic;
using System.Linq;
using ClassExtensions.Scripts.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ServerLocator.Scripts.Components
{
    public class ServiceLocator : MonoBehaviour
    {
        // global and scene service locators act as singletons.
        private static ServiceLocator _serviceLocatorGlobal;
        private static Dictionary<Scene, ServiceLocator> _sceneContainers;
        private static List<GameObject> _rootGameObjects;
        
        private readonly ServiceManager _services = new();

        private const string GlobalName = "ServiceLocatorGlobal";
        private const string SceneName = "ServiceLocatorScene";

        #region Configurations
        internal void ConfigureAsGlobal(bool dontDestroyOnLoad)     
        {
            if (_serviceLocatorGlobal == this)
            {
                Debug.LogWarning("ServiceLocator.ConfigurationAsGlobal: global is already configured.", this);
            } else if (_serviceLocatorGlobal != this)
            {
                Debug.LogError("ServiceLocator.ConfigurationAsGlobal: other global service locator is configured.", this);
            }
            else
            {
                _serviceLocatorGlobal = this;
                if(dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
            }
        }
        internal void ConfigureForScene()
        {
            var scene = gameObject.scene;

            if (_sceneContainers.ContainsKey(scene))
            {
                Debug.LogError("ServiceLocator.ConfigurationAsScene: Another service locator is configured for the scene.", this);
                return;
            }
            _sceneContainers.Add(scene, this);
        }
        #endregion

        #region Service Locator Bootstrap
        public static ServiceLocator ServiceLocatorGlobal
        {
            get
            {
                if (_serviceLocatorGlobal != null) return _serviceLocatorGlobal;

                if (FindFirstObjectByType<GlobalBootstrapper>() is { } found)
                {
                    found.BootstrapOnDemand();
                    return _serviceLocatorGlobal;
                }

                var container = new GameObject(GlobalName, typeof(ServiceLocator));
                container.AddComponent<GlobalBootstrapper>().BootstrapOnDemand();

                return _serviceLocatorGlobal;
            }
        }
        
        public static ServiceLocator ForSceneOf(MonoBehaviour monoBehaviour)
        {
            var scene = monoBehaviour.gameObject.scene;

            // If service is registered in the scene container, return it
            if (_sceneContainers.TryGetValue(scene, out var container) && container != monoBehaviour)
            {
                return container;
            }
            
            // If none is found in the container, go through root game objects and Scene Bootstrapper to create container
            _rootGameObjects.Clear();
            scene.GetRootGameObjects(_rootGameObjects);
            foreach (var gameObj in _rootGameObjects.Where(gameObj => gameObj.GetComponent<SceneBootstrapper>() != null))
            {
                if (gameObj.TryGetComponent(out SceneBootstrapper bootstrapper) && bootstrapper.Container != monoBehaviour)
                {
                    bootstrapper.BootstrapOnDemand();
                    return bootstrapper.Container;
                }
            }

            // If two cases above failed to retrieve scene service locator, return the global one
            return _serviceLocatorGlobal;
        }

        public static ServiceLocator For(MonoBehaviour monoBehaviour)
        {
            return monoBehaviour.GetComponentInParent<ServiceLocator>().OrNull() ??
                   ForSceneOf(monoBehaviour) ?? 
                   _serviceLocatorGlobal;
        }
        
        #endregion

        #region Service Register
        public ServiceLocator Register(Type type, object service)
        {
            _services.Register(type, service);
            return this;
        }

        public ServiceLocator Register<T>(T service) where T : class
        {
            _services.Register(service);
            return this;
        }
        
        #endregion

        #region Get Service Locator
        
        public ServiceLocator Get<T>(out T service) where T : class
        {
            if (TryGetService(out service)) return this;
            if (TryGetInHierarchy(out ServiceLocator container))
            {
                container.Get(out service);
                return this;
            }

            throw new ArgumentException($"ServiceLocator.Get: Service of type {typeof(T).FullName} is not registered.");
        }
        
        bool TryGetService<T>(out T service) where T : class
        {
            return _services.TryGet(out service);
        }

        bool TryGetService<T>(Type type, object service) where T : class
        {
            return _services.TryGet(type, service);
        }

        bool TryGetInHierarchy(out ServiceLocator container)
        {
            if (this == _serviceLocatorGlobal)
            {
                container = null;
                return false;
            }

            container = transform.parent.OrNull()?
                .GetComponentInParent<ServiceLocator>()
                .OrNull() ?? ForSceneOf(this);
            return container != null;
        }
        
        #endregion

        #region Handle Static Objects

        private void OnDestroy()
        {
            if (this == _serviceLocatorGlobal)
            {
                _serviceLocatorGlobal = null;
            }else if (_sceneContainers.ContainsValue(this))
            {
                _sceneContainers.Remove(gameObject.scene);
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            _serviceLocatorGlobal = null;
            _sceneContainers = new();
            _rootGameObjects = new();
        }

        #endregion

        #region Service Locator Menu Tools

#if UNITY_EDITOR
        [MenuItem("GameObject/ServiceLocator/AddGlobalServiceLocator")]
        static void AddGlobal()
        {
            var gameObject = new GameObject(GlobalName, typeof(GlobalBootstrapper));
        }

        [MenuItem("GameObject/ServiceLocator/AddSceneServiceLocator")]
        static void AddScene()
        {
            var gameObject = new GameObject(SceneName, typeof(SceneBootstrapper));
        }

#endif
        
        #endregion
    }
}