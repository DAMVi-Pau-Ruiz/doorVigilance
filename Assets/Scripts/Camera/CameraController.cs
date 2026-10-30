
using UnityEngine;

public enum SecurityCameraState
{
    IDLE,
    MOVING,
    CHASING
}

public class CameraController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private SecurityCameraState cameraState;
    [SerializeField] private float idleTime = 3;
    [SerializeField] private float rotationSpeed = 60;
    [SerializeField, Range(0, 180)] private float maxAngleLeft = 60;
    [SerializeField, Range(0, 180)] private float maxAngleRight = 60;
    [SerializeField] private LayerMask obstacleLayers;

    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float visionDistance = 10;
    [SerializeField] private float loseSightDistance = 15;
    [SerializeField, Range(0, 180)] private float visionAngle = 45;
    [SerializeField, Range(0, 180)] private float loseSightAngle = 70;

    [Header("Gizmos")]
    [SerializeField] private bool showGizmos = true;
    [SerializeField, Range(5, 60)] private int gizmoSegments = 30;

    [Header("Light")]
    [SerializeField] private Light securityLight;
    [SerializeField] private Color normalColor = new Color(1, 0.85f, 0.55f);
    [SerializeField] private Color chasingColor = Color.red;

    private float initialRotationY;
    private float patrolTargetY;
    private float idleTimer;
    private bool movingToRight = true;

    void Start()
    {
        initialRotationY = transform.eulerAngles.y;
    }

    void Update()
    {
        UpdateDetection();
        UpdateState();
    }

    void ChangeState(SecurityCameraState newState)
    {
        if (cameraState == newState)
        {
            return;
        }

        cameraState = newState;

        UpdateLightColor();

        if (newState == SecurityCameraState.IDLE)
        {
            idleTimer = 0;
        }
        else if (newState == SecurityCameraState.MOVING)
        {
            if (movingToRight)
            {
                patrolTargetY = initialRotationY + maxAngleRight;
            }
            else
            {
                patrolTargetY = initialRotationY - maxAngleLeft;
            }
        }
    }

    void UpdateDetection()
    {
        if (player == null)
        {
            return;
        }

        bool canSeePlayer = CanSeePlayer();

        if (canSeePlayer && cameraState != SecurityCameraState.CHASING)
        {
            ChangeState(SecurityCameraState.CHASING);
        }
        else if (!canSeePlayer && cameraState == SecurityCameraState.CHASING)
        {
            ChangeState(SecurityCameraState.IDLE);
        }
    }

    bool CanSeePlayer()
    {
        if (player == null)
        {
            return false;
        }

        Vector3 direction = player.position - transform.position;
        direction.y = 0;

        float distance = direction.magnitude;

        float currentDistance = cameraState == SecurityCameraState.CHASING ? loseSightDistance : visionDistance;

        float currentAngle = cameraState == SecurityCameraState.CHASING ? loseSightAngle : visionAngle;

        if (distance > currentDistance)
        {
            return false;
        }

        Vector3 forward = transform.forward;
        forward.y = 0;

        if (direction.sqrMagnitude > 0.001f)
        {
            float angle = Vector3.Angle(forward, direction);

            if (angle > currentAngle)
            {
                return false;
            }
        }

        Vector3 origin = transform.position + Vector3.up * 0.5f;
        Vector3 target = player.position + Vector3.up * 0.5f;
        Vector3 rayDirection = target - origin;

        if (Physics.Raycast(origin, rayDirection.normalized, rayDirection.magnitude, obstacleLayers))
        {
            return false;
        }

        return true;
    }

    void UpdateState()
    {
        switch (cameraState)
        {
            case SecurityCameraState.IDLE:
                idleTimer += Time.deltaTime;

                if (idleTimer >= idleTime)
                {
                    ChangeState(SecurityCameraState.MOVING);
                }
                break;

            case SecurityCameraState.MOVING:
                UpdatePatrol();
                break;

            case SecurityCameraState.CHASING:
                UpdateChasing();
                break;
        }
    }

    void UpdatePatrol()
    {
        RotateToY(patrolTargetY);

        if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, patrolTargetY)) < 0.5f)
        {
            movingToRight = !movingToRight;
            ChangeState(SecurityCameraState.IDLE);
        }
    }

    void UpdateChasing()
    {
        if (player == null)
        {
            return;
        }

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            float targetY = Quaternion.LookRotation(direction).eulerAngles.y;
            RotateToY(targetY);
        }
    }

    float ClampRotation(float targetY)
    {
        float angle = Mathf.DeltaAngle(initialRotationY, targetY);

        angle = Mathf.Clamp(angle, -maxAngleLeft, maxAngleRight);

        return initialRotationY + angle;
    }

    void RotateToY(float targetY)
    {
        targetY = ClampRotation(targetY);

        Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, targetY, transform.eulerAngles.z);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        if (!showGizmos)
        {
            return;
        }

        float baseRotation = Application.isPlaying ? initialRotationY : transform.eulerAngles.y;

        Vector3 origin = transform.position + Vector3.up * 0.5f;

        Gizmos.color = Color.green;

        Vector3 previousPoint = origin;

        for (int i = 0; i <= gizmoSegments; i++)
        {
            float angle = Mathf.Lerp(-maxAngleLeft, maxAngleRight, (float)i / gizmoSegments);

            Vector3 direction = Quaternion.Euler(0f, baseRotation + angle, 0f) * Vector3.forward;

            Vector3 point = origin + direction * visionDistance;

            Gizmos.DrawLine(previousPoint, point);
            previousPoint = point;
        }

        Vector3 leftDirection = Quaternion.Euler(0f, baseRotation - maxAngleLeft, 0f) * Vector3.forward;

        Vector3 rightDirection = Quaternion.Euler(0f, baseRotation + maxAngleRight, 0f) * Vector3.forward;

        Gizmos.DrawLine(origin, origin + leftDirection * visionDistance);
        Gizmos.DrawLine(origin, origin + rightDirection * visionDistance);

        Gizmos.color = Color.yellow;

        Vector3 forward = transform.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 leftVision = Quaternion.Euler(0f, -visionAngle, 0f) * forward;

        Vector3 rightVision = Quaternion.Euler(0f, visionAngle, 0f) * forward;

        Gizmos.DrawRay(origin, leftVision * visionDistance);
        Gizmos.DrawRay(origin, rightVision * visionDistance);
        Gizmos.DrawRay(origin, forward * visionDistance);
    }

    void UpdateLightColor()
    {
        if (securityLight == null)
        {
            return;
        }

        if (cameraState == SecurityCameraState.CHASING)
        {
            securityLight.color = chasingColor;
        }
        else
        {
            securityLight.color = normalColor;
        }
    }
}
