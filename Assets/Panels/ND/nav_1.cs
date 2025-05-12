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
    public float rotationSpeed = 80f;   // 旋转速度（度/秒）
    
    // 临时用于测试的目标角度，后续会被DataCenter的值替换
    public float targetRotation = 110f;
    
    // 添加实时监测相关变量
    public bool enableRealTimeTracking = true;
    private float lastUpdateTime = 0f;
    public float updateInterval = 0.1f; // 更新间隔，秒
    
    // 添加初始旋转值记录
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

        // 记录初始旋转值
        initialRotation = NormalizeAngle(transform.eulerAngles.z);
        currentRotation = initialRotation;

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

        // 实时监测逻辑
        if (enableRealTimeTracking && Time.time - lastUpdateTime >= updateInterval)
        {
            // 从DataCenter获取目标角度（当前使用临时变量，后续替换）
            // float targetRotation = DataCenter.Instance.你需要的参数名;
            
            // 通过左右键，临时测试代码，模拟实时数据变化
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                targetRotation -= 50f * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                targetRotation += 50f * Time.deltaTime;
            }
            
            lastUpdateTime = Time.time;
        }

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

        // 应用旋转，考虑初始旋转值的偏移
        transform.localRotation = Quaternion.Euler(0, 0, currentRotation);
    }

    private void UpdateVisibility()
    {
        // 修改为支持多个sprite绑定
        bool shouldShow = false;
        
        if (mfdMoodScript.IsShowingSprite0() || 
            mfdMoodScript.IsShowingSprite1() || 
            mfdMoodScript.IsShowingSprite2() || 
            mfdMoodScript.IsShowingSprite3())
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
