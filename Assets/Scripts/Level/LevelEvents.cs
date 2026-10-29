
using UnityEngine;

public enum LevelEvent
{
    TAKE_OUT_TRASH,
    RESTORE_POWER,
    GO_TO_BATHROOM,
    CAMERA_ANOMALY,
    FINISHED
}

public class LevelEvents : MonoBehaviour
{
    [Header("Evento actual")]
    [SerializeField] private LevelEvent currentEvent = LevelEvent.TAKE_OUT_TRASH;

    [Header("Electricidad")]
    [SerializeField] private Light[] lightsToControl;
    [SerializeField] private GameObject[] objectsToDisable;
    [SerializeField] private AudioSource blackoutSound;

    [Header("Anomalia de la camara")]
    [SerializeField] private GameObject cameraAnomaly;

    private bool powerIsOn = true;

    private void Start()
    {
        Debug.Log("Tarea: sacar la basura.");
    }

    public void TrashDisposed()
    {
        if (currentEvent != LevelEvent.TAKE_OUT_TRASH)
        {
            return;
        }

        Debug.Log("Has tirado la basura.");

        currentEvent = LevelEvent.RESTORE_POWER;

        TurnOffPower();

        Debug.Log("¡Se ha ido la luz! Busca la caja de los plomos.");
    }

    private void TurnOffPower()
    {
        powerIsOn = false;

        foreach (Light lightObject in lightsToControl)
        {
            if (lightObject != null)
            {
                lightObject.enabled = false;
            }
        }

        foreach (GameObject objectToDisable in objectsToDisable)
        {
            if (objectToDisable != null)
            {
                objectToDisable.SetActive(false);
            }
        }

        if (blackoutSound != null)
        {
            blackoutSound.Play();
        }
    }

    public void RestorePower()
    {
        if (currentEvent != LevelEvent.RESTORE_POWER)
        {
            return;
        }

        if (powerIsOn)
        {
            return;
        }

        powerIsOn = true;

        foreach (Light lightObject in lightsToControl)
        {
            if (lightObject != null)
            {
                lightObject.enabled = true;
            }
        }

        foreach (GameObject objectToDisable in objectsToDisable)
        {
            if (objectToDisable != null)
            {
                objectToDisable.SetActive(true);
            }
        }

        Debug.Log("La electricidad ha vuelto.");

        currentEvent = LevelEvent.GO_TO_BATHROOM;

        Debug.Log("Tienes que ir al baño.");
    }

    public void CompleteBathroomTask()
    {
        if (currentEvent != LevelEvent.GO_TO_BATHROOM)
        {
            return;
        }

        Debug.Log("Has terminado en el baño.");

        currentEvent = LevelEvent.CAMERA_ANOMALY;

        if (cameraAnomaly != null)
        {
            cameraAnomaly.SetActive(true);
        }

        Debug.Log("Algo extraño está pasando con la cámara...");
    }

    public void CompleteCameraEvent()
    {
        if (currentEvent != LevelEvent.CAMERA_ANOMALY)
        {
            return;
        }

        currentEvent = LevelEvent.FINISHED;

        Debug.Log("Has completado todos los eventos.");
    }

    public LevelEvent GetCurrentEvent()
    {
        return currentEvent;
    }
}
