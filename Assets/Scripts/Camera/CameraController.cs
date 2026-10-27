
using UnityEngine;

public enum SecurityCameraState
{
    IDLE,
    MOVING,
    CHASING
}

public class CameraController : MonoBehaviour
{
    [Header("Estado")]
    [SerializeField] private SecurityCameraState cameraState;

    [Header("Jugador")]
    [SerializeField] private Transform player;
    [SerializeField] private float visionDistance = 10f;
    [SerializeField] private float loseSightDistance = 15f;
    [SerializeField, Range(0f, 180f)] private float visionAngle = 45f;
    [SerializeField, Range(0f, 180f)] private float loseSightAngle = 70f;

    [Header("Rotacion")]
    [SerializeField] private float rotationSpeed = 60f;

    [Header("Limites de giro")]
    [SerializeField, Range(0f, 180f)] private float maxAngleLeft = 60f;
    [SerializeField, Range(0f, 180f)] private float maxAngleRight = 60f;

    [Header("Patrulla")]
    [SerializeField] private float idleTime = 3f;

    [Header("Obstaculos")]
    [SerializeField] private LayerMask obstacleLayers;

    [Header("Gizmos")]
    [SerializeField] private bool showGizmos = true;
    [SerializeField, Range(5, 60)] private int gizmoSegments = 30;


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
            return;

        cameraState = newState;

        if (newState == SecurityCameraState.IDLE)
        {
            idleTimer = 0f;
        }
        else if (newState == SecurityCameraState.MOVING)
        {
            if (movingToRight)
                patrolTargetY = initialRotationY + maxAngleRight;
            else
                patrolTargetY = initialRotationY - maxAngleLeft;
        }
    }

    void UpdateDetection()
    {
        if (player == null)
            return;

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
            return false;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        float distance = direction.magnitude;

        float currentDistance = cameraState == SecurityCameraState.CHASING
            ? loseSightDistance
            : visionDistance;

        float currentAngle = cameraState == SecurityCameraState.CHASING
            ? loseSightAngle
            : visionAngle;

        if (distance > currentDistance)
            return false;

        Vector3 forward = transform.forward;
        forward.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            float angle = Vector3.Angle(forward, direction);

            if (angle > currentAngle)
                return false;
        }

        Vector3 origin = transform.position + Vector3.up * 0.5f;
        Vector3 target = player.position + Vector3.up * 0.5f;
        Vector3 rayDirection = target - origin;

        if (Physics.Raycast(
            origin,
            rayDirection.normalized,
            rayDirection.magnitude,
            obstacleLayers))
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
                    ChangeState(SecurityCameraState.MOVING);

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

        if (Mathf.Abs(Mathf.DeltaAngle(
            transform.eulerAngles.y, patrolTargetY)) < 0.5f)
        {
            movingToRight = !movingToRight;
            ChangeState(SecurityCameraState.IDLE);
        }
    }

    void UpdateChasing()
    {
        if (player == null)
            return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            float targetY =
                Quaternion.LookRotation(direction).eulerAngles.y;

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

        Quaternion targetRotation = Quaternion.Euler(
            transform.eulerAngles.x,
            targetY,
            transform.eulerAngles.z
        );

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    public void StartPatrol()
    {
        if (cameraState == SecurityCameraState.IDLE)
            ChangeState(SecurityCameraState.MOVING);
    }

    public void StopPatrol()
    {
        if (cameraState == SecurityCameraState.MOVING)
            ChangeState(SecurityCameraState.IDLE);
    }

    void OnDrawGizmosSelected()
    {
        if (!showGizmos)
            return;

        float baseRotation = Application.isPlaying
            ? initialRotationY
            : transform.eulerAngles.y;

        Vector3 origin = transform.position + Vector3.up * 0.5f;

        // Area verde de giro permitido
        Gizmos.color = Color.green;

        Vector3 previousPoint = origin;

        for (int i = 0; i <= gizmoSegments; i++)
        {
            float angle = Mathf.Lerp(
                -maxAngleLeft,
                maxAngleRight,
                (float)i / gizmoSegments
            );

            Vector3 direction =
                Quaternion.Euler(0f, baseRotation + angle, 0f)
                * Vector3.forward;

            Vector3 point = origin + direction * visionDistance;

            Gizmos.DrawLine(previousPoint, point);
            previousPoint = point;
        }

        Vector3 leftDirection =
            Quaternion.Euler(0f, baseRotation - maxAngleLeft, 0f)
            * Vector3.forward;

        Vector3 rightDirection =
            Quaternion.Euler(0f, baseRotation + maxAngleRight, 0f)
            * Vector3.forward;

        Gizmos.DrawLine(origin, origin + leftDirection * visionDistance);
        Gizmos.DrawLine(origin, origin + rightDirection * visionDistance);

        // Cono amarillo de vision actual
        Gizmos.color = Color.yellow;

        Vector3 forward = transform.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 leftVision =
            Quaternion.Euler(0f, -visionAngle, 0f) * forward;

        Vector3 rightVision =
            Quaternion.Euler(0f, visionAngle, 0f) * forward;

        Gizmos.DrawRay(origin, leftVision * visionDistance);
        Gizmos.DrawRay(origin, rightVision * visionDistance);
        Gizmos.DrawRay(origin, forward * visionDistance);
    }
}
