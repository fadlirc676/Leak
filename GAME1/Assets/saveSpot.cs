using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSpot : MonoBehaviour
{
    // This variable will store the position of the last save spot
    private static Vector3 lastSavePosition;

    private void Start()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Set the last save position based on the player's initial position in the scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            lastSavePosition = player.transform.position;
            Debug.Log("Initial save spot set to: " + lastSavePosition);
        }
    }

    // This method can be called to respawn the player at the last save spot
    public static void RespawnPlayer(GameObject player)
    {
        // Set the player's position to the last save spot
        player.transform.position = lastSavePosition;

        // Reset the player's health or any other relevant components
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.currentHealth = playerHealth.maxHealth;
            playerHealth.HealtBar.SetHealth(playerHealth.currentHealth);
            playerHealth.isDie = false;
        }

        // You can add additional logic here, like displaying a respawn message
        Debug.Log("Player respawned at: " + lastSavePosition);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Automatically save the player's position when the scene changes
        PlayerPrefs.SetString("LastSavePosition", JsonUtility.ToJson(lastSavePosition));
    }
}
