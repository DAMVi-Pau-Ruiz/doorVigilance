using UnityEngine;

public class LanternController : MonoBehaviour, IEquipable, IUsable
{
    [SerializeField] private Light ilumination;
    [SerializeField] private Transform cameraTransform;

    private bool isActive;

    private void Start()
    {
        isActive = false;
        ilumination.enabled = false;
    }

    private void LateUpdate()
    {
        if (cameraTransform != null)
        {
            ilumination.transform.rotation = cameraTransform.rotation;
        }
    }

    public void ToggleLantern()
    {
        isActive = !isActive;
        ilumination.enabled = isActive;
    }

    public bool IsActive()
    {
        return isActive;
    }

    public void Equip(Transform playerCamera)
    {
        cameraTransform = playerCamera;
    }

    public void Use()
    {
        ToggleLantern();
    }
}