using UnityEngine;
using UnityEngine.UI;

public class FF_Number_display : MonoBehaviour
{
    public Image imageComponent;
    public enum DigitType
    {
        Ones,       // 个位
        Decimals,   // 十分位
        Centesimals // 百分位
    }

    public DigitType digitType;  // 当前数字代表的位数
    
                                     // 当前显示的数值
    public float currentValue = 0f;

    // 0-9的数字图片数组
    [SerializeField]
    private Sprite[] numberSprites = new Sprite[10];

    void Start()
    {
        if (imageComponent == null)
        {
            imageComponent = GetComponent<Image>();
        }

        // 验证图片数组大小
        if (numberSprites == null || numberSprites.Length != 10)
        {
            Debug.LogError($"数字图片数组大小不正确！需要10个图片（0-9）！");
        }
    }

    void Update()
    {
        
        // 从DataCenter获取数值（当前使用临时变量，后续替换）
        currentValue = DataCenter.Instance.FF;
        int displayNumber = CalculateDisplayNumber(currentValue);
        UpdateDigitDisplay(displayNumber);
    }

    private void UpdateDigitDisplay(int displayNumber)
    {
        if (displayNumber == -1)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            if (imageComponent != null && numberSprites != null)
            {
                if (displayNumber >= 0 && displayNumber < numberSprites.Length && numberSprites[displayNumber] != null)
                {
                    imageComponent.sprite = numberSprites[displayNumber];
                }
            }
        }
    }

    private int CalculateDisplayNumber(float value)
    {
        // 使用四舍五入来避免浮点数精度问题
        float roundedValue = Mathf.Round(value * 100) / 100f;
        int intPart = Mathf.FloorToInt(roundedValue);
        float decimalPart = roundedValue - intPart;

        switch (digitType)
        {
            case DigitType.Ones:
                return intPart % 10;

            case DigitType.Decimals:
                // 使用四舍五入来获取十分位
                return Mathf.RoundToInt(decimalPart * 10) % 10;

            case DigitType.Centesimals:
                // 使用四舍五入来获取百分位
                return Mathf.RoundToInt(decimalPart * 100) % 10;

            default:
                return 0;
        }
    }
}
