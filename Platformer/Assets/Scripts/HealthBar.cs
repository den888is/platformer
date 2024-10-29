using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//скрипт отвечает за здоровье персонажа и его отображение
public class HealthBar : MonoBehaviour
{
    public Image healthBar;
    public float currentHealth;
    public float maxHealth = 100f;

    private bool immunity;//неу€звимость
    private float immunityTime;//врем€ неу€звимости (переменна€)
    private GameObject hero;//герой

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar = gameObject.GetComponent<Image>();
        hero = GameObject.Find("Hero");
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
    public void MinusHealth(int h)
    {
        if (!immunity)
        {
            immunity = true;//вкл. иммунитет
            currentHealth -= h;

            StartCoroutine(StartTimer());
        }
        if (currentHealth <= 0)
        {
            //герой погибает
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
        immunity = false;//откл. иммунитет
        Debug.Log("End timer");

        yield return null;
    }
}
