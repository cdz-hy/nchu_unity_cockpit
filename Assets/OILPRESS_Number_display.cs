using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OILPRESS_Number_display : MonoBehaviour
{
    public Image imageComponent;
    public enum DigitType
    {
        Hundreds,    // ��λ
        Tens,       // ʮλ
        Ones,       // ��λ
        Decimals    // ʮ��λ
    }

    public DigitType digitType;  // ��ǰ���ִ�����λ��
    
    // ��ǰ��ʾ����ֵ
    public float currentValue = 0f;

    // ����λ���������ò�ͬ��С������
    [SerializeField]
    private Sprite[] numberSprites;  // ��λ1-9������λ0-9

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        if (imageComponent == null)
        {
            imageComponent = GetComponent<Image>();
        }

        // ��֤ͼƬ�����С
        int requiredSize = (digitType == DigitType.Hundreds) ? 9 : 10;
        if (numberSprites == null || numberSprites.Length != requiredSize)
        {
            Debug.LogError($"����ͼƬ�����С����ȷ��{digitType}λ��Ҫ{requiredSize}��ͼƬ��");
        }
    }

    void Update()
    {
        
        // ��DataCenter��ȡ��ֵ����ǰʹ����ʱ�����������滻��
        currentValue = DataCenter.Instance.OILPRESS;
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
                // ȷ����������Ч��Χ��
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
                // ��λֻ��ʾ1-9����ֻ�е�ֵ���ڵ���100ʱ����ʾ
                return (value >= 100f && hundreds > 0) ? hundreds : -1;

            case DigitType.Tens:
                int tens = (intPart / 10) % 10;
                // ֻ�е�ֵ���ڵ���10ʱ����ʾʮλ
                return (value >= 10f) ? tens : -1;

            case DigitType.Ones:
                // ��λʼ����ʾ
                return intPart % 10;

            case DigitType.Decimals:
                // ʮ��λʼ����ʾ
                return decimalPart;

            default:
                return 0;
        }
    }
}
