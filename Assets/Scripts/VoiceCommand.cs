using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class VoiceCommand : MonoBehaviour
{
    private Dictionary<string, System.Action> commandActions;
    public Gear_1 gearController; // ����ܵĿ��ƽű�
    private DoorControll[] doorScripts;

    public interface DoorControll
    {
        void Open(); // ����
        void Close(); // ����
    }


    private void Start()
    {
        doorScripts = FindObjectsOfType<MonoBehaviour>().OfType<DoorControll>().ToArray();
        // Debug.Log(doorScripts.Length);
        // ��ʼ��ָ�
        commandActions = new Dictionary<string, System.Action>
        {
            { "���������", GearUp },
            { "���������", GearDown },
            { "����", OpenNearestDoor },
            { "����", CloseNearestDoor },
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

    private Camera GetActiveCamera()
    {
        foreach (Camera cam in Camera.allCameras)
        {
            if (cam.gameObject.activeInHierarchy)
            {
                return cam; // ���ص�һ������������
            }
        }
        return null; // ���û�м��������������� null
    }

    private DoorControll GetNearestDoor()
    {
        // ��ʼ������Ľű�����С����
        DoorControll nearestScript = null;
        float nearestDistance = Mathf.Infinity;

        // ��ȡ��ǰ����ͷ��λ��
        Vector3 cameraPosition = GetActiveCamera().transform.position;

        // �������нű����ҵ�������ͷ�����һ��
        foreach (var script in doorScripts)
        {
            // ��ȡ�ű����������λ��
            Vector3 scriptPosition = ((MonoBehaviour)script).gameObject.GetComponent<Transform>().position;

            // �������
            float distance = Vector3.Distance(cameraPosition, scriptPosition);

            // ��������Ľű�����С����
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestScript = script;
            }
            Debug.Log("������룺" + nearestDistance);
        }

        return nearestScript;
    }

    // ʾ��ָ���
    private void GearUp()
    {
        Debug.Log("���������ִ�гɹ���");
        gearController.Up();
    }

    private void GearDown()
    {
        Debug.Log("���������ִ�гɹ���.");
        gearController.Down();
    }

    private void OpenNearestDoor()
    {
        Debug.Log("���ųɹ�");
        GetNearestDoor().Open();
    }

    private void CloseNearestDoor()
    {
        Debug.Log("���ųɹ�");
        GetNearestDoor().Close();
    }

    private void Test()
    {
        Debug.Log("���Թ��ܳɹ�");
        gearController.VoiceControll();
    }
}