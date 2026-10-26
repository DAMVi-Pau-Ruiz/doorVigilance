using UnityEngine;

public enum DOOR_STATES
{
    OPEN,
    CLOSED,
    OPENING,
    CLOSING
}

public class DoorController : MonoBehaviour
{
    [SerializeField] private Transform pivot;

    private void Update()
    {
        transform.RotateAround(pivot.position, pivot.up, 50 * Time.deltaTime);
    }
}
