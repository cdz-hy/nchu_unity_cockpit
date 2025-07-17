using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thrush_1 : MonoBehaviour
{
    public Camera cam;
    public float lever_1 = 0;
    public float angle = 0;
    private int select = 0;
    public GameObject thrush_2;//油门杆2
    private Vector3 past;//存储鼠标之前的位置
    private Vector3 present;//存储鼠标现在的位置
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
        if (Input.GetMouseButtonUp(0))
            select = 0;
        if(select == 1)
        {
            present = Input.mousePosition;
            float changeX = present.x - past.x;
            past = present;

            if (cam.transform.position.x < this.transform.position.x)
                angle = angle - changeX / 5;
            if (cam.transform.position.x >= this.transform.position.x)
                angle = angle + changeX / 5;

            if (angle >= 0)
                angle = 0;
            else if (angle <= -57)
                angle = -57;
            
        }
        lever_1 = -angle / 57.0f;
        DataCenter.Instance.throttleLever1 = lever_1;

        thrush_2.GetComponent<thrush_2>().setAngle(angle);
        this.transform.localRotation = Quaternion.Euler(angle, 0, 0);
    }
    public float getAngle() {  return this.angle; }
    public void setAngle(float angle) {  this.angle = angle; }

}
