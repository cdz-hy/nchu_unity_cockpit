using UnityEngine;
using System.Collections.Generic;

public class VoiceCommand : MonoBehaviour
{
    private Dictionary<string, System.Action> commandActions;
    public Gear_1 gearController; // 目标物体的控制脚本

    private void Start()
    {
        // 初始化指令集
        commandActions = new Dictionary<string, System.Action>
        {
            { "收起起落架", GearUp },
            { "放下起落架", GearDown },
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

    // 示例指令方法
    private void GearUp()
    {
        Debug.Log("收起起落架执行成功！");
        gearController.isKeyPressed = 2;
    }

    private void GearDown()
    {
        Debug.Log("放下起落架执行成功！.");
        gearController.isKeyPressed = 1;
    }

    private void Test()
    {
        Debug.Log("测试功能成功");
        gearController.VoiceControll();
    }
}