using UnityEngine;

public class GlobalMenu : MonoBehaviour
{
    public GameObject menuPanel; // ���ò˵�����Panel

    private void Start()
    {
        menuPanel = transform.Find("Panel").gameObject;
        // ȷ���˵�����ʼʱ�����ص�
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }
    }

    private void Update()
    {
        // ���Esc���İ���
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    private void ToggleMenu()
    {
        if (menuPanel != null)
        {
            // �л��˵�������ʾ״̬
            menuPanel.SetActive(!menuPanel.activeSelf);
        }
    }
}