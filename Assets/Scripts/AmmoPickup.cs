using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public int ammoAmount = 15;
    public float respawnTime = 30f;

    [Header("Animation Settings")]
    public float floatHeight = 1f;
    public float floatAmplitude = 0.5f;
    public float floatSpeed = 1f;
    public float rotationSpeed = 50f;

    [Header("Audio Settings")]
    public AudioClip pickupSound;
    public float pickupVolume = 0.7f;

    private Vector3 startPosition;
    private MeshRenderer[] meshRenderers;
    private Collider pickupCollider;
    private bool isPickedUp;

    private const string PLAYER_TAG = "Player";

    void Start()
    {
        startPosition = transform.position;
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        pickupCollider = GetComponent<Collider>();
    }

    void Update()
    {
        if (!isPickedUp)
        {
            float offset = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
            float newY = startPosition.y + floatHeight + offset;
            transform.position = new Vector3(startPosition.x, newY, startPosition.z);

            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG) && !isPickedUp)
        {
            WeaponController weapon = other.GetComponentInChildren<WeaponController>();
            if (weapon != null)
            {
                weapon.AddAmmo(ammoAmount);
                isPickedUp = true;

                if (pickupSound != null)
                {
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position, pickupVolume);
                }
                
                foreach (MeshRenderer renderer in meshRenderers)
                {
                    if (renderer != null)
                    {
                        renderer.enabled = false;
                    }
                }
                
                if (pickupCollider != null)
                {
                    pickupCollider.enabled = false;
                }

                Invoke(nameof(Respawn), respawnTime);
            }
        }
    }

    void Respawn()
    {
        isPickedUp = false;
        
        foreach (MeshRenderer renderer in meshRenderers)
        {
            if (renderer != null)
            {
                renderer.enabled = true;
            }
        }
        
        if (pickupCollider != null)
        {
            pickupCollider.enabled = true;
        }
    }
}
