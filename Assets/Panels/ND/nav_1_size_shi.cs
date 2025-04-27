using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class nav_1_size_shi : MonoBehaviour
{
    public Image imageComponent;
    private UIImageSwitcher mfdMoodScript;
    private CanvasGroup canvasGroup;
    
    // 0-9的数字图片
    public Sprite[] numberSprites = new Sprite[10];

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
        
        // 如果值小于10，不显示十位
        if (value < 10)
        {
            canvasGroup.alpha = 0;
            return;
        }
        
        // 获取十位数字
        int digit = (value / 10) % 10;
        
        if (digit >= 0 && digit < numberSprites.Length && numberSprites[digit] != null)
        {
            imageComponent.sprite = numberSprites[digit];
            canvasGroup.alpha = mfdMoodScript.IsShowingSprite1() ? 1 : 0;
        }
    }
}
