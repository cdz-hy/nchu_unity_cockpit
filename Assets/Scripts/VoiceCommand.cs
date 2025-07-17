using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class VoiceCommand : MonoBehaviour
{
    private Dictionary<string, System.Action> commandActions;
    public Gear_1 gearController; // 起落架的控制脚本
    private DoorControll[] doorScripts;

    public interface DoorControll
    {
        void Open(); // 开门
        void Close(); // 关门
    }


    private void Start()
    {
        doorScripts = FindObjectsOfType<MonoBehaviour>().OfType<DoorControll>().ToArray();
        // Debug.Log(doorScripts.Length);
        // 初始化指令集
        commandActions = new Dictionary<string, System.Action>
        {
            { "收起起落架", GearUp },
            { "放下起落架", GearDown },
            { "开门", OpenNearestDoor },
            { "关门", CloseNearestDoor },
            { "1234", Test }
        };
    }

    // 方法来接收命令并执行相应的指令
    public void ExecuteCommand(string command)
    {
        if (commandActions.TryGetValue(command, out System.Action action))
        {
            action.Invoke(); // 执行对应的指令
        }
        else
        {
            Debug.LogWarning($"Command '{command}' not recognized.");
        }
    }

    private Camera GetActiveCamera()
    {
        foreach (Camera cam in Camera.allCameras)
        {
            if (cam.gameObject.activeInHierarchy)
            {
                return cam; // 返回第一个激活的摄像机
            }
        }
        return null; // 如果没有激活的摄像机，返回 null
    }

    private DoorControll GetNearestDoor()
    {
        // 初始化最近的脚本和最小距离
        DoorControll nearestScript = null;
        float nearestDistance = Mathf.Infinity;

        // 获取当前摄像头的位置
        Vector3 cameraPosition = GetActiveCamera().transform.position;

        // 遍历所有脚本，找到离摄像头最近的一个
        foreach (var script in doorScripts)
        {
            // 获取脚本所在物体的位置
            Vector3 scriptPosition = ((MonoBehaviour)script).gameObject.GetComponent<Transform>().position;

            // 计算距离
            float distance = Vector3.Distance(cameraPosition, scriptPosition);

            // 更新最近的脚本和最小距离
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestScript = script;
            }
            Debug.Log("最近距离：" + nearestDistance);
        }

        return nearestScript;
    }

    // 示例指令方法
    private void GearUp()
    {
        Debug.Log("收起起落架执行成功！");
        gearController.Up();
    }

    private void GearDown()
    {
        Debug.Log("放下起落架执行成功！.");
        gearController.Down();
    }

    private void OpenNearestDoor()
    {
        Debug.Log("开门成功");
        GetNearestDoor().Open();
    }

    private void CloseNearestDoor()
    {
        Debug.Log("关门成功");
        GetNearestDoor().Close();
    }

    private void Test()
    {
        Debug.Log("测试功能成功");
        gearController.VoiceControll();
    }
}