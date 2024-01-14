using UnityEngine;
using UnityEngine.InputSystem;

namespace ObserverPattern.Scripts
{
    public class CameraMovement : MonoBehaviour
    {
        private InputController _inputActions;
        private InputAction _movement;
        private Transform _cameraTransform;

        [Header("Horizontal Translation")]
        [SerializeField]
        private float maxSpeed = 5f;
        private float speed;
        [Header("Horizontal Translation")]
        [SerializeField]
        private float acceleration = 10f;
        [Header("Horizontal Translation")]
        [SerializeField]
        private float damping = 15f;

        [Header("Vertical Translation")]
        [SerializeField]
        private float stepSize = 2f;
        [Header("Vertical Translation")]
        [SerializeField]
        private float zoomDampening = 7.5f;
        [Header("Vertical Translation")]
        [SerializeField]
        private float minHeight = 5f;
        [Header("Vertical Translation")]
        [SerializeField]
        private float maxHeight = 50f;
        [Header("Vertical Translation")]
        [SerializeField]
        private float zoomSpeed = 2f;

        [Header("Rotation")]
        [SerializeField]
        private float maxRotationSpeed = 1f;

        [Header("Edge Movement")]
        [SerializeField]
        [Range(0f,0.1f)]
        private float edgeTolerance = 0.05f;

        //value set in various functions 
        //used to update the position of the camera base object.
        private Vector3 _targetPosition;

        private float _zoomHeight;

        //used to track and maintain velocity w/o a rigidbody
        private Vector3 _horizontalVelocity;
        private Vector3 _lastPosition;

        //tracks where the dragging action started
        Vector3 _startDrag;

        private void Awake()
        {
            _inputActions = new InputController();
            _cameraTransform = GetComponentInChildren<Camera>().transform;
        }

        private void OnEnable()
        {
            _zoomHeight = _cameraTransform.localPosition.y;
            _cameraTransform.LookAt(this.transform);

            _lastPosition = this.transform.position;

            _movement = _inputActions.Camera.Movement;
            _inputActions.Camera.RotateCamera.performed += RotateCamera;
            _inputActions.Camera.ZoomCamera.performed += ZoomCamera;
            _inputActions.Camera.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Camera.RotateCamera.performed -= RotateCamera;
            _inputActions.Camera.ZoomCamera.performed -= ZoomCamera;
            _inputActions.Camera.Disable();
        }

        private void Update()
        {
            //inputs
            GetKeyboardMovement();
            CheckMouseAtScreenEdge();
            DragCamera();

            //move base and camera objects
            UpdateVelocity();
            UpdateBasePosition();
            UpdateCameraPosition();
        }

        private void UpdateVelocity()
        {
            _horizontalVelocity = (transform.position - _lastPosition) / Time.deltaTime;
            _horizontalVelocity.y = 0f;
            _lastPosition = transform.position;
        }

        private void GetKeyboardMovement()
        {
            Vector3 inputValue = _movement.ReadValue<Vector2>().x * GetCameraRight()
                                 + _movement.ReadValue<Vector2>().y * GetCameraForward();

            inputValue = inputValue.normalized;

            if (inputValue.sqrMagnitude > 0.1f)
                _targetPosition += inputValue;
        }

        private void DragCamera()
        {
            if (!Mouse.current.rightButton.isPressed)
                return;

            //create plane to raycast to
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main!.ScreenPointToRay(Mouse.current.position.ReadValue());
        
            if(plane.Raycast(ray, out float distance))
            {
                if (Mouse.current.rightButton.wasPressedThisFrame)
                    _startDrag = ray.GetPoint(distance);
                else
                    _targetPosition += _startDrag - ray.GetPoint(distance);
            }
        }

        private void CheckMouseAtScreenEdge()
        {
            //mouse position is in pixels
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector3 moveDirection = Vector3.zero;

            //horizontal scrolling
            if (mousePosition.x < edgeTolerance * Screen.width)
                moveDirection -= GetCameraRight();
            else if (mousePosition.x > (1f - edgeTolerance) * Screen.width)
                moveDirection += GetCameraRight();

            //vertical scrolling
            if (mousePosition.y < edgeTolerance * Screen.height)
                moveDirection -= GetCameraForward();
            else if (mousePosition.y > (1f - edgeTolerance) * Screen.height)
                moveDirection += GetCameraForward();

            _targetPosition += moveDirection;
        }

        private void UpdateBasePosition()
        {
            if (_targetPosition.sqrMagnitude > 0.1f)
            {
                //create a ramp up or acceleration
                speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime * acceleration);
                transform.position += _targetPosition * speed * Time.deltaTime;
            }
            else
            {
                //create smooth slow down
                _horizontalVelocity = Vector3.Lerp(_horizontalVelocity, Vector3.zero, Time.deltaTime * damping);
                transform.position += _horizontalVelocity * Time.deltaTime;
            }

            //reset for next frame
            _targetPosition = Vector3.zero;
        }

        private void ZoomCamera(InputAction.CallbackContext obj)
        {
            float inputValue = -obj.ReadValue<Vector2>().y / 100f;

            if (Mathf.Abs(inputValue) > 0.1f)
            {
                _zoomHeight = _cameraTransform.localPosition.y + inputValue * stepSize;

                if (_zoomHeight < minHeight)
                    _zoomHeight = minHeight;
                else if (_zoomHeight > maxHeight)
                    _zoomHeight = maxHeight;
            }
        }

        private void UpdateCameraPosition()
        {
            //set zoom target
            Vector3 zoomTarget = new Vector3(_cameraTransform.localPosition.x, _zoomHeight, _cameraTransform.localPosition.z);
            //add vector for forward/backward zoom
            zoomTarget -= zoomSpeed * (_zoomHeight - _cameraTransform.localPosition.y) * Vector3.forward;

            _cameraTransform.localPosition = Vector3.Lerp(_cameraTransform.localPosition, zoomTarget, Time.deltaTime * zoomDampening);
            _cameraTransform.LookAt(this.transform);
        }
     
        private void RotateCamera(InputAction.CallbackContext obj)
        {
            if (!Mouse.current.middleButton.isPressed)
                return;

            float inputValue = obj.ReadValue<Vector2>().x;
            transform.rotation = Quaternion.Euler(0f, inputValue * maxRotationSpeed + transform.rotation.eulerAngles.y, 0f);
        }

        //gets the horizontal forward vector of the camera
        private Vector3 GetCameraForward()
        {
            Vector3 forward = _cameraTransform.forward;
            forward.y = 0f;
            return forward;
        }

        //gets the horizontal right vector of the camera
        private Vector3 GetCameraRight()
        {
            Vector3 right = _cameraTransform.right;
            right.y = 0f;
            return right;
        }
        
    }
}