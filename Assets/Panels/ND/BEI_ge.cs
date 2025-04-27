using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BEI_ge : MonoBehaviour
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
    }

    void Update()
    {
        if (mfdMoodScript != null)
        {
            UpdateVisibility();
        }
        
        // 从DataCenter获取数值（当前使用临时变量，后续替换）
        // currentValue = DataCenter.Instance.你需要的参数名;
        
        // 更新图片显示
        UpdateDigitDisplay();
    }

    private void UpdateVisibility()
    {
        // 使用IsShowingSprite3()方法来检查状态
        bool isSprite3Showing = mfdMoodScript.IsShowingSprite3();
        canvasGroup.alpha = isSprite3Showing ? 1 : 0;
        canvasGroup.blocksRaycasts = isSprite3Showing;
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
            canvasGroup.alpha = mfdMoodScript.IsShowingSprite3() ? 1 : 0;
        }
    }
}
