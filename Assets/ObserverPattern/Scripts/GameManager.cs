using System;
using System.Collections;
using System.Collections.Generic;
using FiniteStateMachine.Scripts;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Transform> _interactables;

    public List<Transform> Interactables
    {
        get => _interactables;
    }
    
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
        _interactables.Add(obj);
    }

    private void AddToInteractables(Transform obj)
    {
        _interactables.Remove(obj);
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
