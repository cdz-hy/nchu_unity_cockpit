using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Number_display : MonoBehaviour
{
    public Image imageComponent;
    public enum DigitType
    {
        Hundreds,    // 百位
        Tens,       // 十位
        Ones,       // 个位
        Decimals    // 十分位
    }

    public DigitType digitType;  // 当前数字代表的位数
    
    // 当前显示的数值
    public float currentValue = 0f;

    // 根据位数类型设置不同大小的数组
    [SerializeField]
    private Sprite[] numberSprites;  // 百位1-9，其他位0-9
    
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        if (imageComponent == null)
        {
            imageComponent = GetComponent<Image>();
        }

        // 验证图片数组大小
        int requiredSize = (digitType == DigitType.Hundreds) ? 9 : 10;
        if (numberSprites == null || numberSprites.Length != requiredSize)
        {
            Debug.LogError($"数字图片数组大小不正确！{digitType}位需要{requiredSize}个图片！");
        }
    }

    void Update()
    {
        
        // 从DataCenter获取数值（当前使用临时变量，后续替换）
        currentValue = DataCenter.Instance.N2_left;
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
                // 确保索引在有效范围内
                int spriteIndex = (digitType == DigitType.Hundreds && displayNumber > 0) ? displayNumber - 1 : displayNumber;
                if (spriteIndex >= 0 && spriteIndex < numberSprites.Length && numberSprites[spriteIndex] != null)
                {
                    imageComponent.sprite = numberSprites[spriteIndex];
                }
            }
        }
    }

    private int CalculateDisplayNumber(float value)
    {
        int intPart = Mathf.FloorToInt(value);
        int decimalPart = Mathf.FloorToInt((value - intPart) * 10);

        switch (digitType)
        {
            case DigitType.Hundreds:
                int hundreds = (intPart / 100) % 10;
                // 百位只显示1-9，且只有当值大于等于100时才显示
                return (value >= 100f && hundreds > 0) ? hundreds : -1;

            case DigitType.Tens:
                int tens = (intPart / 10) % 10;
                // 只有当值大于等于10时才显示十位
                return (value >= 10f) ? tens : -1;

            case DigitType.Ones:
                // 个位始终显示
                return intPart % 10;

            case DigitType.Decimals:
                // 十分位始终显示
                return decimalPart;

            default:
                return 0;
        }
    }
}
