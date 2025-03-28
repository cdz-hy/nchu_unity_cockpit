using UnityEngine;
using System.Collections.Generic;

public class VoiceCommand : MonoBehaviour
{
    private Dictionary<string, System.Action> commandActions;
    public Gear_1 gearController; // Ŀ������Ŀ��ƽű�

    private void Start()
    {
        // ��ʼ��ָ�
        commandActions = new Dictionary<string, System.Action>
        {
            { "���������", GearUp },
            { "���������", GearDown },
            { "1234", Test }
        };
    }

    // �������������ִ����Ӧ��ָ��
    public void ExecuteCommand(string command)
    {
        if (commandActions.TryGetValue(command, out System.Action action))
        {
            action.Invoke(); // ִ�ж�Ӧ��ָ��
        }
        else
        {
            Debug.LogWarning($"Command '{command}' not recognized.");
        }
    }

    // ʾ��ָ���
    private void GearUp()
    {
        Debug.Log("���������ִ�гɹ���");
        gearController.isKeyPressed = 2;
    }

    private void GearDown()
    {
        Debug.Log("���������ִ�гɹ���.");
        gearController.isKeyPressed = 1;
    }

    private void Test()
    {
        Debug.Log("���Թ��ܳɹ�");
        gearController.VoiceControll();
    }
}