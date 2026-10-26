
using UnityEngine;

public enum SecurityCameraState
{
    IDLE,
    MOVING,
    CHASING
}

public class CameraController : MonoBehaviour
{
    [Header("FSM")]
    [SerializeField] private SecurityCameraState cameraState;

    private float currentStateTime;

    void Update()
    {
        UpdateState();
    }

    void ChangeState(SecurityCameraState newState)
    {
        ExitState();
        EnterState(newState);
    }

    void EnterState(SecurityCameraState newState)
    {
        // Reiniciar temporizador
        currentStateTime = 0;

        switch (newState)
        {
            case SecurityCameraState.IDLE:
                break;

            case SecurityCameraState.MOVING:
                break;

            case SecurityCameraState.CHASING:
                break;
        }
    }

    void UpdateState()
    {
        switch (cameraState)
        {
            case SecurityCameraState.IDLE:
                break;

            case SecurityCameraState.MOVING:
                break;

            case SecurityCameraState.CHASING:
                break;
        }
    }

    void ExitState()
    {
        switch (cameraState)
        {
            case SecurityCameraState.IDLE:
                break;

            case SecurityCameraState.MOVING:
                break;

            case SecurityCameraState.CHASING:
                break;
        }
    }
}
