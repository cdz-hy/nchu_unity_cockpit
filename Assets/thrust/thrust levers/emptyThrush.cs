using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class emptyThrush : MonoBehaviour
{
    public Camera cam;
    public float angle = 0;
    public GameObject thrush_1;//油门杆1
    public GameObject thrush_2;//油门杆2
    private int select = 0;
    public float level;
    private Vector3 past;//存储鼠标之前的位置
    private Vector3 present;//存储鼠标现在的位置
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (select == 0)
        {
            angle = (thrush_1.GetComponent<thrush_1>().getAngle() + thrush_2.GetComponent<thrush_2>().getAngle()) / 2;
        }
        if (Input.GetMouseButtonDown(0)) // 检测左键点击
        {

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                //检查是否点击了这个特定物体
                if (hit.collider.gameObject == this.gameObject)
                {
                    select = 1;
                    past = Input.mousePosition;
                }
            }
        }
        
            if (select == 1)
        {
            present = Input.mousePosition;
            float changeX = present.x - past.x;
            past = present;

            if (cam.transform.localPosition.x < this.transform.localPosition.x)
                angle = angle - changeX / 5;
            if (cam.transform.localPosition.x >= this.transform.localPosition.x)
                angle = angle + changeX / 5;

            if (angle >= 0)
                angle = 0;
            else if (angle <= -57)
                angle = -57;
            level = -angle / 57;

            DataCenter.Instance.throttleLever1 = level;
            DataCenter.Instance.throttleLever2 = level;
            thrush_1.GetComponent<thrush_1>().setAngle(angle);
            thrush_2.GetComponent<thrush_2>().setAngle(angle);
        }
        if (Input.GetMouseButtonUp(0))
        {
            select = 0;
        }
        this.transform.localRotation = Quaternion.Euler(angle, 0, 0);
    }
}
