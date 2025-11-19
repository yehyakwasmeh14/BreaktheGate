using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public float gameOverDelay = 2f;

    [Header("Victory UI")]
    public GameObject victoryPanel;
    public float victoryDelay = 2f;

    [Header("Objective UI")]
    public GameObject objectivePanel;
    public TMP_Text timerText;
    public float missionTimeLimit = 180f;

    private float timeRemaining;
    private bool timerRunning = true;
    private bool gameEnded = false;

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
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }

        if (objectivePanel != null)
        {
            objectivePanel.SetActive(true);
        }

        timeRemaining = missionTimeLimit;
    }

    void Update()
    {
        if (timerRunning && timeRemaining > 0 && !gameEnded)
        {
            timeRemaining -= Time.deltaTime;
            
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                UpdateTimerDisplay();
                timerRunning = false;
                OnTimerExpired();
            }
            else
            {
                UpdateTimerDisplay();
            }
        }

        if (gameEnded && UnityEngine.InputSystem.Keyboard.current != null && 
            UnityEngine.InputSystem.Keyboard.current.rKey.wasPressedThisFrame)
        {
            RestartGame();
        }
    }

    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);
            timerText.text = string.Format("{0}:{1:00}", minutes, seconds);

            if (timeRemaining <= 30f)
            {
                timerText.color = Color.red;
            }
            else if (timeRemaining <= 60f)
            {
                timerText.color = Color.yellow;
            }
        }
    }

    void OnTimerExpired()
    {
        timerRunning = false;
        gameEnded = true;
        StartCoroutine(ShowGameOverDelayed());
    }

    public void HideObjective()
    {
        timerRunning = false;
        
        if (objectivePanel != null)
        {
            objectivePanel.SetActive(false);
        }
    }

    public void ShowGameOver()
    {
        if (gameEnded) return;
        gameEnded = true;
        StartCoroutine(ShowGameOverDelayed());
    }

    public void ShowVictory()
    {
        if (gameEnded) return;
        gameEnded = true;
        StartCoroutine(ShowVictoryDelayed());
    }

    IEnumerator ShowGameOverDelayed()
    {
        yield return new WaitForSeconds(gameOverDelay);

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            FixUIRaycastTargets(gameOverPanel);
        }

        DisablePlayerControls();
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    IEnumerator ShowVictoryDelayed()
    {
        yield return new WaitForSeconds(victoryDelay);

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            FixUIRaycastTargets(victoryPanel);
        }

        DisablePlayerControls();
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void FixUIRaycastTargets(GameObject panel)
    {
        TMP_Text[] allTexts = panel.GetComponentsInChildren<TMP_Text>();
        foreach (TMP_Text text in allTexts)
        {
            text.raycastTarget = false;
        }
        
        Image panelImage = panel.GetComponent<Image>();
        if (panelImage != null)
        {
            panelImage.raycastTarget = false;
        }
    }

    void DisablePlayerControls()
    {
        ThirdPersonMovement playerMovement = FindFirstObjectByType<ThirdPersonMovement>();
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        WeaponController[] weapons = FindObjectsByType<WeaponController>(FindObjectsSortMode.None);
        foreach (WeaponController weapon in weapons)
        {
            weapon.enabled = false;
        }

        PlayerInput playerInput = FindFirstObjectByType<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.currentActionMap.Disable();
        }
    }

    public void RestartGame()
    {
        StopAllCoroutines();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
