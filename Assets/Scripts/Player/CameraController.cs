using UnityEngine;
using UnityEngine.EventSystems;
using UpsideDown.Environment;
using Grid = UpsideDown.Environment.Grid;

namespace UpsideDown.Player
{
    // CameraController is a script that controls the player movement
    // for their camera. It can move, rotate, and zoom the camera.
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float movementSpeed;
        [SerializeField] private float movementTime;
        [SerializeField] private float rotationAmount;
        [SerializeField] private Vector3 zoomAmount;
        [SerializeField] private Vector2 moveBounds;
        [SerializeField] private Vector2 zoomBounds;
        private Vector3 _newPosition;
        private Vector3 _newZoom;
        private Quaternion _newRotation;
        private InputActions _inputActions;

        private void Start()
        {
            _inputActions = PlayerInputManager.Instance.PlayerInputActions;
            _newPosition = transform.position;
            _newZoom = cameraTransform.localPosition;
            _newRotation = transform.rotation;
        }

        private void Update()
        {
            Movement();
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (!EventSystem.current.IsPointerOverGameObject() && hit.collider.CompareTag("Grid"))
                    {
                        Grid grid = hit.collider.transform.parent.gameObject.GetComponent<Grid>();
                        if (grid != null)
                        {
                            Debug.Log("Grid clicked.");
                            GridManager.Instance.selectedGrid = grid;
                        }
                    }
                }
            }
        }

        private void Movement()
        {
            // Camera movement
            Vector2 movementInput = _inputActions.Player.Move.ReadValue<Vector2>();
            if (movementInput.y > 0)
            {
                _newPosition += (transform.forward * movementSpeed);
            }
            if (movementInput.y < 0)
            {
                _newPosition += (transform.forward * -movementSpeed);
            }
            if (movementInput.x > 0)
            {
                _newPosition += (transform.right * movementSpeed);
            }
            if (movementInput.x < 0)
            {
                _newPosition += (transform.right * -movementSpeed);
            }

            // Camera rotation
            if (_inputActions.Player.RotateLeft.IsPressed())
            {
                _newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
            }
            if (_inputActions.Player.RotateRight.IsPressed())
            {
                _newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
            }
            
            // Camera zoom
            if (_inputActions.Player.Zoom.ReadValue<float>() > 0)
            {
                _newZoom += zoomAmount;
            }
            if (_inputActions.Player.Zoom.ReadValue<float>() < 0)
            {
                _newZoom -= zoomAmount;
            }

            // Clamp camera movement and zoom
            _newZoom.y = Mathf.Clamp(_newZoom.y, zoomBounds.x, zoomBounds.y);
            _newZoom.z = Mathf.Clamp(_newZoom.z, -zoomBounds.y, -zoomBounds.x);
            _newPosition.x = Mathf.Clamp(_newPosition.x, moveBounds.x, moveBounds.y);
            _newPosition.z = Mathf.Clamp(_newPosition.z, moveBounds.x, moveBounds.y);
            
            // Lerp the camera to the new position, rotation, and zoom
            transform.position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * movementTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, _newRotation, Time.deltaTime * movementTime);
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, _newZoom, Time.deltaTime * movementTime);
        }
    }
}