using UnityEngine;
using UnityEngine.UI;

public class LighterBar : MonoBehaviour
{
    public Slider lighterSlider;

    public void UpdateLightDuration(float currentLightDuration, float maxLightDuration)
    {
        if (lighterSlider != null)
        {
            // Add debug statements for troubleshooting
            Debug.Log($"currentLightDuration: {currentLightDuration}, maxLightDuration: {maxLightDuration}");

            // Ensure maxLightDuration is not zero to prevent division by zero
            float normalizedValue = maxLightDuration > 0 ? currentLightDuration / maxLightDuration : 0f;

            lighterSlider.value = normalizedValue;
        }
    }
}
