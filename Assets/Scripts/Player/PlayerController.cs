using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CharacterController controller;

        [SerializeField] private float moveSpeed = 3f;

        [SerializeField] private float lookSensitivity = 2f;

        [SerializeField] private Joystick joystick;

        [SerializeField] private Transform cameraTransform;

        private Vector2 _moveInput;
        private Vector2 _lookInput;
        private float _xRotation;
        private PlayerControls _controls;
        
        private void Awake()
        {
            _controls = new PlayerControls();

            _controls.Gameplay.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
            _controls.Gameplay.Move.canceled += _ => _moveInput = Vector2.zero;

            _controls.Gameplay.Look.performed += ctx => _lookInput = ctx.ReadValue<Vector2>();
            _controls.Gameplay.Look.canceled += _ => _lookInput = Vector2.zero;
        }

        private void OnEnable() => _controls.Gameplay.Enable();
        private void OnDisable() => _controls.Gameplay.Disable();

        private void Update()
        {
            _moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);
            var move = transform.right * _moveInput.x + transform.forward * _moveInput.y;
            controller.Move(move * moveSpeed * Time.deltaTime);

            transform.Rotate(Vector3.up * _lookInput.x * lookSensitivity);

            _xRotation -= _lookInput.y * lookSensitivity;
            _xRotation = Mathf.Clamp(_xRotation, -80f, 80f);
            cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        }
    }
}