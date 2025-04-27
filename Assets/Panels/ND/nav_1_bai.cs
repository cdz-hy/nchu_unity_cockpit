using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class nav_1_bai : MonoBehaviour
{
    public Image imageComponent;
    private UIImageSwitcher mfdMoodScript;
    private CanvasGroup canvasGroup;
    
    // 1-9的数字图片（百位只有1-9）
    public Sprite[] numberSprites = new Sprite[3];
    
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
        // 获取百位数字
        int value = Mathf.FloorToInt(currentValue);
        
        // 如果值小于100，则不显示百位
        if (value < 100)
        {
            canvasGroup.alpha = 0;
            return;
        }
        
        int digit = (value / 100) % 10;
        
        // 百位只有1-9，所以索引需要减1
        int spriteIndex = digit - 1;
        
        // 确保索引在有效范围内
        if (spriteIndex >= 0 && spriteIndex < numberSprites.Length && numberSprites[spriteIndex] != null)
        {
            imageComponent.sprite = numberSprites[spriteIndex];
            canvasGroup.alpha = mfdMoodScript.IsShowingSprite1() ? 1 : 0;
        }
    }
}
