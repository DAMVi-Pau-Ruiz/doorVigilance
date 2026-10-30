using UnityEngine;

public class TrashcanController : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private string interactionText = "[ E ] Throw trash";
    
    public string InteractionText
    {
        get
        {
            if (playerInventory.GetCurrentItem() == ItemType.TRASH)
            {
                return interactionText;
            }

            return "";
        }
    }

    public void Interact()
    {
        if (playerInventory.GetCurrentItem() == ItemType.TRASH)
        {
            playerInventory.RemoveCurrentItem();
        }
    }
}
