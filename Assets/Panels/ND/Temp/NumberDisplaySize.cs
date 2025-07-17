using UnityEngine;
using UnityEngine.UI;

public class NumberDisplayController : MonoBehaviour
{
    public Image imageComponent;
    private UIImageSwitcher mfdMoodScript;
    private CanvasGroup canvasGroup;

    // 数字图片数组
    public Sprite[] numberSprites;

    // 数值的当前值，最小值为5
    public static int currentValue = 5;

    // 显示位数枚举
    public enum DisplayDigit
    {
        Ones,   // 个位
        Tens,   // 十位
        Hundreds // 百位
    }

    // 设置显示哪一位
    public DisplayDigit digitToDisplay;
    
    // 添加绑定模式枚举
    [System.Flags]
    public enum BindMode
    {
        None = 0,
        Sprite0 = 1,    // 2^0
        Sprite1 = 2,    // 2^1
        Sprite2 = 4,    // 2^2
        Sprite3 = 8     // 2^3
    }

    // 设置绑定模式，支持多选
    [SerializeField]
    private BindMode bindModes = BindMode.Sprite1;

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

    // ������ز��������ڸ�λ����ʾ�����ʹ�ã�
    private float holdStartTime = 0f;
    private float holdDelay = 0.5f;
    private float holdInterval = 0.1f;
    private float nextHoldActionTime = 0f;

    void Update()
    {
        if (mfdMoodScript != null)
        {
            UpdateVisibility();
        }

        // ֻ�ڸ�λ����ʾ����ϴ�������
        if (digitToDisplay == DisplayDigit.Ones)
        {
            HandleInput();
        }

        UpdateDigitDisplay();
    }

    private void HandleInput()
    {
        // �ϼ�ͷ�������
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            holdStartTime = Time.time;
            currentValue = Mathf.Min(currentValue + 5, 360);
            Debug.Log("Up Arrow pressed, current value: " + currentValue);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
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

        // �¼�ͷ�������
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            holdStartTime = Time.time;
            currentValue = Mathf.Max(currentValue - 5, 5);
            Debug.Log("Down Arrow pressed, current value: " + currentValue);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
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
    }

    private void UpdateVisibility()
    {
        bool shouldShow = false;
        
        // 检查是否是单一绑定模式
        if (bindModes == BindMode.None)
        {
            shouldShow = false;
        }
        else if (bindModes == BindMode.Sprite0)
        {
            shouldShow = mfdMoodScript.IsShowingSprite0();
        }
        else if (bindModes == BindMode.Sprite1)
        {
            shouldShow = mfdMoodScript.IsShowingSprite1();
        }
        else if (bindModes == BindMode.Sprite2)
        {
            shouldShow = mfdMoodScript.IsShowingSprite2();
        }
        else if (bindModes == BindMode.Sprite3)
        {
            shouldShow = mfdMoodScript.IsShowingSprite3();
        }
        else
        {
            // 处理多个绑定模式的情况
            if ((bindModes & BindMode.Sprite0) != 0 && mfdMoodScript.IsShowingSprite0())
            {
                shouldShow = true;
            }
            
            if ((bindModes & BindMode.Sprite1) != 0 && mfdMoodScript.IsShowingSprite1())
            {
                shouldShow = true;
            }
            
            if ((bindModes & BindMode.Sprite2) != 0 && mfdMoodScript.IsShowingSprite2())
            {
                shouldShow = true;
            }
            
            if ((bindModes & BindMode.Sprite3) != 0 && mfdMoodScript.IsShowingSprite3())
            {
                shouldShow = true;
            }
        }

        canvasGroup.alpha = shouldShow ? 1 : 0;
        canvasGroup.blocksRaycasts = shouldShow;
    }

    private void UpdateDigitDisplay()
    {
        int value = currentValue;
        int digit = 0;
        int spriteIndex = 0;
        bool shouldShow = true;

        switch (digitToDisplay)
        {
            case DisplayDigit.Ones:
                // ��ȡ��λ����
                digit = value % 10;
                // ������Сֵ��5�����Ը�λֻ����0��5
                spriteIndex = (digit == 0) ? 0 : 1;
                break;

            case DisplayDigit.Tens:
                // ���ֵС��10������ʾʮλ
                if (value < 10)
                {
                    canvasGroup.alpha = 0;
                    return;
                }
                // ��ȡʮλ����
                digit = (value / 10) % 10;
                spriteIndex = digit;
                break;

            case DisplayDigit.Hundreds:
                // ���ֵС��100������ʾ��λ
                if (value < 100)
                {
                    canvasGroup.alpha = 0;
                    return;
                }
                // ��ȡ��λ����
                digit = (value / 100);
                // ��λֻ��1-5������������Ҫ��1
                spriteIndex = digit - 1;
                break;
        }

        if (spriteIndex >= 0 && spriteIndex < numberSprites.Length && numberSprites[spriteIndex] != null)
        {
            imageComponent.sprite = numberSprites[spriteIndex];
            // 使用更新后的可见性检查
            bool isVisible = false;
            if (mfdMoodScript != null)
            {
                // 检查当前绑定模式下是否应该显示
                if (bindModes == BindMode.None)
                {
                    isVisible = false;
                }
                else if (bindModes == BindMode.Sprite0)
                {
                    isVisible = mfdMoodScript.IsShowingSprite0();
                }
                else if (bindModes == BindMode.Sprite1)
                {
                    isVisible = mfdMoodScript.IsShowingSprite1();
                }
                else if (bindModes == BindMode.Sprite2)
                {
                    isVisible = mfdMoodScript.IsShowingSprite2();
                }
                else if (bindModes == BindMode.Sprite3)
                {
                    isVisible = mfdMoodScript.IsShowingSprite3();
                }
                else
                {
                    // 多选模式
                    isVisible = ((bindModes & BindMode.Sprite0) != 0 && mfdMoodScript.IsShowingSprite0()) ||
                                ((bindModes & BindMode.Sprite1) != 0 && mfdMoodScript.IsShowingSprite1()) ||
                                ((bindModes & BindMode.Sprite2) != 0 && mfdMoodScript.IsShowingSprite2()) ||
                                ((bindModes & BindMode.Sprite3) != 0 && mfdMoodScript.IsShowingSprite3());
                }
            }
            canvasGroup.alpha = isVisible ? 1 : 0;
        }
    }
}