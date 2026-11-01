using TMPro;
using UnityEngine;

public class ShowCameraCurrentState : MonoBehaviour
{
    [SerializeField] private TMP_Text currentState;
    [SerializeField] private SecurityCameraController videoCamera;

    private void Update()
    {
        currentState.text = videoCamera.GetCurrentState().ToString();
    }
}
