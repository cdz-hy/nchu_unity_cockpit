using UnityEngine;

public class NumberDisplay_hun : MonoBehaviour
{
    public SpriteRenderer numbers;  // Sprite���
    public Sprite[] pic;            // ͼƬ���飨0-9��ʮλ���֣�
    public float airSpeed;    // ��ǰ�ٶ�ֵ
    public float scaleMultiplier = 0.0172f; // ���ű���

    void Start()
    {
        airSpeed = 45;
        numbers = GetComponent<SpriteRenderer>();
        ApplyStaticScale();  // ��ʼ��ʱӦ������
        UpdateDisplay();      // ��ʼ����һ��
    }

    void Update()
    {
        airSpeed = DataCenter.Instance.airSpeed;
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        if (airSpeed > 100)
        {
            // ��ȡʮλ���֣�airSpeed=123 �� 2, airSpeed=5 �� 0��
            int hun = Mathf.FloorToInt(airSpeed / 100) % 10;
            hun = Mathf.Clamp(hun, 0, 9); // ȷ�����鲻Խ��

            // ����ͼƬ
            numbers.sprite = pic[hun];

            // ȷ�����ű���ʼ����Ч
            ApplyStaticScale();
        }
        
    }

    void ApplyStaticScale()
    {
        // ���þ�̬���ű���
        transform.localScale = Vector3.one * scaleMultiplier;
    }
}