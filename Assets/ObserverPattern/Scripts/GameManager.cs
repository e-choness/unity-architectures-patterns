using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObserverPattern.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private List<Transform> interactables;

        public List<Transform> Interactables => interactables;

        private Camera _camera;

        public static Action<Transform> AddToInteractablesEvent;
        public static Action<Transform> RemoveFromInteractablesEvent;

        private void Awake()
        {
            AddToInteractablesEvent += AddToInteractables;
            RemoveFromInteractablesEvent += RemoveFromInteractables;
        }

        private void RemoveFromInteractables(Transform obj)
        {
            interactables.Add(obj);
        }

        private void AddToInteractables(Transform obj)
        {
            interactables.Remove(obj);
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void CollectibleToScreen()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).position = _camera.WorldToScreenPoint(transform.GetChild(i).position);

                transform.GetChild(i).localScale = Vector3.one * 100;
            }
        }
    }
}
