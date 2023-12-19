using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    private bool isGamePaused = false;
    public static GameManager instance;
    public PlayerHealth playerHealth;
    public bool alreadyDie = false;
    public GameCanvas gameCanvas;
    void Start()
    {
        // Ensure the game starts with the Time.timeScale set to 1 (normal speed)
        Time.timeScale = 1f;
        gameOverScreen.SetActive(false);
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    void Update()
    {
        // Check for input to pause/unpause the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
        if(!alreadyDie)
        {
            if (playerHealth.isDie == true)
            {
                alreadyDie = true;
                PlayerDied();
            }
        }
        
        
    }

    public void PlayerDied()
    {
        // Set the game over screen object to not null
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }

        // Pause the game
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        // Reload the current scene when the player chooses to restart
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // Unpause the game
        Time.timeScale = 1f;
        playerHealth.isDie = false;
    }

    void TogglePause()
    {
        // Toggle the game's pause state
        isGamePaused = !isGamePaused;

        // Adjust time scale based on pause state
        Time.timeScale = isGamePaused ? 0f : 1f;
    }

    public void Restart()
    {
        // Unpause the game before reloading the scene
        Time.timeScale = 1f;
        playerHealth.isDie = false;

        // Reload the current scene when the player chooses to restart
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void BackToMainMenu()
    {
        // Unpause the game before going back to the main menu
        Time.timeScale = 1f;
        playerHealth.isDie = false;
        if (gameCanvas != null)
        {
            Destroy(gameCanvas.gameObject);
        }
        // Load the main menu scene
        SceneManager.LoadScene("MainMenu");
    }
}
