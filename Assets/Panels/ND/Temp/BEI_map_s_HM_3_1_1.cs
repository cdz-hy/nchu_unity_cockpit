using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BEI_map_s_HM_3_1_1 : MonoBehaviour
{
    private float currentAngle = 0f;
    private Image imageComponent;
    private UIImageSwitcher mfdMoodScript;
    private CanvasGroup canvasGroup;

    // 添加旋转控制相关变量
    private float rotationSpeed = 1f;        // 每次旋转的角度
    private float holdRotationSpeed = 100f;  // 长按时的旋转速度（度/秒）
    private float holdDelay = 0.5f;         // 长按判定延迟
    private float holdStartTime = 0f;        // 开始按住的时间

    void Start()
    {
        // 初始化UI组件
        imageComponent = GetComponent<Image>();
        if (imageComponent == null)
        {
            Debug.LogError($"[{gameObject.name}] 未找到Image组件");
            return;
        }

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
            Debug.Log($"[{gameObject.name}] 自动添加了CanvasGroup组件");
        }

        // 确保CanvasGroup初始值正确
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

        mfdMoodScript = FindObjectOfType<UIImageSwitcher>();
        if (mfdMoodScript == null)
        {
            Debug.LogError($"[{gameObject.name}] 未找到UIImageSwitcher组件");
            return;
        }

        // 记录初始旋转角度
        currentAngle = transform.eulerAngles.z;
    }

    void Update()
    {
        // 处理左旋转（逗号键）
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            // 单次按下，立即旋转一度
            currentAngle -= rotationSpeed;
            holdStartTime = Time.time;
        }
        else if (Input.GetKey(KeyCode.Comma))
        {
            // 长按判定
            if (Time.time - holdStartTime >= holdDelay)
            {
                // 持续旋转
                currentAngle -= holdRotationSpeed * Time.deltaTime;
            }
        }

        // 处理右旋转（句号键）
        if (Input.GetKeyDown(KeyCode.Period))
        {
            // 单次按下，立即旋转一度
            currentAngle += rotationSpeed;
            holdStartTime = Time.time;
        }
        else if (Input.GetKey(KeyCode.Period))
        {
            // 长按判定
            if (Time.time - holdStartTime >= holdDelay)
            {
                // 持续旋转
                currentAngle += holdRotationSpeed * Time.deltaTime;
            }
        }

        // 直接应用旋转
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
        
        // 更新可见性
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        if (mfdMoodScript != null)
        {
            bool shouldShow = false;
            
            // 检查所有可能的sprite状态
            if (mfdMoodScript.IsShowingSprite0() || 
                mfdMoodScript.IsShowingSprite1() || 
                mfdMoodScript.IsShowingSprite2() || 
                mfdMoodScript.IsShowingSprite3())
            {
                shouldShow = true;
            }
            
            if (canvasGroup != null)
            {
                canvasGroup.alpha = shouldShow ? 1f : 0f;
                canvasGroup.blocksRaycasts = shouldShow;
            }
            else
            {
                Debug.LogError($"[{gameObject.name}] CanvasGroup组件丢失！");
            }
        }
        else
        {
            Debug.LogWarning($"[{gameObject.name}] mfdMoodScript为空！");
        }
    }
}