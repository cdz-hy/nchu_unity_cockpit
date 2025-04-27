using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BEI_nav : MonoBehaviour
{
    public Image imageComponent;
    private UIImageSwitcher mfdMoodScript;
    private CanvasGroup canvasGroup;

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
        
        // 在Start中立即检查并设置初始状态
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

    // 抽取显示逻辑到单独的方法
    private void UpdateVisibility()
    {
        canvasGroup.alpha = mfdMoodScript.IsShowingSprite3() ? 1 : 0;
        canvasGroup.blocksRaycasts = mfdMoodScript.IsShowingSprite3();
    }
}
