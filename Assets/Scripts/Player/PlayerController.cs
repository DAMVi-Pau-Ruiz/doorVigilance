
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 2f;

    public Transform cameraTransform;

    private float rotationX = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Movimiento WASD
        Vector2 movementInput = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed)
                movementInput.y += 1;

            if (Keyboard.current.sKey.isPressed)
                movementInput.y -= 1;

            if (Keyboard.current.dKey.isPressed)
                movementInput.x += 1;

            if (Keyboard.current.aKey.isPressed)
                movementInput.x -= 1;
        }

        Vector3 movement = transform.right * movementInput.x
                         + transform.forward * movementInput.y;

        transform.position += movement.normalized * speed * Time.deltaTime;

        // Movimiento del ratón
        if (Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            float mouseX = mouseDelta.x * mouseSensitivity * 0.05f;
            float mouseY = mouseDelta.y * mouseSensitivity * 0.05f;

            // Rotación horizontal del jugador
            transform.Rotate(0f, mouseX, 0f);

            // Rotación vertical de la cámara
            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, -90f, 90f);

            cameraTransform.localRotation =
                Quaternion.Euler(rotationX, 0f, 0f);
        }
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
