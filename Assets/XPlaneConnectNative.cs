using System;
using System.Runtime.InteropServices;

namespace XPlaneConnect
{
    // 与 C 端 XPCSocket 结构对应（注意：xpIP 为 16 字节的固定字符数组）
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct XPCSocket
    {
        public ushort port;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string xpIP;
        public ushort xpPort;
        public IntPtr sock; // Windows 下 SOCKET 为指针类型
    }

    // 对应 xplaneConnect.h 中的 WYPT_OP 枚举
    public enum WYPT_OP : byte
    {
        XPC_WYPT_ADD = 1,
        XPC_WYPT_DEL = 2,
        XPC_WYPT_CLR = 3
    }

    // 对应 xplaneConnect.h 中的 VIEW_TYPE 枚举
    public enum VIEW_TYPE
    {
        XPC_VIEW_FORWARDS = 73,
        XPC_VIEW_DOWN,
        XPC_VIEW_LEFT,
        XPC_VIEW_RIGHT,
        XPC_VIEW_BACK,
        XPC_VIEW_TOWER,
        XPC_VIEW_RUNWAY,
        XPC_VIEW_CHASE,
        XPC_VIEW_FOLLOW,
        XPC_VIEW_FOLLOWWITHPANEL,
        XPC_VIEW_SPOT,
        XPC_VIEW_FULLSCREENWITHHUD,
        XPC_VIEW_FULLSCREENNOHUD
    }

    public static class XPlaneConnectNative
    {
        const string dllName = "xplaneConnect";

        // ----- UDP 连接管理函数 -----

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern XPCSocket openUDP(string xpIP);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern XPCSocket aopenUDP(string xpIP, ushort xpPort, ushort port);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void closeUDP(XPCSocket sock);

        // ----- 配置函数 -----

        // 注意：setCONN 的第一个参数为指向 XPCSocket 的指针，因此使用 ref
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int setCONN(ref XPCSocket sock, ushort port);

        // pauseSim 中 pause 参数在 C 端为 char，这里使用 byte 表示 0~255 的值
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int pauseSim(XPCSocket sock, byte pause);

        // ----- X-Plane UDP DATA 函数 -----

        // sendDATA：data 为二维数组，但这里采用扁平化的 float 数组，要求长度为 rows*9
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int sendDATA(XPCSocket sock, [In] float[] data, int rows);

        // readDATA：data 为预先分配好的扁平 float 数组（长度至少为 rows*9）
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int readDATA(XPCSocket sock, [Out] float[] data, int rows);

        // ----- DREF 操作函数 -----

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int sendDREF(XPCSocket sock, string dref, [In] float[] values, int size);

        // sendDREFs：drefs 为字符串数组，values 为指针数组（调用者需处理非托管内存），sizes 为各个数据长度数组
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int sendDREFs(
            XPCSocket sock,
            [In, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] drefs,
            [In, MarshalAs(UnmanagedType.LPArray)] IntPtr[] values,
            [In, MarshalAs(UnmanagedType.LPArray)] int[] sizes,
            int count);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int sendDREFRequest(
            XPCSocket sock,
            [In, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] drefs,
            byte count);

        // getDREFResponse：values 为指针数组，sizes 为输入输出数组
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int getDREFResponse(
            XPCSocket sock,
            [In, Out, MarshalAs(UnmanagedType.LPArray)] IntPtr[] values,
            byte count,
            [In, Out, MarshalAs(UnmanagedType.LPArray)] int[] sizes);

        // getDREF：size 参数以 ref 传递
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int getDREF(
            XPCSocket sock,
            string dref,
            [Out] float[] values,
            ref int size);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int getDREFs(
            XPCSocket sock,
            [In, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] drefs,
            [In, Out, MarshalAs(UnmanagedType.LPArray)] IntPtr[] values,
            byte count,
            [In, Out, MarshalAs(UnmanagedType.LPArray)] int[] sizes);

        // ----- 位置函数 -----

        // getPOSI：values 数组长度应为 7，ac 用 byte 表示
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int getPOSI(XPCSocket sock, [Out] double[] values, byte ac);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int sendPOSI(XPCSocket sock, [In] double[] values, int size, byte ac);

        // ----- TERR（地形）函数 -----

        // sendTERRRequest：posi 数组长度应为 3
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int sendTERRRequest(XPCSocket sock, [In] double[] posi, byte ac);

        // getTERRResponse：values 数组长度应为 11
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int getTERRResponse(XPCSocket sock, [Out] double[] values, byte ac);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int sendPOST(XPCSocket sock, [In] double[] posi, int size, [Out] double[] values, byte ac);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int getTERR(XPCSocket sock, [In] double[] posi, [Out] double[] values, byte ac);

        // ----- 控制函数 -----

        // getCTRL：values 数组长度应为 7
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int getCTRL(XPCSocket sock, [Out] float[] values, byte ac);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int sendCTRL(XPCSocket sock, [In] float[] values, int size, byte ac);

        // ----- 绘图函数 -----

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int sendTEXT(XPCSocket sock, string msg, int x, int y);

        // sendWYPT：points 数组长度应为 count * 3（每个点包含经、纬、高）
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int sendWYPT(XPCSocket sock, WYPT_OP op, [In] float[] points, int count);

        // ----- 视角函数 -----

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int sendVIEW(XPCSocket sock, VIEW_TYPE view);

        // ----- COMM 命令函数 -----

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int sendCOMM(XPCSocket sock, string comm);
    }
}
