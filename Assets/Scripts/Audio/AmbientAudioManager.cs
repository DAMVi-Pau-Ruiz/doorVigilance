
using UnityEngine;
using System.Collections;

public class AmbientAudioController : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    [Header("Ambient Sounds")]
    [SerializeField] private AudioClip defaultSound;
    [SerializeField] private AudioClip gasStationSound;
    [SerializeField] private AudioClip bathroomSound;

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private float defaultVolume = 0.4f;

    private Coroutine fadeCoroutine;

    private void Start()
    {
        PlayAmbient(defaultSound, defaultVolume);
    }

    public void PlayDefault()
    {
        PlayAmbient(defaultSound, defaultVolume);
    }

    public void PlayGasStation(float zoneVolume)
    {
        PlayAmbient(gasStationSound, zoneVolume);
    }

    public void PlayBathroom(float zoneVolume)
    {
        PlayAmbient(bathroomSound, zoneVolume);
    }

    private void PlayAmbient(AudioClip newSound, float targetVolume)
    {
        if (newSound == null)
        {
            return;
        }

        if (audioSource.clip == newSound && audioSource.isPlaying)
        {
            return;
        }

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeAmbient(newSound, targetVolume));
    }

    private IEnumerator FadeAmbient(AudioClip newSound, float targetVolume)
    {
        while (audioSource.volume > 0.01f)
        {
            audioSource.volume = Mathf.MoveTowards(
                audioSource.volume, 0f,
                Time.deltaTime / fadeDuration
            );

            yield return null;
        }

        audioSource.Stop();
        audioSource.clip = newSound;
        audioSource.loop = true;
        audioSource.Play();

        while (audioSource.volume < targetVolume)
        {
            audioSource.volume = Mathf.MoveTowards(
                audioSource.volume, targetVolume,
                Time.deltaTime / fadeDuration
            );

            yield return null;
        }

        audioSource.volume = targetVolume;
        fadeCoroutine = null;
    }
}
