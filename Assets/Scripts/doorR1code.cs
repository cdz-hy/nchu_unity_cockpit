using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorR1code : MonoBehaviour
{
    [Header("Door Movement Settings")]
    [SerializeField] private float openSpeed = 0.8f;          // 舱门运动速度  
    [SerializeField] private float innerPullDistance = 0.1f;  // 向内拉的距离  
    [SerializeField] private float outerPushAngle = 180f;     // 向外推的角度（180度）    
    [SerializeField] private float finalOffset = 1f;      // 最终位置偏移量  
    [SerializeField] private float extraLeftOffset = 0.5f;    // 向左额外平移的距离  

    private Vector3 initialPosition;    // 舱门初始位置  
    private Quaternion initialRotation;  // 舱门初始旋转  
    private bool isDoorOpened = false;   // 舱门状态  
    private bool isDoorMoving = false;   // 舱门是否正在移动  

    private void Start()
    {
        // 自动赋值为当前物体的 Transform  
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    private void Update()
    {
        // 按下 F 键切换舱门状态  
        if (Input.GetKeyDown(KeyCode.F) && !isDoorMoving)
        {
            if (!isDoorOpened)
            {
                StartCoroutine(OpenDoorSequence());
            }
            else
            {
                StartCoroutine(CloseDoorSequence());
            }
        }
    }

    // 开门协程（分阶段运动）  
    private System.Collections.IEnumerator OpenDoorSequence()
    {
        isDoorMoving = true;

        // 阶段1：向内拉动  
        Vector3 targetInnerPosition = initialPosition + transform.InverseTransformDirection(transform.right) * innerPullDistance;
        yield return MoveDoor(targetInnerPosition, initialRotation); // 向内拉动  

        // 阶段2：以门的中心为旋转中心旋转到与原位置垂直（90度，围绕自身中轴旋转）  
        Quaternion verticalRotation = initialRotation * Quaternion.Euler(0, -90, 0); // 旋转90度  
        yield return MoveDoor(targetInnerPosition, verticalRotation); // 保持在90度  

        // 阶段3：以门的边缘为旋转中心向外旋转到180度并平移  
        Quaternion targetRotation = verticalRotation * Quaternion.Euler(0, -90, 0); // 从90度变化到180度  
        Vector3 outerPosition = targetInnerPosition - transform.right * 0.3f + transform.forward * 0.35f;
        yield return MoveDoor(outerPosition, targetRotation); // 向外推动  

        // 阶段4：向左平移一段距离  
        Vector3 leftOffsetPosition = outerPosition - transform.right * 0.02f + transform.forward * 0.1f;
        yield return MoveDoor(leftOffsetPosition, targetRotation); // 继续保持旋转，向左平移  

        isDoorOpened = true;
        isDoorMoving = false;
    }

    // 关门协程（反向操作）  
    private System.Collections.IEnumerator CloseDoorSequence()
    {
        isDoorMoving = true;

        // 阶段1：以门的边缘为旋转中心回到180度引导到内侧  
        Quaternion verticalRotation = initialRotation * Quaternion.Euler(0, -90, 0);
        // 先到边缘位置   
        Vector3 positionAfterPush = initialPosition + transform.right * 0.02f - transform.forward * 0.1f;
        yield return MoveDoor(positionAfterPush, verticalRotation);

        // 阶段2：旋转回原位置  
        yield return MoveDoor(initialPosition, initialRotation); // 回到起始位置  

        isDoorOpened = false;
        isDoorMoving = false;
    }

    // 通用移动方法  
    private System.Collections.IEnumerator MoveDoor(Vector3 targetPos, Quaternion targetRot)
    {
        float progress = 0;
        Vector3 startPos = transform.localPosition;
        Quaternion startRot = transform.localRotation;

        while (progress < 1)
        {
            progress += Time.deltaTime * openSpeed;
            transform.localPosition = Vector3.Lerp(startPos, targetPos, progress);
            transform.localRotation = Quaternion.Slerp(startRot, targetRot, progress);
            yield return null;
        }

        // 确保完成后设置到目标位置  
        transform.localPosition = targetPos;
        transform.localRotation = targetRot;
    }
}