using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class nav_2_4 : MonoBehaviour
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

    private void UpdateVisibility()
    {
        // 使用IsShowingSprite2()方法来检查状态
        bool isSprite2Showing = mfdMoodScript.IsShowingSprite2();
        canvasGroup.alpha = isSprite2Showing ? 1 : 0;
        canvasGroup.blocksRaycasts = isSprite2Showing;
    }
}
