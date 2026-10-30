
using UnityEngine;

public class FootstepController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AudioSource audioSource;

    [Header("Footsteps - Concrete")]
    [SerializeField] private AudioClip[] concreteSounds;

    [Header("Footsteps - Grass")]
    [SerializeField] private AudioClip[] grassSounds;

    [Header("Footsteps - Stone")]
    [SerializeField] private AudioClip[] stoneSounds;

    [Header("Footsteps - Metal")]
    [SerializeField] private AudioClip[] metalSounds;

    [Header("Settings")]
    [SerializeField] private float stepInterval = 0.45f;
    [SerializeField] private float rayDistance = 1.5f;

    private CharacterController controller;
    private float stepTimer;

    private int lastConcreteIndex = -1;
    private int lastGrassIndex = -1;
    private int lastStoneIndex = -1;
    private int lastMetalIndex = -1;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        CheckMovement();
    }

    private void CheckMovement()
    {
        Vector3 horizontalVelocity = controller.velocity;
        horizontalVelocity.y = 0;

        if (horizontalVelocity.magnitude > 0.1f && controller.isGrounded)
        {
            stepTimer += Time.deltaTime;

            if (stepTimer >= stepInterval)
            {
                PlayFootstep();
                stepTimer = 0;
            }
        }
        else
        {
            stepTimer = 0;
        }
    }

    private void PlayFootstep()
    {
        Vector3 origin = controller.bounds.center;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.CompareTag("Concrete"))
            {
                Debug.Log("SONIDO CONCRETE");
                PlayRandomSound(concreteSounds, ref lastConcreteIndex);
            }
            else if (hit.collider.CompareTag("Grass"))
            {
                Debug.Log("SONIDO GRASS");
                PlayRandomSound(grassSounds, ref lastGrassIndex);
            }
            else if (hit.collider.CompareTag("Stone"))
            {
                Debug.Log("SONIDO STONE");
                PlayRandomSound(stoneSounds, ref lastStoneIndex);
            }
            else if (hit.collider.CompareTag("Metal"))
            {
                Debug.Log("SONIDO METAL");
                PlayRandomSound(metalSounds, ref lastMetalIndex);
            }
        }
    }

    private void PlayRandomSound(AudioClip[] sounds, ref int lastIndex)
    {
        if (sounds == null || sounds.Length == 0)
        {
            return;
        }

        int randomIndex;

        if (sounds.Length == 1)
        {
            randomIndex = 0;
        }
        else
        {
            do
            {
                randomIndex = Random.Range(0, sounds.Length);
            }
            while (randomIndex == lastIndex);
        }

        lastIndex = randomIndex;

        audioSource.pitch = Random.Range(0.9f, 1.1f);

        float volume = Random.Range(0.85f, 1f);

        audioSource.PlayOneShot(sounds[randomIndex], volume);

        audioSource.pitch = 1f;
    }
}
