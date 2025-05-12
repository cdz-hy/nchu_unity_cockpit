using UnityEngine;
using UnityEngine.UI;

public class SpriteVisibilityController : MonoBehaviour
{
    private UIImageSwitcher mfdMoodScript;
    private CanvasGroup canvasGroup;

    // 使用位掩码来支持多选
    [System.Flags]
    public enum BindMode
    {
        None = 0,
        Sprite0 = 1,    // 2^0
        Sprite1 = 2,    // 2^1
        Sprite2 = 4,    // 2^2
        Sprite3 = 8     // 2^3
    }

    // 设置绑定模式，支持多选
    [SerializeField] // 添加这个特性确保设置会被正确序列化
    private BindMode bindModes = BindMode.None;  // 修改为私有变量

    void Start()
    {
        Debug.Log($"对象 {gameObject.name} 在Start时的绑定模式: {bindModes}");
        
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        mfdMoodScript = FindObjectOfType<UIImageSwitcher>();
        
        if (mfdMoodScript != null)
        {
            UpdateVisibility();
        }
    }

    void Update()
    {
        if (mfdMoodScript != null)
        {
            UpdateVisibility();
        }
    }

    public void UpdateVisibility()
    {
        bool shouldShow = false;
        
        // 添加当前绑定模式的调试日志
        Debug.Log($"对象 {gameObject.name} 的当前绑定模式: {bindModes}");
        
        if (bindModes == BindMode.None)
        {
            shouldShow = false;
            Debug.Log($"对象 {gameObject.name} 绑定模式为None，不显示");
        }
        else
        {
            // 检查是否只选择了一个模式
            if (bindModes == BindMode.Sprite0)
            {
                shouldShow = mfdMoodScript.IsShowingSprite0();
                Debug.Log($"对象 {gameObject.name} 单独绑定Sprite0，显示状态: {shouldShow}");
            }
            else if (bindModes == BindMode.Sprite1)
            {
                shouldShow = mfdMoodScript.IsShowingSprite1();
                Debug.Log($"对象 {gameObject.name} 单独绑定Sprite1，显示状态: {shouldShow}");
            }
            else if (bindModes == BindMode.Sprite2)
            {
                shouldShow = mfdMoodScript.IsShowingSprite2();
                Debug.Log($"对象 {gameObject.name} 单独绑定Sprite2，显示状态: {shouldShow}");
            }
            else if (bindModes == BindMode.Sprite3)
            {
                shouldShow = mfdMoodScript.IsShowingSprite3();
                Debug.Log($"对象 {gameObject.name} 单独绑定Sprite3，显示状态: {shouldShow}");
            }
            else
            {
                Debug.Log($"对象 {gameObject.name} 检测到多个绑定模式");
                // 如果选择了多个模式，则使用位运算检查
                if ((bindModes & BindMode.Sprite0) != 0)
                {
                    bool sprite0Show = mfdMoodScript.IsShowingSprite0();
                    shouldShow |= sprite0Show;
                    Debug.Log($"对象 {gameObject.name} 检查Sprite0，状态: {sprite0Show}");
                }
                
                if ((bindModes & BindMode.Sprite1) != 0)
                {
                    bool sprite1Show = mfdMoodScript.IsShowingSprite1();
                    shouldShow |= sprite1Show;
                    Debug.Log($"对象 {gameObject.name} 检查Sprite1，状态: {sprite1Show}");
                }
                
                if ((bindModes & BindMode.Sprite2) != 0)
                {
                    bool sprite2Show = mfdMoodScript.IsShowingSprite2();
                    shouldShow |= sprite2Show;
                    Debug.Log($"对象 {gameObject.name} 检查Sprite2，状态: {sprite2Show}");
                }
                
                if ((bindModes & BindMode.Sprite3) != 0)
                {
                    bool sprite3Show = mfdMoodScript.IsShowingSprite3();
                    shouldShow |= sprite3Show;
                    Debug.Log($"对象 {gameObject.name} 检查Sprite3，状态: {sprite3Show}");
                }
            }
        }

        Debug.Log($"对象 {gameObject.name} 最终显示状态: {shouldShow}");
        canvasGroup.alpha = shouldShow ? 1 : 0;
        canvasGroup.blocksRaycasts = shouldShow;
    }
}