using UnityEngine;

public class Scrolling : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float resetYPosition = 100f; // 重置位置的Y阈值

    [Header("External Value")]
    public float externalValue; // 外部脚本修改的数值
    public float height;

    private Vector3 _initialPosition;

    void Start()
    {
        height = 10000;
        _initialPosition = transform.localPosition;
    }

    void Update()
    {
        height-=1;
        float value = height % 100;
        externalValue = value * 0.0001467f;

        // 直接使用外部数值控制Y轴位置
        Vector3 newPos = _initialPosition + Vector3.up * externalValue;
        transform.localPosition = newPos;

        // 数值超过阈值时重置（可选逻辑）
        if (value >= resetYPosition || value <= -resetYPosition)
        {
            transform.localPosition = _initialPosition; // 重置位置
        }
    }
}