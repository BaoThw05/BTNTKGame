using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Image healthFill; // Reference to the Image component that fills the health bar
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    public void UpdateBar(float currentHealth, float maxHealth)
    {
        float fillAmount = currentHealth / maxHealth;
        healthFill.fillAmount = fillAmount;
    }
}
