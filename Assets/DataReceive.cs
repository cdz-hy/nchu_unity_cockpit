using System;
using System.Runtime.InteropServices;
using UnityEngine;
using XPlaneConnect; // 使用我们之前定义的命名空间

public class XPlaneConnectManager : MonoBehaviour
{
    // 存储连接信息
    private static ushort UDP_port = 49001;
    private XPCSocket socket;
    
    private const int rows = 28;
    private float[] datas = new float[rows * 9];

    // 定义一个事件，当数据更新时触发
    public static event Action<float[]> OnDataReceived;

    void Start()
    {

        // 建立与 X-Plane 的 UDP 连接，假设 X-Plane 运行在本地
        socket = XPlaneConnectNative.aopenUDP("", UDP_port, UDP_port);
        Debug.Log("UDP创建接收端口");

    }

    void Update()
    {

        const int rows = 28;
        XPlaneConnectNative.readDATA(socket, datas, rows);
        // if(datas[0] != 0)
        //     Debug.Log("接收数据中" + datas[0] + " " +
        //     + datas[1] + " " + datas[2] + " " + datas[3] + " " + datas[4] + " "
        //     + datas[5] + " " + datas[6] + " " + datas[7] + " " + datas[8] + " "
        //     );

        // 触发事件，传递数据数组
        OnDataReceived?.Invoke(datas);

    }

    void OnApplicationQuit()
    {
        // 在应用退出时关闭 UDP 连接
        XPlaneConnectNative.closeUDP(socket);
        Debug.Log("UDP连接关闭");
    }
}
