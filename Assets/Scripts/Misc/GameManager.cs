using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool gateDestroyed = false;
    private int enemiesAlive = 0;

    public static GameManager Instance { get; private set; }
    public bool IsGateDestroyed => gateDestroyed;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        enemiesAlive = FindObjectsByType<EnemyHealth>(FindObjectsSortMode.None).Length;
    }

    public void SetGateDestroyed()
    {
        gateDestroyed = true;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.HideObjective();
        }
    }

    public void OnEnemyDeath()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0 && gateDestroyed)
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowVictory();
            }
        }
    }
}
