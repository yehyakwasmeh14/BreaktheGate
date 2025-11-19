using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    [Header("Footstep Settings")]
    public AudioClip[] footstepSounds;
    public float stepInterval = 0.5f;
    public float volume = 0.3f;
    
    private AudioSource audioSource;
    private CharacterController controller;
    private float stepTimer;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = volume;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }

    void Update()
    {
        if (controller == null || footstepSounds.Length == 0) return;

        if (controller.velocity.magnitude > 0.1f)
        {
            stepTimer -= Time.deltaTime;
            
            if (stepTimer <= 0f)
            {
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.PlayOneShot(footstepSounds[Random.Range(0, footstepSounds.Length)]);
                stepTimer = stepInterval;
            }
        }
    }
}
