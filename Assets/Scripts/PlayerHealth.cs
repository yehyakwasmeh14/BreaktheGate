using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Health Regeneration")]
    public bool enableRegeneration = true;
    public float regenDelay = 3f;
    public int regenAmount = 1;
    public float regenInterval = 0.1f;

    [Header("UI Settings")]
    public GameObject[] healthBars;

    [Header("Death Settings")]
    public GameObject deathExplosionVFX;
    public AudioClip explosionSound;
    public float explosionVolume = 0.8f;
    public float destroyDelay = 0.1f;

    private bool hasDied = false;
    private float timeSinceLastDamage;
    private float regenTimer;

    void Awake()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    void Update()
    {
        if (hasDied || !enableRegeneration || currentHealth >= maxHealth)
        {
            return;
        }

        timeSinceLastDamage += Time.deltaTime;

        if (timeSinceLastDamage >= regenDelay)
        {
            regenTimer += Time.deltaTime;

            if (regenTimer >= regenInterval)
            {
                Heal(regenAmount);
                regenTimer = 0f;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (hasDied) return;

        currentHealth -= damage;
        timeSinceLastDamage = 0f;
        regenTimer = 0f;
        
        UpdateHealthUI();
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        if (healthBars == null || healthBars.Length == 0)
            return;

        int barsToShow = Mathf.CeilToInt(currentHealth / 10f);

        for (int i = 0; i < healthBars.Length; i++)
        {
            if (healthBars[i] != null)
            {
                healthBars[i].SetActive(i < barsToShow);
            }
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

        if (deathExplosionVFX != null)
        {
            Instantiate(deathExplosionVFX, transform.position, Quaternion.identity);
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowGameOver();
        }

        Destroy(gameObject, destroyDelay);
    }
}
