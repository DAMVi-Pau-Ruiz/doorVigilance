using UnityEngine;
using UnityEngine.InputSystem;

public class HeadBob : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference moveAction;

    [Header("Head Bob")]
    [SerializeField] private float bobFrequency = 10;
    [SerializeField] private float bobAmplitude = 0.05f;
    [SerializeField] private float returnSpeed = 5;

    private Vector3 initialPosition;
    private float bobTimer;

    private void Start()
    {
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();

        if (input.magnitude > 0.1f)
        {
            bobTimer += Time.deltaTime * bobFrequency;

            float offsetY = Mathf.Sin(bobTimer) * bobAmplitude;

            transform.localPosition = initialPosition + Vector3.up * offsetY;
        }
        else
        {
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                initialPosition,
                Time.deltaTime * returnSpeed
            );
        }
    }
}