using UnityEngine;

public enum AmbientZoneType
{
    DEFAULT,
    GAS_STATION,
    BATHROOM
}

public class AmbientZone : MonoBehaviour
{
    [SerializeField] private AmbientAudioController ambientAudio;

    [Header("Zone Settings")]
    [SerializeField] private AmbientZoneType zoneType;

    [Range(0f, 1f)]
    [SerializeField] private float zoneVolume = 0.4f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        switch (zoneType)
        {
            case AmbientZoneType.GAS_STATION:
                ambientAudio.PlayGasStation(zoneVolume);
                break;

            case AmbientZoneType.BATHROOM:
                ambientAudio.PlayBathroom(zoneVolume);
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        ambientAudio.PlayDefault();
    }
}