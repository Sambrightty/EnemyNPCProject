using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class UIManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject instructionsPanel;
    public GameObject pausePanel;
    public GameObject gameButtonsPanel;
    public GameObject quitConfirmationPanel;
    public GameObject endGamePanel;
    public TextMeshProUGUI resultText; // UnityEngine.UI

    private bool isPaused = false;
    private bool inInstructionOverlay = false;
    private bool gameStarted = false;

    public PlayerBehaviorTracker playerBehaviorTracker;
    public GameObject enemyGameObject;  // Enemy GameObject reference


    private void Start()
    {
        Time.timeScale = 0f; // Start paused
        ShowMainMenu();
    }

    public void StartGame()
    {
        gameStarted = true;
        Time.timeScale = 1f;

        mainMenuPanel.SetActive(false);
        instructionsPanel.SetActive(false);
        quitConfirmationPanel.SetActive(false);
        pausePanel.SetActive(false);

        gameButtonsPanel.SetActive(true);
    }

    public void TogglePause()
    {
        if (inInstructionOverlay || !gameStarted) return;

        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ShowInstructions()
    {
        inInstructionOverlay = true;
        Time.timeScale = 0f;

        instructionsPanel.SetActive(true);
        gameButtonsPanel.SetActive(false);
        pausePanel.SetActive(false);
        quitConfirmationPanel.SetActive(false);
    }

    public void BackToMenu()
    {
        instructionsPanel.SetActive(false);
        inInstructionOverlay = false;

        if (gameStarted)
        {
            gameButtonsPanel.SetActive(true);
            Time.timeScale = 1f;
        }
        else
        {
            ShowMainMenu();
        }
    }

    public void RestartGame()
    {
        if (inInstructionOverlay) return;

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        if (inInstructionOverlay || !gameStarted) return;

        quitConfirmationPanel.SetActive(true);
        gameButtonsPanel.SetActive(false);
        pausePanel.SetActive(false);

        Time.timeScale = 0f;
    }

    public void ConfirmQuitToMainMenu()
    {
        Time.timeScale = 1f; // Avoid freezing on reload
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reset all
    }

    public void CancelQuit()
    {
        quitConfirmationPanel.SetActive(false);
        gameButtonsPanel.SetActive(true);
        Time.timeScale = 1f;
    }

    private void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        instructionsPanel.SetActive(false);
        pausePanel.SetActive(false);
        quitConfirmationPanel.SetActive(false);
        gameButtonsPanel.SetActive(false);
    }

    public void EndGame(string winner)
    {
        Time.timeScale = 0f;

        endGamePanel.SetActive(true);
        gameButtonsPanel.SetActive(false);
        pausePanel.SetActive(false);
        quitConfirmationPanel.SetActive(false);

        resultText.text = winner + " Wins!";

        // NEW: Only evolve if player lost (enemy won)
        if (winner == "Enemy")
        {
            if (playerBehaviorTracker != null && enemyGameObject != null)
            {
                string playerBehavior = playerBehaviorTracker.GetBehaviorType();

                GeneticEnemyAI enemyAI = enemyGameObject.GetComponent<GeneticEnemyAI>();
                if (enemyAI != null)
                {
                    enemyAI.EvolveGene(playerBehavior);
                    Debug.Log("Enemy AI evolved based on player behavior: " + playerBehavior);
                }
            }
        }

        // Reset player behavior counts for next game
        if (playerBehaviorTracker != null)
        {
            playerBehaviorTracker.ResetBehavior();
        }
    }


}
