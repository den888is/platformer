using UnityEngine;
using UnityEngine.UI;

public class KeysIndicator : MonoBehaviour
{
    public int keyCount;//���������� ��������� ������
    GameObject keysIndicator;
    public GameObject[] keys;
    void Start()
    {
        keys = new GameObject[5];
        InitiateKeys(keys);
    }
    //�������������� ������
    private void InitiateKeys(GameObject[] keys)
    {
        keysIndicator = GameObject.Find("KeysIndicator");
        if (keysIndicator != null)
        {
            int i = 0;
            foreach (Transform child in transform)
            {

                Debug.Log(child.name);
                keys[i] = child.gameObject;
                i++;
            }
            foreach (GameObject obj in keys)
            {
                obj.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
            }
        }
    }
    //�������� ����, ���������� � ����������
    public void KeyCountPlus()
    {
        keys[keyCount].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        keyCount++;
    }
}
