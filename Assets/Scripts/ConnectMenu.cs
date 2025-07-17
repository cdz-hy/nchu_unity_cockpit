using UnityEngine;
using UnityEngine.UI;
using TMPro; // 添加TextMeshPro命名空间

public class InputDialogManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialogPanel;              // 弹窗面板
    public TMP_InputField inputField;           // TextMeshPro输入框
    public Button confirmButton;                // 确定按钮
    public Button cancelButton;                 // 取消按钮
    public Button triggerButton;                // 触发弹窗的按钮

    [Header("Target Script")]
    public DataSend targetScript;           // 要修改变量的目标脚本

    void Start()
    {
        // 初始化时隐藏弹窗
        dialogPanel.SetActive(false);

        // 绑定按钮事件
        triggerButton.onClick.AddListener(ShowDialog);
        confirmButton.onClick.AddListener(OnConfirm);
        cancelButton.onClick.AddListener(OnCancel);

        // 绑定输入框事件（可选）
        inputField.onEndEdit.AddListener(OnInputEndEdit);
    }

    // 显示弹窗
    public void ShowDialog()
    {
        dialogPanel.SetActive(true);
        inputField.text = ""; // 清空输入框
        inputField.Select(); // 选中输入框
        inputField.ActivateInputField(); // 激活输入框焦点
    }

    // 确定按钮点击事件
    void OnConfirm()
    {
        string inputText = inputField.text;

        if (!string.IsNullOrEmpty(inputText))
        {
            // 将输入的文本传递给目标脚本
            if (targetScript != null)
            {
                targetScript.UpdateVariable(inputText);
            }

            Debug.Log("输入的信息: " + inputText);
        }
        else
        {
            Debug.LogWarning("输入框为空，请输入有效信息！");
            return; // 如果输入为空，不关闭弹窗
        }

        HideDialog();
    }

    // 取消按钮点击事件
    void OnCancel()
    {
        HideDialog();
    }

    // 输入框结束编辑事件（按Enter键时触发）
    void OnInputEndEdit(string text)
    {
        // 检查是否按下了Enter键
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnConfirm();
        }
    }

    // 隐藏弹窗
    void HideDialog()
    {
        dialogPanel.SetActive(false);
        inputField.DeactivateInputField(); // 取消输入框焦点
    }

    void Update()
    {
        // 只有在弹窗显示时才处理快捷键
        if (dialogPanel.activeInHierarchy)
        {
            // 按ESC键关闭弹窗
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnCancel();
            }
        }
    }

    // 公共方法：外部调用显示弹窗
    public void ShowDialogWithPlaceholder(string placeholder = "请输入信息...")
    {
        ShowDialog();
        inputField.placeholder.GetComponent<TextMeshProUGUI>().text = placeholder;
    }
}