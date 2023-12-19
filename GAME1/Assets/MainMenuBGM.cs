using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuMusic : MonoBehaviour
{
    public AudioClip menuMusic; // Assign your main menu music clip in the Unity Editor
    private AudioSource audioSource;

    private void Start()
    {
        // Create an AudioSource component dynamically
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = menuMusic;
        audioSource.loop = true;

        // Play the music
        audioSource.Play();
    }

    private void Update()
    {
        // Check if the current scene is different from the main menu scene
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            // Stop the music if not in the main menu scene
            audioSource.Stop();
            // Destroy this script to prevent it from affecting the music in other scenes
            Destroy(this);
        }
    }
}
