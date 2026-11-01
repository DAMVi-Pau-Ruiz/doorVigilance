
using UnityEngine;

public enum CAMERA_STATES
{
    IDLE,
    MOVING,
    CHASING
}

public class SecurityCameraController : MonoBehaviour
{
    [Header("State")]
    [SerializeField] private CAMERA_STATES cameraState;
    [SerializeField] private float idleDuration = 3;

    [Header("Movement")]
    [SerializeField] private float maxRotation = 60;
    [SerializeField] private float rotationSpeed = 60;

    [Header("Detection")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform visionOrigin;
    [SerializeField] private float visionDistance = 10;
    [SerializeField] private float visionAngle = 45;

    [Header("Light")]
    [SerializeField] private Light securityLight;
    [SerializeField] private Color normalColor = new Color(1, 0.85f, 0.55f);
    [SerializeField] private Color chasingColor = Color.red;

    private float currentStateTime;
    private float defaultYRotation;
    private bool movingLeft = false;
    private float targetRotation;
    private Vector3 currentRotation;

    private void Start()
    {
        defaultYRotation = transform.eulerAngles.y;
    }

    private void Update()
    {
        CheckPlayerDetection();
        UpdateState();
        UpdateLightColor();
    }

    private void ChangeState(CAMERA_STATES newState)
    {
        if (cameraState == newState)
        {
            return;
        }

        ExitState();

        cameraState = newState;

        EnterState(newState);
    }

    private void UpdateState()
    {
        currentStateTime += Time.deltaTime;

        switch (cameraState)
        {
            case CAMERA_STATES.IDLE:

                if (currentStateTime >= idleDuration)
                {
                    ChangeState(CAMERA_STATES.MOVING);
                }

                break;

            case CAMERA_STATES.MOVING:

                RotateCamera();

                if (HasReachedTarget())
                {
                    movingLeft = !movingLeft;
                    ChangeState(CAMERA_STATES.IDLE);
                }

                break;

            case CAMERA_STATES.CHASING:

                ChasePlayer();

                break;
        }
    }

    private void EnterState(CAMERA_STATES newState)
    {
        currentStateTime = 0;

        switch (newState)
        {
            case CAMERA_STATES.IDLE:

                break;

            case CAMERA_STATES.MOVING:

                targetRotation = GetTargetRotation();

                break;

            case CAMERA_STATES.CHASING:

                break;
        }
    }

    private void ExitState()
    {
        switch (cameraState)
        {
            case CAMERA_STATES.IDLE:

                break;

            case CAMERA_STATES.MOVING:

                break;

            case CAMERA_STATES.CHASING:

                break;
        }
    }

    private float GetTargetRotation()
    {
        if (movingLeft)
        {
            return defaultYRotation - maxRotation;
        }
        else
        {
            return defaultYRotation + maxRotation;
        }
    }

    private void RotateCamera()
    {
        currentRotation = transform.eulerAngles;

        currentRotation.y = Mathf.MoveTowardsAngle(currentRotation.y, targetRotation, rotationSpeed * Time.deltaTime);

        transform.eulerAngles = currentRotation;
    }

    private bool HasReachedTarget()
    {
        return Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetRotation)) < 0.1f;
    }

    private bool CanSeePlayer(bool checkAngle)
    {
        Vector3 origin = visionOrigin != null ? visionOrigin.position : transform.position;

        Vector3 direction = player.position - origin;

        float distance = direction.magnitude;

        if (distance > visionDistance)
        {
            return false;
        }

        direction.Normalize();

        if (checkAngle)
        {
            float angle = Vector3.Angle(transform.forward, direction);

            if (angle > visionAngle)
            {
                return false;
            }
        }

        RaycastHit hit;

        if (Physics.Raycast(origin, direction, out hit, distance))
        {
            return hit.transform == player || hit.transform.IsChildOf(player);
        }

        return false;
    }

    private void CheckPlayerDetection()
    {
        if (player == null)
        {
            return;
        }

        if (cameraState == CAMERA_STATES.CHASING)
        {
            if (!CanSeePlayer(false))
            {
                ChangeState(CAMERA_STATES.IDLE);
            }
        }
        else
        {
            if (CanSeePlayer(true))
            {
                ChangeState(CAMERA_STATES.CHASING);
            }
        }
    }

    private void ChasePlayer()
    {
        Vector3 direction = player.position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        float targetYRotation = targetRotation.eulerAngles.y;

        currentRotation = transform.eulerAngles;

        currentRotation.y = Mathf.MoveTowardsAngle(currentRotation.y, targetYRotation, rotationSpeed * Time.deltaTime);

        transform.eulerAngles = currentRotation;
    }

    private void UpdateLightColor()
    {
        if (securityLight == null)
        {
            return;
        }

        if (cameraState == CAMERA_STATES.CHASING)
        {
            securityLight.color = chasingColor;
        }
        else
        {
            securityLight.color = normalColor;
        }
    }

    public CAMERA_STATES GetCurrentState()
    {
        return cameraState;
    }
}
