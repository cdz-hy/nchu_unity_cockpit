using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BEI_nav : MonoBehaviour
{
    private UIImageSwitcher mfdMoodScript;
    private CanvasGroup canvasGroup;

    void Start()
    {
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
        canvasGroup.alpha = mfdMoodScript.IsShowingSprite3() ? 1 : 0;
        canvasGroup.blocksRaycasts = mfdMoodScript.IsShowingSprite3();
    }
}
