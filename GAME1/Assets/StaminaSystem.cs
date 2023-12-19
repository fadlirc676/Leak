using UnityEngine;

public class StaminaSystem : MonoBehaviour
{
    public float staminaMax = 100f;
    public float staminaRegenRate = 10f;

    private float currentStamina;

    public StaminaBar staminaBar; // Reference to the StaminaBar script

    void Start()
    {
        currentStamina = staminaMax;

        if (staminaBar != null)
        {
            staminaBar.UpdateStamina(currentStamina, staminaMax);
        }
    }

    void Update()
    {
        Sprinting();
        UpdateStamina();
    }

    void Sprinting()
    {
        bool isSprinting = Input.GetKey(KeyCode.RightShift) && currentStamina > 0;

        if (isSprinting)
        {
            currentStamina -= Time.deltaTime * (staminaMax / 4f);

            if (currentStamina < 0)
            {
                currentStamina = 0;
            }
        }
        else
        {
            currentStamina += Time.deltaTime * staminaRegenRate;
            currentStamina = Mathf.Clamp(currentStamina, 0, staminaMax);
        }

        if (staminaBar != null)
        {
            staminaBar.UpdateStamina(currentStamina, staminaMax);
        }
    }

    void UpdateStamina()
    {
        // Additional logic for updating stamina (if needed)
    }
}
