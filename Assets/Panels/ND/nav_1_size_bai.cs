using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class nav_1_size_bai : MonoBehaviour
{
    public Image imageComponent;
    private UIImageSwitcher mfdMoodScript;
    private CanvasGroup canvasGroup;
    
    // 1-5的数字图片
    public Sprite[] numberSprites = new Sprite[5];

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

        UpdateDigitDisplay();
    }

    private void UpdateVisibility()
    {
        bool shouldShow = mfdMoodScript.IsShowingSprite1();
        canvasGroup.alpha = shouldShow ? 1 : 0;
        canvasGroup.blocksRaycasts = shouldShow;
    }

    private void UpdateDigitDisplay()
    {
        int value = nav_1_size_ge.currentValue;
        
        // 如果值小于100，不显示百位
        if (value < 100)
        {
            canvasGroup.alpha = 0;
            return;
        }
        
        // 获取百位数字
        int digit = (value / 100);
        
        // 百位只有1-5，所以索引需要减1
        int spriteIndex = digit - 1;
        
        if (spriteIndex >= 0 && spriteIndex < numberSprites.Length && numberSprites[spriteIndex] != null)
        {
            imageComponent.sprite = numberSprites[spriteIndex];
            canvasGroup.alpha = mfdMoodScript.IsShowingSprite1() ? 1 : 0;
        }
    }
}
