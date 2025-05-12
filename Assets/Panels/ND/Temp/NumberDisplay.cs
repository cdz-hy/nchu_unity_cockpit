using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberDisplay : MonoBehaviour
{
    public Image imageComponent;
    private UIImageSwitcher mfdMoodScript;
    private CanvasGroup canvasGroup;
    
    // 0-9的数字图片
    public Sprite[] numberSprites = new Sprite[10];
    
    // 当前显示的数值
    public float currentValue = 0f;

    // 显示位数枚举
    public enum DisplayDigit
    {
        Ones,   // 个位
        Tens,   // 十位
        Hundreds // 百位
    }

    // 设置显示哪一位
    public DisplayDigit digitToDisplay;

    // 添加绑定模式枚举
    [System.Flags]
    public enum BindMode
    {
        None = 0,
        Sprite0 = 1,    // 2^0
        Sprite1 = 2,    // 2^1
        Sprite2 = 4,    // 2^2
        Sprite3 = 8     // 2^3
    }

    // 设置绑定模式
    [SerializeField]
    private BindMode bindMode = BindMode.Sprite1;

    void Start()
    {
        if (imageComponent == null)
        {
            imageComponent = GetComponent<Image>();
        }

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
        
        UpdateDigitDisplay();
    }

    void Update()
    {
        if (mfdMoodScript != null)
        {
            UpdateVisibility();
        }
        
        // 从DataCenter获取数值（当前使用临时变量，后续替换）
        // currentValue = DataCenter.Instance.你需要的参数名;
        
        UpdateDigitDisplay();
    }

    private void UpdateVisibility()
    {
        bool shouldShow = false;
        
        // 检查是否是单一绑定模式
        if (bindMode == BindMode.None)
        {
            shouldShow = false;
        }
        else if (bindMode == BindMode.Sprite0)
        {
            shouldShow = mfdMoodScript.IsShowingSprite0();
        }
        else if (bindMode == BindMode.Sprite1)
        {
            shouldShow = mfdMoodScript.IsShowingSprite1();
        }
        else if (bindMode == BindMode.Sprite2)
        {
            shouldShow = mfdMoodScript.IsShowingSprite2();
        }
        else if (bindMode == BindMode.Sprite3)
        {
            shouldShow = mfdMoodScript.IsShowingSprite3();
        }
        else
        {
            // 处理多个绑定模式的情况
            if ((bindMode & BindMode.Sprite0) != 0 && mfdMoodScript.IsShowingSprite0())
            {
                shouldShow = true;
            }
            
            if ((bindMode & BindMode.Sprite1) != 0 && mfdMoodScript.IsShowingSprite1())
            {
                shouldShow = true;
            }
            
            if ((bindMode & BindMode.Sprite2) != 0 && mfdMoodScript.IsShowingSprite2())
            {
                shouldShow = true;
            }
            
            if ((bindMode & BindMode.Sprite3) != 0 && mfdMoodScript.IsShowingSprite3())
            {
                shouldShow = true;
            }
        }

        canvasGroup.alpha = shouldShow ? 1 : 0;
        canvasGroup.blocksRaycasts = shouldShow;
    }
    
    private void UpdateDigitDisplay()
    {
        int value = Mathf.FloorToInt(currentValue);
        int digit = 0;

        // 根据设置的位数获取对应数字
        switch (digitToDisplay)
        {
            case DisplayDigit.Ones:
                digit = value % 10;
                break;
            case DisplayDigit.Tens:
                digit = (value / 10) % 10;
                break;
            case DisplayDigit.Hundreds:
                digit = (value / 100) % 10;
                break;
        }

        // 确保索引在有效范围内
        if (digit >= 0 && digit < numberSprites.Length && numberSprites[digit] != null)
        {
            imageComponent.sprite = numberSprites[digit];
        }
    }
}