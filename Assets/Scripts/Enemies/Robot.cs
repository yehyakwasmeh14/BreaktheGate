using UnityEngine;
using UnityEngine.AI;

public class Robot : MonoBehaviour
{
    [Header("Detection Settings")]
    public float detectionRadius = 10f;
    
    [Header("Movement Settings")]
    public float stoppingDistance = 5f;
    public float rotationSpeed = 5f;

    [Header("Wandering Settings")]
    public float wanderRadius = 20f;
    public float minWaitTime = 0.5f;
    public float maxWaitTime = 2f;

    [Header("Combat Settings")]
    public int damageToPlayer = 20;

    private Transform player;
    private NavMeshAgent agent;
    private bool isChasing;
    private bool isWandering;
    private float waitTimer;
    private Vector3 wanderTarget;
    private Vector3 lastPosition;
    private float stuckTimer;

    private const string PLAYER_TAG = "Player";
    private const float STUCK_CHECK_INTERVAL = 0.5f;
    private const float STUCK_THRESHOLD = 0.1f;
    private const float MIN_WANDER_DISTANCE = 2f;

    public bool IsChasing => isChasing;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        lastPosition = transform.position;
    }

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag(PLAYER_TAG);
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            ThirdPersonMovement playerController = FindFirstObjectByType<ThirdPersonMovement>();
            if (playerController != null)
            {
                player = playerController.transform;
            }
        }
        
        if (agent != null)
        {
            agent.avoidancePriority = Random.Range(40, 60);
        }

        SetNewWanderTarget();
    }

    void Update()
    {
        if (agent == null || player == null) return;

        if (GameManager.Instance != null && !GameManager.Instance.IsGateDestroyed)
        {
            isChasing = false;
            isWandering = false;
            agent.isStopped = true;
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            ChasePlayer();
        }
        else
        {
            if (isChasing)
            {
                isChasing = false;
            }
            
            Wander();
        }
    }

    void ChasePlayer()
    {
        isChasing = true;
        isWandering = false;
        agent.isStopped = false;
        agent.stoppingDistance = stoppingDistance;
        agent.SetDestination(player.position);
        
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f;
        
        if (directionToPlayer.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void Wander()
    {
        stuckTimer += Time.deltaTime;
        
        if (stuckTimer >= STUCK_CHECK_INTERVAL)
        {
            float distanceMoved = Vector3.Distance(transform.position, lastPosition);
            
            if (distanceMoved < STUCK_THRESHOLD && agent.hasPath && !isWandering)
            {
                SetNewWanderTarget();
            }
            
            lastPosition = transform.position;
            stuckTimer = 0f;
        }
        
        if (!agent.hasPath || (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 1f))
        {
            if (!isWandering)
            {
                isWandering = true;
                waitTimer = Random.Range(minWaitTime, maxWaitTime);
            }

            waitTimer -= Time.deltaTime;
            
            if (waitTimer <= 0f)
            {
                SetNewWanderTarget();
            }
        }
    }

    void SetNewWanderTarget()
    {
        int attempts = Random.Range(3, 8);
        
        for (int i = 0; i < attempts; i++)
        {
            float randomDistance = Random.Range(5f, wanderRadius);
            Vector3 randomDirection = Random.onUnitSphere;
            randomDirection.y = 0f;
            randomDirection.Normalize();
            
            Vector3 targetPosition = transform.position + randomDirection * randomDistance;
            
            NavMeshHit hit;
            if (NavMesh.SamplePosition(targetPosition, out hit, wanderRadius, NavMesh.AllAreas))
            {
                if (Vector3.Distance(transform.position, hit.position) > MIN_WANDER_DISTANCE)
                {
                    agent.isStopped = false;
                    agent.stoppingDistance = 1f;
                    agent.SetDestination(hit.position);
                    
                    wanderTarget = hit.position;
                    isWandering = false;
                    stuckTimer = 0f;
                    lastPosition = transform.position;
                    return;
                }
            }
        }
        
        Vector3 fallbackDirection = Random.onUnitSphere;
        fallbackDirection.y = 0f;
        fallbackDirection.Normalize();
        Vector3 fallbackTarget = transform.position + fallbackDirection * 3f;
        
        NavMeshHit finalHit;
        if (NavMesh.SamplePosition(fallbackTarget, out finalHit, 5f, NavMesh.AllAreas))
        {
            agent.isStopped = false;
            agent.SetDestination(finalHit.position);
            isWandering = false;
            stuckTimer = 0f;
            lastPosition = transform.position;
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag(PLAYER_TAG)) 
        {
            if (GameManager.Instance != null && !GameManager.Instance.IsGateDestroyed)
            {
                return;
            }

            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageToPlayer);
            }

            EnemyHealth enemyHealth = GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.SelfDestruct();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
        
        if (Application.isPlaying && isWandering)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(wanderTarget, 0.5f);
        }
    }
}
