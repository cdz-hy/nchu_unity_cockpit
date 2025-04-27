using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class nav_pointer : MonoBehaviour
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

        // ��ȡ������ CanvasGroup ���
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        mfdMoodScript = FindObjectOfType<UIImageSwitcher>();
    }

    void Update()
    {
        if (mfdMoodScript != null)
        {
            // ʹ�� CanvasGroup �� alpha ������͸����
            canvasGroup.alpha = mfdMoodScript.IsShowingSprite1() ? 1 : 0;
            // ��͸��ʱ�������߼�⣬�����Ͳ����ڵ�����UIԪ��
            canvasGroup.blocksRaycasts = mfdMoodScript.IsShowingSprite1();
        }
    }
}
