using System;
using System.Runtime.InteropServices;
using UnityEngine;
using XPlaneConnect; // 使用我们之前定义的命名空间

public class XPlaneConnectManager : MonoBehaviour
{
    // 存储连接信息
    private static ushort UDP_port = 49001;
    private XPCSocket socket;
    
    // 用于测试的位置信息数据
    // 数组长度为 7，分别代表：纬度、经度、高度、俯仰、滚转、偏航、起落架状态
    private const int rows = 28;
    private float[] datas = new float[rows * 9];

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
        if(datas[0] != 0)
            Debug.Log("接收数据中" + datas[0] + " " +
            + datas[1] + " " + datas[2] + " " + datas[3] + " " + datas[4] + " "
            + datas[5] + " " + datas[6] + " " + datas[7] + " " + datas[8] + " "
            );

    }

    void OnApplicationQuit()
    {
        // 在应用退出时关闭 UDP 连接
        XPlaneConnectNative.closeUDP(socket);
        Debug.Log("UDP连接关闭");
    }
}
