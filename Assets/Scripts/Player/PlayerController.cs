
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerUI playerUI;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private GameObject lanternPrefab;
    [SerializeField] private Transform leftHandTransform;
    [SerializeField] private Transform rightHandTransform;
    [SerializeField] private bool isLeftHandler = false;

    private CharacterController characterController;
    private float rotationX = 0f;
    private bool hasLantern = false;
    private LanternController currentLantern;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
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

        characterController.Move(movement.normalized * speed * Time.deltaTime);

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

        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            DoorController door = hit.collider.GetComponentInParent<DoorController>();

            if (door != null)
            {
                playerUI.ShowInteractionUI();

                switch (door.GetCurrentState())
                {
                    case DOOR_STATES.OPEN:
                        playerUI.SetText("[ e ] close door");
                        break;

                    case DOOR_STATES.CLOSED:
                        playerUI.SetText("[ e ] open door");
                        break;
                }
            }
            else if (hit.collider.CompareTag("Lantern") && !hasLantern)
            {
                playerUI.ShowInteractionUI();
                playerUI.SetText("[ e ] take lantern");
                if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
                {
                    PutLantern();
                    Destroy(hit.collider.gameObject);
                }
            }
            else
            {
                playerUI.HideInteractionUI();
            }
        }
        else
        {
            playerUI.HideInteractionUI();
        }

        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("Has pulsado E");

            if (Physics.Raycast(ray, out hit, interactDistance))
            {
                Debug.Log("Raycast detectado: " + hit.collider.name);

                DoorController door = hit.collider.GetComponentInParent<DoorController>();

                if (door != null)
                {
                    Debug.Log("DoorController encontrado");
                    door.ToggleDoor();
                }
                else
                {
                    Debug.Log("No se ha encontrado DoorController");
                }
            }
            else
            {
                Debug.Log("El Raycast no detecta nada");
            }
        }

        if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
        {
            if (hasLantern && currentLantern != null)
            {
                currentLantern.ToggleLantern();
                Debug.Log("Linterna interaccionada");
            }
            else
            {
                Debug.Log("No tiene la linterna en las manos");
            }
        }

    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PutLantern()
    {
        Transform currentHand = isLeftHandler ? leftHandTransform : rightHandTransform;

        GameObject lantern = Instantiate(lanternPrefab, currentHand);
        
        currentLantern = lantern.GetComponent<LanternController>();
        currentLantern.Equip(cameraTransform);
        hasLantern = true;
    }
}
