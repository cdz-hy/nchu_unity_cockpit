using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public GameObject targetObject; // 目标物体  
    public Vector3 offset; // 偏移量  
    private Quaternion initialRotation;
    private RectTransform uiTransform; // UI 元素的 RectTransform  

    void Start()
    {
        uiTransform = GetComponent<RectTransform>(); // 获取 UI 元素的 RectTransform  
        
    }

    void Update()
    {
        offset = uiTransform.position - targetObject.transform.localPosition;
        // 如果目标物体存在，则更新 UI 位置  
        if (targetObject != null)
        {
            // 设置 UI 元素的位置为目标物体的位置加上偏移  
            uiTransform.position = targetObject.transform.position + offset;

            // 使 UI 面向主摄像机 (可选，取决于需求)  
            uiTransform.LookAt(Camera.main.transform);
        }
    }
}
