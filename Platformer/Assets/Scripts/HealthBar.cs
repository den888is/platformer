using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;
    public float currentHealth;
    public float maxHealth = 100f;

    private void Start()
    {
        healthBar = gameObject.GetComponent<Image>();
    }
    void Update()//вызываетс€ при обновлении шкалы здоровь€ (тест)
    {
        currentHealth = this.GetComponent<HealthBar>().currentHealth;
        Debug.Log("HealthBar Update");
        healthBar.fillAmount = currentHealth / maxHealth;
        Debug.Log("healthBar.fillAmount = " + healthBar.fillAmount);
    }
    void UpdateHealthBar()//вызываетс€ при обновлении шкалы здоровь€
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }
}
