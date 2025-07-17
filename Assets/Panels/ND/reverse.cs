using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class reverse : MonoBehaviour
{
    private Image imageComponent;

    void Start()
    {
        // 获取Image组件
        imageComponent = GetComponent<Image>();
        if (imageComponent == null)
        {
            Debug.LogError("未找到Image组件！");
            return;
        }

        // 在Start中直接执行镜面翻转
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }

    void Update()
    {
        // 空的Update方法保留以备后续需要添加其他功能
    }
}
