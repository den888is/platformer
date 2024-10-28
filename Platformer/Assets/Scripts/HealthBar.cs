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
    void Update()//���������� ��� ���������� ����� �������� (����)
    {
        currentHealth = this.GetComponent<HealthBar>().currentHealth;
        Debug.Log("HealthBar Update");
        healthBar.fillAmount = currentHealth / maxHealth;
        Debug.Log("healthBar.fillAmount = " + healthBar.fillAmount);
    }
    void UpdateHealthBar()//���������� ��� ���������� ����� ��������
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }
}
