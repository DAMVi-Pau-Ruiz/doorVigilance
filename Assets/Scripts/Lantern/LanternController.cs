using UnityEngine;

public class LanternController : MonoBehaviour
{
    [SerializeField] private Light ilumination;
    [SerializeField] private Transform cameraTransform;

    private bool isActive;

    private void Start()
    {
        SetActive(true);
        ToggleLantern();
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
        if (isActive)
        {
            ilumination.enabled = false;
            isActive = false;
        }
        else
        {
            ilumination.enabled = true;
            isActive = true;
        }
    }

    public void SetActive(bool value)
    {
        isActive = value;
    }

    public bool IsActive()
    {
        return isActive;
    }

    public void SetCamera(Transform camera)
    {
        cameraTransform = camera;
    }
}