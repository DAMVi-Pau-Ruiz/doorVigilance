
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

    public LevelEvent GetCurrentEvent()
    {
        return currentEvent;
    }
}
