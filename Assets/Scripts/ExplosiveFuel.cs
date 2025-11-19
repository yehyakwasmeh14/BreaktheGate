using UnityEngine;

public class ExplosiveFuel : MonoBehaviour
{
    [Header("Explosion Settings")]
    public GameObject explosionVFXPrefab;
    public AudioClip bigExplosionSound;
    public float explosionVolume = 1.2f;
    public float destroyDelay = 0.1f;

    [Header("References")]
    public GameObject gateToDestroy;

    private bool hasExploded = false;
    private bool isArmed = false;

    void Start()
    {
        if (gateToDestroy == null)
        {
            gateToDestroy = GameObject.Find("Gate");
        }
    }

    public void Arm()
    {
        isArmed = true;
    }

    public void TakeDamage()
    {
        if (!isArmed || hasExploded)
        {
            return;
        }

        Explode();
    }

    void Explode()
    {
        hasExploded = true;

        if (bigExplosionSound != null)
        {
            AudioSource.PlayClipAtPoint(bigExplosionSound, transform.position, explosionVolume);
        }

        if (explosionVFXPrefab != null)
        {
            GameObject explosion = Instantiate(explosionVFXPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 5f);
        }
        else
        {
            SpawnFallbackExplosion();
        }

        if (gateToDestroy != null)
        {
            Destroy(gateToDestroy, destroyDelay);
            NotifyGateDestroyed();
        }
        else
        {
            GameObject gate = GameObject.Find("Gate");
            if (gate != null)
            {
                Destroy(gate, destroyDelay);
                NotifyGateDestroyed();
            }
        }

        Destroy(gameObject, destroyDelay);
    }

    void NotifyGateDestroyed()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetGateDestroyed();
        }
    }

    void SpawnFallbackExplosion()
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = transform.position;
        sphere.transform.localScale = Vector3.one * 5f;
        Renderer renderer = sphere.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = new Color(1f, 0.5f, 0f, 0.8f);
        }
        Destroy(sphere.GetComponent<Collider>());
        Destroy(sphere, 2f);
    }
}
