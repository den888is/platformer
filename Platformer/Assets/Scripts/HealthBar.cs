using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//������ �������� �� �������� ��������� � ��� �����������
public class HealthBar : MonoBehaviour
{
    public Image healthBar;
    public float currentHealth;

    private float maxHealth = 100f;
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
        // currentHealth = GetComponent<HealthBar>().currentHealth;
        Debug.Log("HealthBar Update");
        healthBar.fillAmount = currentHealth / maxHealth;
        Debug.Log("healthBar.fillAmount = " + healthBar.fillAmount);
    }
    void UpdateHealthBar()//���������� ��� ���������� ����� ��������
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }
    //��������� ������ ������������� ��������
    public void MinusHealth(int h)
    {
        if (h <= 0) { Debug.LogError("������ �������������"); }
        else
        if (!immunity)
        {
            immunity = true;//���. ���������
            currentHealth -= h;
            if (currentHealth <= 0)
            {
                //����� ��������
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            //��������� ���������� ��������
            StartCoroutine(StartTimer());
        }
    }
    public void PlusKitHealth(GameObject g)
    {
        currentHealth = maxHealth;//���������� ��������
        Destroy(g);//������ ������
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
    //��������� ����� �������� velocity.y
    public bool CheckFallDamage(int speed)//���������� true ���� �������� ������
    {
        Debug.Log("FallDamage");
        if (speed >= 0) { Debug.Log("������ �� ������ �� �������"); return false; }
        else
        {
            Debug.Log("speed < 0:");
            //������������ ������ ������������� (�������)
            if (speed < Constants.MAX_SPEED_VECTOR_VELOCITY_NO_DAMAGE)     //������ ������ ������
            {
                Debug.Log("speed < Constants.MAX_HEIGHT_VECTOR_VELOCITY_NO_DAMAGE");
                Debug.Log("speed negativ^ " + speed);
                int s = -speed;////������������� � �������������
                Debug.Log("speed positiv^ " + s);
                //������� ����, ��������� � �������������!
                int damage = (s - (Constants.MAX_SPEED_VECTOR_VELOCITY_NO_DAMAGE) * (-1));//������������� � �������������
                                                                                          //�������� ���
                                                                                          //����������� �������� �� ������������� �������� damage
                MinusHealth(damage * 3);
                return true;
            }
        }
        return false;
    }
}
