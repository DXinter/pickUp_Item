using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CharacterController controller;

        [SerializeField] private float moveSpeed = 3f;

        [SerializeField] private float lookSensitivity = 2f;

        [SerializeField] private Joystick joystick;

        private Vector2 moveInput;
        private Vector2 lookInput;
        private float xRotation = 0f;

        private PlayerControls controls;

        private void Awake()
        {
            controls = new PlayerControls();

            controls.Gameplay.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            controls.Gameplay.Move.canceled += _ => moveInput = Vector2.zero;

            controls.Gameplay.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
            controls.Gameplay.Look.canceled += _ => lookInput = Vector2.zero;
        }

        private void OnEnable() => controls.Gameplay.Enable();
        private void OnDisable() => controls.Gameplay.Disable();

        private void Update()
        {
            moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);
            var move = transform.right * moveInput.x + transform.forward * moveInput.y;
            controller.Move(move * moveSpeed * Time.deltaTime);

            xRotation -= lookInput.y * lookSensitivity;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            transform.Rotate(Vector3.up * lookInput.x * lookSensitivity);
        }
    }
}