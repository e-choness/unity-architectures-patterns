using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ObserverPattern.Scripts
{
    public class Collectible : MonoBehaviour
    {
        [SerializeField] private int score = 10;
        private Camera _mainCamera;
        private InputAction _clickAction;
        private InputAction _cursorPositionAction;

        private Vector3 _cursorScreenPosition;
        
        private Vector3 _worldPosition
        {
            get
            {
                float z = _mainCamera.WorldToScreenPoint(transform.position).z;
                return _mainCamera.ScreenToWorldPoint(_cursorScreenPosition + new Vector3(0, 0, z));
            }
        }
        
        private bool _isClickedOn
        {
            get
            {
                Ray ray = _mainCamera.ScreenPointToRay(_cursorScreenPosition);
                Debug.Log($"Cursor screen position {_cursorScreenPosition.ToString()}");
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    return hit.transform == transform;
                }
                return false;
            }
        }
        
        private void Awake()
        {
            _mainCamera = Camera.main;
            var input = new ObserverInput();
            
            _clickAction = input.Mouse.Select;
            _cursorPositionAction = input.Mouse.Position;
        }

        void OnEnable()
        {
            _clickAction.Enable();
            _cursorPositionAction.Enable();
            Debug.Log("Click Action enabled.");
            _clickAction.performed += OnClick;
            _cursorPositionAction.performed += context =>
            {
                _cursorScreenPosition = context.ReadValue<Vector2>(); 
                Debug.Log($"Cursor position {_cursorScreenPosition.ToString()}");
            };
        }

        private void OnDisable()
        {
            _clickAction.performed -= OnClick;
            _clickAction.Disable();
            _cursorPositionAction.Disable();
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            Debug.Log("Clicked on the object");
            if (_isClickedOn)
            {
                Debug.Log($"Collective added.");
                gameObject.SetActive(false);
            }
            
        }
    }
}
