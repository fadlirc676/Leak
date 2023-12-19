using UnityEngine;

public class LighterSystem : MonoBehaviour
{
    public float maxLightDuration = 20f;
    public float lightRegenRate = 20f;

    private float currentLightDuration;

    public LighterBar lighterBar; // Reference to the LighterBar script

    void Start()
    {
        currentLightDuration = maxLightDuration;

        if (lighterBar != null)
        {
            lighterBar.UpdateLightDuration(currentLightDuration, maxLightDuration);
        }
    }

    void Update()
    {
        UsingLighter();
        UpdateLightDuration();
    }


    void UsingLighter()
    {
        // Decrease the current light duration when using lighter
        currentLightDuration -= Time.deltaTime;

        // Ensure the currentLightDuration stays within bounds
        currentLightDuration = Mathf.Clamp(currentLightDuration, 0, maxLightDuration);

        // Replenish light duration based on light regen rate
        if (!Input.GetKey(KeyCode.Space) && currentLightDuration < maxLightDuration)
        {
            currentLightDuration += Time.deltaTime * lightRegenRate;
            currentLightDuration = Mathf.Clamp(currentLightDuration, 0, maxLightDuration);
        }
    }






    void UpdateLightDuration()
    {
        // Regenerate light duration over time
        if (currentLightDuration < maxLightDuration)
        {
            currentLightDuration += Time.deltaTime * lightRegenRate;
            currentLightDuration = Mathf.Clamp(currentLightDuration, 0, maxLightDuration);

            if (lighterBar != null)
            {
                lighterBar.UpdateLightDuration(currentLightDuration, maxLightDuration);
            }
        }
    }
}
