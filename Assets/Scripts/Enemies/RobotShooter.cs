using UnityEngine;

public class RobotShooter : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float range = 20f;
    public float shootInterval = 1f;
    public float aimHeight = 1.5f;
    
    [Header("Aiming Settings")]
    [Range(0f, 180f)]
    public float shootingAngle = 45f;
    public float spawnOffset = 1.5f;
    
    [Header("Audio Settings")]
    public AudioClip[] shootSounds;
    public float shootVolume = 0.7f;
    public float minDistance = 5f;
    public float maxDistance = 50f;

    private Transform playerTransform;
    private float nextShootTime;
    private Robot robotAI;
    private AudioSource audioSource;

    private const string PLAYER_TAG = "Player";

    void Awake()
    {
        robotAI = GetComponent<Robot>();
        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        audioSource.spatialBlend = 1f;
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.volume = shootVolume;
    }

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag(PLAYER_TAG);
        if (player != null)
        {
            playerTransform = player.transform;
        }
        
        if (shootPoint == null)
        {
            shootPoint = transform;
        }
    }

    void Update()
    {
        if (playerTransform == null || bulletPrefab == null)
        {
            return;
        }

        if (GameManager.Instance != null && !GameManager.Instance.IsGateDestroyed)
        {
            return;
        }

        if (robotAI != null && !robotAI.IsChasing)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, playerTransform.position);
        
        if (distance <= range && IsFacingPlayer() && Time.time >= nextShootTime)
        {
            FireBullet();
            nextShootTime = Time.time + shootInterval;
        }
    }

    bool IsFacingPlayer()
    {
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        return angle <= shootingAngle;
    }

    void FireBullet()
    {
        if (shootSounds.Length > 0 && audioSource != null)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(shootSounds[Random.Range(0, shootSounds.Length)]);
        }
        
        Vector3 targetPosition = playerTransform.position;
        targetPosition.y = shootPoint.position.y + aimHeight;
        
        Vector3 direction = (targetPosition - shootPoint.position).normalized;
        direction.y = 0f;
        direction.Normalize();
        
        Vector3 spawnPosition = shootPoint.position + direction * spawnOffset;
        Quaternion rotation = Quaternion.LookRotation(direction);
        
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, rotation);
        
        if (bullet != null)
        {
            bullet.transform.localScale = Vector3.one * 0.5f;
            
            Collider[] robotColliders = GetComponentsInChildren<Collider>();
            Collider bulletCollider = bullet.GetComponent<Collider>();
            
            if (bulletCollider != null)
            {
                foreach (Collider col in robotColliders)
                {
                    Physics.IgnoreCollision(bulletCollider, col);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
        
        Vector3 forward = transform.forward * range;
        Vector3 rightBoundary = Quaternion.Euler(0, shootingAngle, 0) * forward;
        Vector3 leftBoundary = Quaternion.Euler(0, -shootingAngle, 0) * forward;
        
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position + rightBoundary, transform.position + leftBoundary);
    }
}
