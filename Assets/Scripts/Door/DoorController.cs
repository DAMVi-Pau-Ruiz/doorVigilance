using UnityEditor.Rendering.LookDev;
using UnityEngine;

public enum DOOR_STATES
{
    OPEN,
    CLOSED,
    OPENING,
    CLOSING
}

public class DoorController : MonoBehaviour, IInteractable
{
    [Header("Door")]
    [SerializeField] private Transform pivot;
    [SerializeField] private DOOR_STATES doorState;
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float rotationSpeed = 90f;

    [Header("UI")]
    [SerializeField] private string openText = "[ E ] Open door";
    [SerializeField] private string closeText = "[ E ] Close door";

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private float currentStateTime;

    void Start()
    {
        closedRotation = pivot.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
    }

    void Update()
    {
        UpdateState();
    }

    void ChangeState(DOOR_STATES newState)
    {
        ExitState();
        doorState = newState;
        EnterState(newState);
    }

    void EnterState(DOOR_STATES newState)
    {
        currentStateTime = 0;

        switch (newState)
        {
            case DOOR_STATES.OPEN:
                pivot.localRotation = openRotation;
                break;

            case DOOR_STATES.CLOSED:
                pivot.localRotation = closedRotation;
                break;

            case DOOR_STATES.OPENING:
                break;

            case DOOR_STATES.CLOSING:
                break;
        }
    }

    void UpdateState()
    {
        currentStateTime += Time.deltaTime;

        switch (doorState)
        {
            case DOOR_STATES.OPEN:
                break;

            case DOOR_STATES.CLOSED:
                break;

            case DOOR_STATES.OPENING:
                pivot.localRotation = Quaternion.RotateTowards(pivot.localRotation, openRotation, rotationSpeed * Time.deltaTime);

                if (pivot.localRotation == openRotation)
                {
                    ChangeState(DOOR_STATES.OPEN);
                }
                break;

            case DOOR_STATES.CLOSING:
                pivot.localRotation = Quaternion.RotateTowards(pivot.localRotation, closedRotation, rotationSpeed * Time.deltaTime);

                if (pivot.localRotation == closedRotation)
                {
                    ChangeState(DOOR_STATES.CLOSED);
                }
                break;
        }
    }

    void ExitState()
    {
        switch (doorState)
        {
            case DOOR_STATES.OPEN:
                break;

            case DOOR_STATES.CLOSED:
                break;

            case DOOR_STATES.OPENING:
                break;

            case DOOR_STATES.CLOSING:
                break;
        }
    }

    public void Interact()
    {
        ToggleDoor();
    }

    public void ToggleDoor()
    {
        switch (doorState)
        {
            case DOOR_STATES.CLOSED:
                ChangeState(DOOR_STATES.OPENING);
                break;
            case DOOR_STATES.OPEN:
                ChangeState(DOOR_STATES.CLOSING);
                break;
        }
    }

    public DOOR_STATES GetCurrentState()
    {
        return doorState;
    }

    public string InteractionText
    {
        get
        {
            switch (doorState)
            {
                case DOOR_STATES.CLOSED:
                    return openText;

                case DOOR_STATES.OPEN:
                    return closeText;

                default:
                    return "";
            }
        }
    }
}
