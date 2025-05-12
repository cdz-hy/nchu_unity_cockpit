using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class as_scrolling : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float resetYPosition1 = 10f; // 重置位置的Y阈值

    [Header("External Value")]
    public float externalValue1; // 外部脚本修改的数值
    public float airSpeed;

    private Vector3 _initialPosition1;

    void Start()
    {
        //airSpeed = 3;
        _initialPosition1 = transform.localPosition;
    }

    void Update()
    {
        //airSpeed = DataCenter.Instance.AirSpeed;
        //airSpeed+=0.001f;
        float value = airSpeed % 10;
        externalValue1 = value * 0.00449f;

        // 直接使用外部数值控制Y轴位置
        Vector3 newPos = _initialPosition1 - Vector3.up * externalValue1;
        transform.localPosition = newPos;

        // 数值超过阈值时重置（可选逻辑）
        if (value >= resetYPosition1 || value <= -resetYPosition1)
        {
            transform.localPosition = _initialPosition1; // 重置位置
        }
    }
}
