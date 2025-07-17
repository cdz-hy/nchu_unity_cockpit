using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class doorL1Nob : MonoBehaviour
{
    //private float targetRotationX = 169.831f; // 目标旋转角度  
    private float speed = 150f; // 旋转速度  
    private Quaternion targetRotation; // 目标四元数旋转  
    public bool isDoorOpened = false;
    public bool shift = false;
    private Quaternion initialRotation;

    // Start is called before the first frame update  
    void Start()
    {
        initialRotation = transform.localRotation;
        // 设置目标四元数旋转，只改变X轴旋转，保持其他轴不变  
        targetRotation = initialRotation * Quaternion.Euler(-180, 0, 0); // 设置目标旋转  
        isDoorOpened = false;
    }

    // Update is called once per frame  
    void Update()
    {

    }

    public void OpenDoor()
    {
        Debug.Log("Open");
        StartCoroutine(Move(transform.localPosition, targetRotation));
    }

    public void CloseDoor()
    {
        StartCoroutine(Move(transform.localPosition, initialRotation));
    }

    private IEnumerator Move(Vector3 targetPos, Quaternion targetRot)
    {
        float progress = 0;
        Vector3 startPos = transform.localPosition;
        Quaternion startRot = transform.localRotation;
        while (progress < 1)
        {
            progress += Time.deltaTime * speed * 0.01f;
            transform.localPosition = Vector3.Lerp(startPos, targetPos, progress);
            transform.localRotation = Quaternion.Slerp(startRot, targetRot, progress);
            yield return null;
        }

        // 确保完成后设置到目标位置  
        transform.localPosition = targetPos;
        transform.localRotation = targetRot;
    }
}