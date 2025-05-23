// using UnityEngine;

// public class DataCenter : MonoBehaviour
// {


//     float pitchAngle = 0;
//     float rollAngle  = 0;
//     float rotationangle  = 0;
//     float altitude  = 0;
//     float airSpeed  = 0;

//     float[] datas = null;

//     void Start()
//     {
//         // 订阅事件
//         XPlaneConnectManager.OnDataReceived += HandleData;
//     }

//     void OnDestroy()
//     {
//         // 取消订阅（避免内存泄漏）
//         XPlaneConnectManager.OnDataReceived -= HandleData;
//     }

//     private void HandleData(float[] datas)
//     {
//         // 处理接收到的数据
//         this.datas = datas;

//         for (int i = 0; i < datas.Length; i += 9){

//             switch(i){
//                 case 17:
//                     pitchAngle = datas[i + 1];
//                     rollAngle  = datas[i + 2];
//                     rotationangle = datas[i + 3];
//                     break;
//                 case 2:
//                     break;

//             }

//         }


//     }
// }



using System;
using UnityEngine;

public class DataCenter : MonoBehaviour
{
    // 单例模式，便于全局访问最新数据
    public static DataCenter Instance { get; private set; }

    // 保存飞机状态数据（可根据实际数据进行扩展）


    //====== 飞机外部姿态信息 ======//
    public float pitchAngle { get; private set; }
    public float rollAngle { get; private set; }
    public float rotationAngle { get; private set; }
    public float altitude { get; private set; }
    public float airSpeed { get; private set; }

    public float latitude { get; private set; }
    public float longitude { get; private set; }


    //====== 飞机内部操控信息 ======//
    public float pitchControl { get; private set; }
    public float rollControl { get; private set; }

    public float throttleLever1 { get; private set; }
    public float throttleLever2 { get; private set; }
    

    void Awake()
    {
        // 保证单例唯一性
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        // 订阅数据接收事件
        XPlaneConnectManager.OnDataReceived += HandleData;

        // 订阅操纵杆移动事件
        JoystickController.joystickControllerRotation += renewController;

    }

    void OnDestroy()
    {
        // 取消订阅，防止内存泄漏
        XPlaneConnectManager.OnDataReceived -= HandleData;
        JoystickController.joystickControllerRotation -= renewController;
    }

    private void HandleData(float[] datas)
    {
        // 按照原有逻辑处理数据，每9个为一组，取第17号组的数据更新角度
        for (int i = 0; i < datas.Length; i += 9)
        {
            if (Math.Abs(datas[i] - 3) < 0.01){
                airSpeed  = datas[i + 1];
            }
            else if (Math.Abs(datas[i] - 17) < 0.01)
            {
                pitchAngle = datas[i + 1];
                rollAngle  = datas[i + 2];
                rotationAngle = datas[i + 4];
            }
            else if (Math.Abs(datas[i] - 20) < 0.01)
            {
                latitude = datas[i + 1];
                longitude  = datas[i + 2];
                altitude = datas[i + 6];
            }
        }
        //Debug.Log(pitchAngle + " " + rollAngle + " " + rotationAngle);
        // Debug.Log(latitude + " " + longitude + " ");
        // Debug.Log(rotationAngle + " ");
        // Debug.Log(altitude + " ");
        
    }

    void Update()
    {
        //小键盘按键1增加throttleLever1的值，按键2减少
        if (Input.GetKey(KeyCode.Alpha1)){
            throttleLever1 += 0.01f;
        }
        if (Input.GetKey(KeyCode.Alpha2)){
            throttleLever1 -= 0.01f;
        }
        if (Input.GetKey(KeyCode.Alpha1)){
            throttleLever2 += 0.01f;
        }
        if (Input.GetKey(KeyCode.Alpha2)){
            throttleLever2 -= 0.01f;
        }
        
    }


    private void renewController(float[] datas){

        this.pitchControl = datas[0];
        this.rollControl = datas[1];

        //Debug.Log(pitchControl + " " + rollControl + " ");
    }
}



// using System;
// using UnityEngine;

// public class DataCenter : MonoBehaviour
// {
//     // 单例模式，便于全局访问最新数据
//     public static DataCenter Instance { get; private set; }

//     // 平滑因子（0～1之间，值越小数据变化越平滑）
//     [Range(0.01f, 1f)]
//     public float smoothing = 0.1f;

//     //====== 飞机外部姿态信息 ======//
//     public float pitchAngle { get; private set; }
//     public float rollAngle { get; private set; }
//     public float rotationAngle { get; private set; }
//     public float altitude { get; private set; }
//     public float airSpeed { get; private set; }
//     public float latitude { get; private set; }
//     public float longitude { get; private set; }

//     //====== 飞机内部操控信息 ======//
//     public float pitchControl { get; private set; }
//     public float rollControl { get; private set; }

//     void Awake()
//     {
//         // 保证单例唯一性
//         if (Instance != null && Instance != this)
//         {
//             Destroy(this.gameObject);
//             return;
//         }
//         Instance = this;
//     }

//     void Start()
//     {
//         // 订阅数据接收事件
//         XPlaneConnectManager.OnDataReceived += HandleData;

//         // 订阅操纵杆移动事件
//         JoystickController.joystickControllerRotation += RenewController;
//     }

//     void OnDestroy()
//     {
//         // 取消订阅，防止内存泄漏
//         XPlaneConnectManager.OnDataReceived -= HandleData;
//         JoystickController.joystickControllerRotation -= RenewController;
//     }

//     private void HandleData(float[] datas)
//     {
//         // 按照协议，每9个数据为一组
//         for (int i = 0; i < datas.Length; i += 9)
//         {
//             // 当数据标识为17时，表示外部姿态信息
//             if (Math.Abs(datas[i] - 17) < 0.01f)
//             {
//                 // 使用低通滤波平滑数据
//                 pitchAngle = Mathf.Lerp(pitchAngle, datas[i + 1], smoothing);
//                 rollAngle = Mathf.Lerp(rollAngle, datas[i + 2], smoothing);
//                 rotationAngle = Mathf.Lerp(rotationAngle, datas[i + 3], smoothing);
//                 // altitude、airSpeed 可按需添加
//             }
//             // 当数据标识为20时，表示经纬度信息
//             else if (Math.Abs(datas[i] - 20) < 0.01f)
//             {
//                 latitude = Mathf.Lerp(latitude, datas[i + 1], smoothing);
//                 longitude = Mathf.Lerp(longitude, datas[i + 2], smoothing);
//             }
//         }
//     }

//     private void RenewController(float[] datas)
//     {
//         this.pitchControl = datas[0];
//         this.rollControl = datas[1];
//     }
// }
