using UnityEngine;

public class GlobalMenu : MonoBehaviour
{
    public GameObject menuPanel; // 引用菜单条的Panel

    private void Start()
    {
        menuPanel = transform.Find("Panel").gameObject;
        // 确保菜单条初始时是隐藏的
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }
    }

    private void Update()
    {
        // 检测Esc键的按下
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    private void ToggleMenu()
    {
        if (menuPanel != null)
        {
            // 切换菜单条的显示状态
            menuPanel.SetActive(!menuPanel.activeSelf);
        }
    }
}