using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject robotExplosionVFX;
    public AudioClip explosionSound;
    public float explosionVolume = 0.8f;
    public int maxHealth = 3;

    [Header("Runtime Stats")]
    [SerializeField] private int currentHealth;

    private bool hasDied;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (hasDied) return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (hasDied) return;
        hasDied = true;

        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position, explosionVolume);
        }

        if (robotExplosionVFX != null)
        {
            Instantiate(robotExplosionVFX, transform.position, Quaternion.identity);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnEnemyDeath();
        }

        Destroy(gameObject);
    }

    public void SelfDestruct()
    {
        Die();
    }
}
