using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class nav_0 : MonoBehaviour
{
    public Image imageComponent;
    private UIImageSwitcher mfdMoodScript;
    private CanvasGroup canvasGroup;

    // 引用nav_1脚本
    public nav_1 referenceNav;

    // 记录初始相对角度
    private float relativeAngle = 0f;
    private float initialRotation = 0f;

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

        // 如果没有指定referenceNav，尝试查找
        if (referenceNav == null)
        {
            referenceNav = FindObjectOfType<nav_1>();
        }

        // 记录初始旋转值
        initialRotation = NormalizeAngle(transform.eulerAngles.z);

        // 计算与nav_1的相对角度
        if (referenceNav != null)
        {
            float nav1Rotation = NormalizeAngle(referenceNav.transform.eulerAngles.z);
            relativeAngle = NormalizeAngle(initialRotation - nav1Rotation);
            Debug.Log($"初始相对角度: {relativeAngle}");
        }

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

        // 跟随nav_1旋转，但保持相对角度不变
        if (referenceNav != null)
        {
            float nav1Rotation = NormalizeAngle(referenceNav.transform.eulerAngles.z);
            float newRotation = NormalizeAngle(nav1Rotation + relativeAngle);
            transform.localRotation = Quaternion.Euler(0, 0, newRotation);
        }
    }

    private void UpdateVisibility()
    {
        // 修改为只在特定sprite时显示
        bool shouldShow = false;

        // 根据你的需求，设置这个对象应该在哪些sprite状态下显示
        // 例如：只在sprite0和sprite1时显示
        if (mfdMoodScript.IsShowingSprite0() ||
            mfdMoodScript.IsShowingSprite1())
        {
            shouldShow = true;
        }

        canvasGroup.alpha = shouldShow ? 1 : 0;
        canvasGroup.blocksRaycasts = shouldShow;
    }

    // 角度归一化方法
    private float NormalizeAngle(float angle)
    {
        return ((angle % 360) + 360) % 360;
    }
}
