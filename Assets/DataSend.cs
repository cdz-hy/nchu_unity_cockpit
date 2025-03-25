using System;
using System.Text;
using UnityEngine;
using XPlaneConnect; // 使用我们之前定义的命名空间

public class DataSend : MonoBehaviour
{
    // 发送数据的目标IP和端口
    public string targetIP = "10.86.224.135";
    private static ushort UDP_port = 49009;

    private XPCSocket socket;

    void Start()
    {
        // 建立与 X-Plane 的 UDP 连接
        socket = XPlaneConnectNative.aopenUDP(targetIP, UDP_port, 0);
        Debug.Log("UDP创建发送端口");

    }

    void Update()
    {
        
        // 从DataCenter获取最新数据
        float pitchControl = DataCenter.Instance.pitchControl;
        float rollControl = DataCenter.Instance.rollControl;

        // 发送数据
        string controllerDref1 =  "sim/joystick/FC_ptch";
        string controllerDref2 =  "sim/joystick/FC_roll";
        string controllerDref3 =  "sim/joystick/FC_hdng";
        XPlaneConnectNative.sendDREF(socket, controllerDref1, new float[] { pitchControl }, 1);
        XPlaneConnectNative.sendDREF(socket, controllerDref2, new float[] { rollControl / 2 }, 1);
        XPlaneConnectNative.sendDREF(socket, controllerDref2, new float[] { rollControl }, 1);
    }

    private void SendStatusData()
    {
        
        

    }

    
    void OnApplicationQuit()
    {
        // 在应用退出时关闭 UDP 连接
        XPlaneConnectNative.closeUDP(socket);
        Debug.Log("UDP连接关闭");
    }

}
