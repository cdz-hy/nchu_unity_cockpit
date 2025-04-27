using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class nav_1_size_ge : MonoBehaviour
{
    public Image imageComponent;
    private UIImageSwitcher mfdMoodScript;
    private CanvasGroup canvasGroup;
    
    // 0和5两个图片
    public Sprite[] numberSprites = new Sprite[2];
    
    // 共享的当前值，最小值设为5
    public static int currentValue = 5;

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

    // 长按相关参数
    private float holdStartTime = 0f;        // 开始长按的时间
    private float holdDelay = 0.5f;          // 开始识别为长按的时间
    private float holdInterval = 0.1f;       // 长按时数值变化的间隔
    private float nextHoldActionTime = 0f;   // 下一次长按动作的时间

    void Update()
    {
        if (mfdMoodScript != null)
        {
            UpdateVisibility();
        }

        // 上箭头按键检测
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            holdStartTime = Time.time;
            currentValue = Mathf.Min(currentValue + 5, 360);
            Debug.Log("Up Arrow pressed, current value: " + currentValue);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            // 长按检测
            if (Time.time - holdStartTime >= holdDelay)
            {
                if (Time.time >= nextHoldActionTime)
                {
                    currentValue = Mathf.Min(currentValue + 5, 360);
                    nextHoldActionTime = Time.time + holdInterval;
                    Debug.Log("Up Arrow holding, current value: " + currentValue);
                }
            }
        }

        // 下箭头按键检测
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            holdStartTime = Time.time;
            currentValue = Mathf.Max(currentValue - 5, 5);
            Debug.Log("Down Arrow pressed, current value: " + currentValue);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            // 长按检测
            if (Time.time - holdStartTime >= holdDelay)
            {
                if (Time.time >= nextHoldActionTime)
                {
                    currentValue = Mathf.Max(currentValue - 5, 5);
                    nextHoldActionTime = Time.time + holdInterval;
                    Debug.Log("Down Arrow holding, current value: " + currentValue);
                }
            }
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
        // 获取个位数字
        int digit = currentValue % 10;
        
        // 由于最小值是5，所以个位只会是0或5
        int spriteIndex = (digit == 0) ? 0 : 1;
        
        if (spriteIndex >= 0 && spriteIndex < numberSprites.Length && numberSprites[spriteIndex] != null)
        {
            imageComponent.sprite = numberSprites[spriteIndex];
        }
    }
}
