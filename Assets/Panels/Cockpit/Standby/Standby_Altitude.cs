using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Standby_Altitude : MonoBehaviour
{
    public float MoveSpeed = 360f; // ÿ����ת�ĽǶ�  
    public float Altitude;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    // Start is called before the first frame update
    void Start()
    {
        //airSpeed = DataCenter.Instance.AirSpeed;
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        //Data = DataCenter.Instance.Altitude;
        //Altitude++;
        //airSpeed = 60;
        Control(Altitude);
        //�� Z ����ת
        Control(Altitude);
    }

    void Control(float height)
    {
        height *= -0.000059f;
        Vector3 targetPosition = initialPosition + new Vector3(0, height, 0);
        StartCoroutine(Move(targetPosition, initialRotation));
    }

    public IEnumerator Move(Vector3 targetPos, Quaternion targetRot)
    {
        float progress = 0;
        Vector3 startPos = transform.localPosition;
        Quaternion startRot = transform.localRotation;

        while (progress < 1)
        {
            progress += Time.deltaTime * MoveSpeed;
            transform.localPosition = Vector3.Lerp(startPos, targetPos, progress);
            transform.localRotation = Quaternion.Slerp(startRot, targetRot, progress);
            yield return null;
        }

        // ȷ����ɺ����õ�Ŀ��λ��  
        transform.localPosition = targetPos;
        transform.localRotation = targetRot;
    }
}
