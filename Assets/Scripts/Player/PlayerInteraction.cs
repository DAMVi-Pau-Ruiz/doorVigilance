using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference interactAction;

    [Header("Raycast")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactDistance = 3;

    [Header("UI")]
    [SerializeField] private PlayerUI playerUI;

    private IInteractable currentInteractable;
    private string currentText;

    private void OnEnable()
    {
        interactAction.action.Enable();
        interactAction.action.performed += OnInteract;
    }

    private void OnDisable()
    {
        interactAction.action.performed -= OnInteract;
        interactAction.action.Disable();
    }

    private void Update()
    {
        DetectInteractable();
        UpdateInteractionUI();
    }

    private void DetectInteractable()
    {
        currentInteractable = null;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            currentInteractable = hit.collider.GetComponentInParent<IInteractable>();
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("Has pulsado E");

        if (currentInteractable != null)
        {
            Debug.Log("Objeto detectado: " + currentInteractable);
            currentInteractable.Interact();
        }

    }

    private void UpdateInteractionUI()
    {
        if (currentInteractable == null)
        {
            currentText = "";
            playerUI.HideInteractionUI();
            return;
        }

        string newText = currentInteractable.InteractionText;

        if (string.IsNullOrEmpty(newText))
        {
            currentText = "";
            playerUI.HideInteractionUI();
            return;
        }

        if (currentText != newText)
        {
            currentText = newText;
            playerUI.SetText(currentText);
        }

        playerUI.ShowInteractionUI();
    }
}
