using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 引入Unity引擎核心命名空间

public class ControlColumn : MonoBehaviour
{
    public Camera cam;
    public GameObject obj;//与杆相连的滑块
    float objPastX;
    public int select = 0;
    private Vector3 past;//存储鼠标之前的位置
    private Vector3 present;//存储鼠标现在的位置
    // Start is called before the first frame update
    void Start()
    {
         objPastX = obj.transform.localRotation.eulerAngles.x;
    }
/// <summary>
/// 物体选择器类 - 用于通过鼠标点击选择带有Mesh Collider的物体
/// </summary>

    /// <summary>
    /// Unity每帧调用的更新方法
    /// </summary>
    void Update()
    {
        // 检测鼠标左键是否被按下(0表示左键，1右键，2中键)
        if (Input.GetMouseButtonDown(0))
        {
            // 从主摄像机发射一条射线，射线起点是摄像机位置，方向指向鼠标屏幕位置对应的世界坐标
            // ScreenPointToRay将屏幕坐标(像素位置)转换为世界空间中的射线
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            // RaycastHit结构体，用于存储射线碰撞信息
            RaycastHit hit;

            // 执行射线检测，参数说明：
            // ray - 要发射的射线
            // out hit - 输出参数，存储碰撞信息
            // 无距离参数表示使用无限远距离
            // 无层级掩码表示检测所有层
            if (Physics.Raycast(ray, out hit))
            {
                // 尝试将碰撞体的Collider转换为MeshCollider
                // as操作符会尝试转换，如果失败则返回null而不会抛出异常
                past = Input.mousePosition;
                MeshCollider meshCollider = hit.collider as MeshCollider;

                // 检查转换是否成功(即碰撞体是否是MeshCollider)
                //if (meshCollider != null)
                //{

                select = 1;
                    
               // }

            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            select = 0;
        }
        if (select == 1)
        {
            present = Input.mousePosition;
            float changeX = present.x - past.x;
            float changeY = present.y - past.y;
            past = present;
            objPastX = objPastX + changeX;
            obj.transform.localRotation = Quaternion.Euler(objPastX , 0, 0);

            
        }
        
    }
}

