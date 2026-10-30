using UnityEngine;

public class PropController : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private ItemType itemType;
    [SerializeField] private string interactionText = "[ E ] Grab ...";

    public string InteractionText => interactionText;

    public void Interact()
    {
        bool pickedUp = playerInventory.PickUpItem(itemType);

        if (pickedUp)
        {
            Destroy(gameObject);
        }
    }
}
