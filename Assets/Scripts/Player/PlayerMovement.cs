using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference lookAction;

    [Header("Camera")]
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private float mouseSensitivity = 2;

    [Header("Movement")]
    [SerializeField] private float speed = 5;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 vecrticalVelocity;
    private float cameraRotationX;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        lookAction.action.Disable();
    }

    private void Update()
    {
        Move();
        Look();
    }

    private void Move()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();

        Vector3 movement = transform.right * input.x + transform.forward * input.y;

        if (controller.isGrounded && vecrticalVelocity.y < 0)
        {
            vecrticalVelocity.y = -2f;
        }

        vecrticalVelocity.y += gravity * Time.deltaTime;

        Vector3 finalMovement = movement * speed + vecrticalVelocity;

        controller.Move(finalMovement * Time.deltaTime);
    }

    private void Look()
    {
        Vector2 mouseInput = lookAction.action.ReadValue<Vector2>();

        float mouseX = mouseInput.x * mouseSensitivity * 0.05f;
        float mouseY = mouseInput.y * mouseSensitivity * 0.05f;

        transform.Rotate(Vector3.up * mouseX);

        cameraRotationX -= mouseY;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -90, 90);

        cameraHolder.localRotation = Quaternion.Euler(cameraRotationX, 0, 0);
    }
}
