using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider volumeSlider; // Reference to the Slider UI element
    private AudioSource audioSource; // Reference to the AudioSource component

    void Start()
    {
        // Ensure we have a valid AudioSource component
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();

            // If AudioSource is still null, add one dynamically
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        // Set the initial volume to the slider value
        audioSource.volume = volumeSlider.value;

        // Add a listener to the slider's value changed event
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

    void ChangeVolume(float volume)
    {
        // Update the volume of the AudioSource
        audioSource.volume = volume;
    }
}
