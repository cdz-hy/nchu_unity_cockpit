using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class nav_1 : MonoBehaviour
{
    public Image imageComponent;
    private UIImageSwitcher mfdMoodScript;
    private CanvasGroup canvasGroup;

    private float currentRotation = 0f; // 当前旋转角度
    public float rotationSpeed = 50f;   // 旋转速度（度/秒）
    
    // 临时用于测试的目标角度，后续会被DataCenter的值替换
    public float targetRotation = 80f;
    
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

        // 初始化时就进行归一化
        currentRotation = NormalizeAngle(transform.eulerAngles.z);

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

        // 从DataCenter获取目标角度（当前使用临时变量，后续替换）
        // float targetRotation = DataCenter.Instance.你需要的参数名;
        
        // 确保目标角度在0-360范围内
        float normalizedTarget = NormalizeAngle(targetRotation);
        float normalizedCurrent = NormalizeAngle(currentRotation);
        
        // 计算当前角度到目标角度的最短路径
        float angleDifference = Mathf.DeltaAngle(normalizedCurrent, normalizedTarget);

        // 平滑旋转到目标角度
        if (Mathf.Abs(angleDifference) > 0.1f)
        {
            float rotationThisFrame = rotationSpeed * Time.deltaTime;
            if (Mathf.Abs(angleDifference) <= rotationThisFrame)
            {
                currentRotation = normalizedTarget;
            }
            else
            {
                float newRotation = normalizedCurrent + Mathf.Sign(angleDifference) * rotationThisFrame;
                currentRotation = NormalizeAngle(newRotation);
            }
        }
        else
        {
            currentRotation = normalizedTarget;
        }

        // 直接使用currentRotation，因为它已经被归一化了
        transform.localRotation = Quaternion.Euler(0, 0, currentRotation);
    }

    private void UpdateVisibility()
    {
        canvasGroup.alpha = mfdMoodScript.IsShowingSprite1() ? 1 : 0;
        canvasGroup.blocksRaycasts = mfdMoodScript.IsShowingSprite1();
    }

    // 角度归一化方法
    private float NormalizeAngle(float angle)
    {
        return ((angle % 360) + 360) % 360;
    }
}
