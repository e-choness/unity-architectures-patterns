using System;
using UnityEngine;
using UnityEngine.Events;

namespace ObserverPattern.Scripts
{
    [Serializable]
    public class Observer<T>
    {
        [SerializeField] private T value;
        [SerializeField] private UnityEvent<T> onValueChanged;

        public T Value
        {
            get => value;
            set => Set(value);
        }

        public static implicit operator T(Observer<T> observer) => observer.value;

        public Observer(T value, UnityAction<T> callback = null)
        {
            this.value = value;
            onValueChanged = new UnityEvent<T>();
            if(callback!=null) onValueChanged.AddListener(callback);
        }

        private void Set(T value)
        {
            if (Equals(this.value, value)) return;
            this.value = value;
            Invoke();
        }

        private void Invoke()
        {
            Debug.Log($"Invoking {onValueChanged.GetPersistentEventCount()} listeners.");
            onValueChanged?.Invoke(value);
        }

        public void AddListener(UnityAction<T> callback)
        {
            if (callback == null) return;
            if (onValueChanged == null) onValueChanged = new();
            
            onValueChanged?.AddListener(callback);
        }

        public void RemoveListener(UnityAction<T> callback)
        {
            if (callback == null) return;
            
            onValueChanged?.RemoveListener(callback);
        }

        public void RemoveAllListeners()
        {
            onValueChanged?.RemoveAllListeners();
        }
        
        public void Dispose()
        {
            RemoveAllListeners(); 
        }
    }
}
