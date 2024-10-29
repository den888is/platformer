using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//������ �������� �� �������� ��������� � ��� �����������
public class HealthBar : MonoBehaviour
{
    public Image healthBar;
    public float currentHealth;
    public float maxHealth = 100f;

    private bool immunity;//������������
    private float immunityTime;//����� ������������ (����������)
    private GameObject hero;//�����

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar = gameObject.GetComponent<Image>();
        hero = GameObject.Find("Hero");
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
    public void MinusHealth(int h)
    {
        if (!immunity)
        {
            immunity = true;//���. ���������
            currentHealth -= h;

            StartCoroutine(StartTimer());
        }
        if (currentHealth <= 0)
        {
            //����� ��������
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    private IEnumerator StartTimer()
    {
        immunityTime = Constants.IMMUNITY_TIME;
        Debug.Log("Start timer");
        hero.GetComponent<PlayerController>().BlinkingRed(immunityTime);
        while (immunityTime > 0)
        {
            immunityTime -= Time.deltaTime;
            yield return null;
        }
        immunity = false;//����. ���������
        Debug.Log("End timer");

        yield return null;
    }
}
