using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vertical_diamend_1 : MonoBehaviour
{
    public float glideslopeAngle;
    public float rotationSpeed = 360f;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        glideslopeAngle = 0.8f * Mathf.Sin(Time.time);
        Vector3 targetPosition = initialPosition + new Vector3(0, glideslopeAngle * 0.016f, 0);
        StartCoroutine(Move(targetPosition, initialRotation)); 
    }
    public IEnumerator Move(Vector3 targetPos, Quaternion targetRot)
    {
        float progress = 0;
        Vector3 startPos = transform.localPosition;
        Quaternion startRot = transform.localRotation;

        while (progress < 1)
        {
            progress += Time.deltaTime * rotationSpeed;
            transform.localPosition = Vector3.Lerp(startPos, targetPos, progress);
            transform.localRotation = Quaternion.Slerp(startRot, targetRot, progress);
            yield return null;
        }

        // 确保完成后设置到目标位置  
        transform.localPosition = targetPos;
        transform.localRotation = targetRot;
    }
}
