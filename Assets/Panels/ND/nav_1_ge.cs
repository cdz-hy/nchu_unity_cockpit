using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class nav_1_ge : MonoBehaviour
{
    public Image imageComponent;
    private UIImageSwitcher mfdMoodScript;
    private CanvasGroup canvasGroup;
    
    // 0-9的数字图片
    public Sprite[] numberSprites = new Sprite[10];
    
    // 当前显示的数值
    public float currentValue = 0f;

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
        
        // 初始更新图片
        UpdateDigitDisplay();
    }

    void Update()
    {
        if (mfdMoodScript != null)
        {
            UpdateVisibility();
        }
        
        // 从DataCenter获取数值（当前使用临时变量，后续替换）
        currentValue = DataCenter.Instance.rotationAngle;
        
        // 更新图片显示
        UpdateDigitDisplay();
    }

    private void UpdateVisibility()
    {
        // 只在显示sprite1时显示
        canvasGroup.alpha = mfdMoodScript.IsShowingSprite1() ? 1 : 0;
        canvasGroup.blocksRaycasts = mfdMoodScript.IsShowingSprite1();
    }
    
    private void UpdateDigitDisplay()
    {
        // 获取个位数字
        int value = Mathf.FloorToInt(currentValue);
        int digit = value % 10;
        
        // 确保索引在有效范围内
        if (digit >= 0 && digit < numberSprites.Length && numberSprites[digit] != null)
        {
            imageComponent.sprite = numberSprites[digit];
        }
    }
}
