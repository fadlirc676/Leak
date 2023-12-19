using UnityEngine;
using UnityEngine.UI;

public class UIButtonSound : MonoBehaviour
{
    public AudioClip clickSound;

    private Button button;
    private AudioSource audioSource;

    void Start()
    {
        button = GetComponent<Button>();
        audioSource = GetComponent<AudioSource>();

        // Check if AudioSource component is not present and add it
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set the audio clip for the button click sound
        audioSource.clip = clickSound;

        // Add a listener to the button click event
        button.onClick.AddListener(PlayClickSound);
    }

    void PlayClickSound()
    {
        // Play the click sound
        audioSource.Play();
    }
}
