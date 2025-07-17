using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Compass : MonoBehaviour
{
    public float rotationSpeed = 360f; 
    public float angle;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        //angle = 0;
       initialPosition = transform.localPosition;
       initialRotation = transform.localRotation;
    }
    void Update()
    {
        //angle++;
        angle = DataCenter.Instance.rotationAngle;
        Control(angle);
    }
    void Control(float angle)
    {
        Quaternion targetRotation = initialRotation * Quaternion.Euler(0, 0, angle);
        StartCoroutine(Move(initialPosition, targetRotation));        
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

        // ???????????????¦Ë??  
        transform.localPosition = targetPos;
        transform.localRotation = targetRot;
        }
    }
