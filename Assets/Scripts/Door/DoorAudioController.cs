using UnityEngine;

public class DoorAudioController : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    [Header("Sounds")]
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;

    public void PlayOpenSound()
    {
        PlaySound(openSound);
    }

    public void PlayCloseSound()
    {
        PlaySound(closeSound);
    }

    private void PlaySound(AudioClip sound)
    {
        if (sound != null)
        {
            audioSource.PlayOneShot(sound);
        }
    }
}
