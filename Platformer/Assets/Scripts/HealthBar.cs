using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//скрипт отвечает за здоровье персонажа и его отображение
public class HealthBar : MonoBehaviour
{
    public Image healthBar;
    public float currentHealth;

    private float maxHealth = 100f;
    private bool immunity;//неуязвимость
    private float immunityTime;//время неуязвимости (переменная)
    private GameObject hero;//герой

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar = gameObject.GetComponent<Image>();
        hero = GameObject.Find("Hero");
    }
    void Update()//вызывается при обновлении шкалы здоровья (тест)
    {
        // currentHealth = GetComponent<HealthBar>().currentHealth;
        Debug.Log("HealthBar Update");
        healthBar.fillAmount = currentHealth / maxHealth;
        Debug.Log("healthBar.fillAmount = " + healthBar.fillAmount);
    }
    void UpdateHealthBar()//вызывается при обновлении шкалы здоровья
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }
    //принимает только положительные значения
    public void MinusHealth(int h)
    {
        if (h <= 0) { Debug.LogError("Только положительные"); }
        else
        if (!immunity)
        {
            immunity = true;//вкл. иммунитет
            currentHealth -= h;
            if (currentHealth <= 0)
            {
                //герой погибает
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            //сделаться неуязвимым временно
            StartCoroutine(StartTimer());
        }
    }
    public void PlusKitHealth(GameObject g)
    {
        currentHealth = maxHealth;//восполнили здоровье
        Destroy(g);//убрали объект
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
    //принимает любые значения velocity.y
    public bool CheckFallDamage(int speed)//возвращает true если ударился сильно
    {
        Debug.Log("FallDamage");
        if (speed >= 0) { Debug.Log("Ничего не делаем на отскоке"); return false; }
        else
        {
            Debug.Log("speed < 0:");
            //обрабатывает только отрицательные (падение)
            if (speed < Constants.MAX_SPEED_VECTOR_VELOCITY_NO_DAMAGE)     //меньше порога ущерба
            {
                Debug.Log("speed < Constants.MAX_HEIGHT_VECTOR_VELOCITY_NO_DAMAGE");
                Debug.Log("speed negativ^ " + speed);
                int s = -speed;////отрицательное в положительное
                Debug.Log("speed positiv^ " + s);
                //считаем урон, переводим в положительный!
                int damage = (s - (Constants.MAX_SPEED_VECTOR_VELOCITY_NO_DAMAGE) * (-1));//отрицательное в положительное
                                                                                          //отнимаем его
                                                                                          //Обязательно проверка на положительное значение damage
                MinusHealth(damage * 3);
                return true;
            }
        }
        return false;
    }
}
