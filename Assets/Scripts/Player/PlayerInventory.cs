using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class ItemPrefab
{
    public ItemType itemType;
    public GameObject prefab;
}

public class PlayerInventory : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference toggleLanternAction;

    [Header("Inventory")]
    [SerializeField] private ItemPrefab[] itemPrefabs;
    [SerializeField] private Transform handTransform;
    [SerializeField] private Transform playerCamera;

    private GameObject currentItem;
    private ItemType currentItemType = ItemType.NONE;

    private void OnEnable()
    {
        toggleLanternAction.action.Enable();
        toggleLanternAction.action.performed += OnUsable;
    }

    private void OnDisable()
    {
        toggleLanternAction.action.Disable();
        toggleLanternAction.action.performed -= OnUsable;
    }

    public bool HasItem()
    {
        return currentItem != null;
    }

    public bool PickUpItem(ItemType itemType)
    {
        if (HasItem())
        {
            return false;
        }

        foreach (ItemPrefab item in itemPrefabs)
        {
            if (item.itemType == itemType)
            {
                currentItem = Instantiate(item.prefab, handTransform);
                currentItemType = itemType;

                IEquipable equipable = currentItem.GetComponent<IEquipable>();

                if (equipable != null)
                {
                    equipable.Equip(playerCamera);
                }

                return true;
            }
        }
        return false;
    }

    private void OnUsable(InputAction.CallbackContext context)
    {
        if (currentItem == null)
        {
            return;
        }

        IUsable usable = currentItem.GetComponent<IUsable>();

        if (usable != null)
        {
            usable.Use();
        }
    }

    public ItemType GetCurrentItem()
    {
        return currentItemType;
    }

    public GameObject GettCurrentItemPrefab()
    {
        return currentItem;
    }

    public void RemoveCurrentItem()
    {
        if (currentItem != null)
        {
            Destroy(currentItem);

            currentItem = null;
            currentItemType = ItemType.NONE;
        }
    }
}
