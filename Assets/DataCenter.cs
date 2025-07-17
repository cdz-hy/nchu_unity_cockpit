using System;
using UnityEngine;
using UnityEngine.UI;

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

    public float throttleLever1;
    public float throttleLever2;
    //====== 座舱灯光信息 ======//
    public float cockpitLight1 { get; set; } = 0f; // 座舱灯光亮度值(0-1)
                                                   //====== 油门 ======//
    public float thrust_1;//左边油门参数
    public float thrust_2;
    //====== EICAS1数据信息 ======//
    public float N1_Left = (float)101.5;
    public float N1_Right = (float)50.5;
    public float EGT_Left = 900;
    public float EGT_Right = 400;
    public float FF_Left = (float)11.2;
    public float FF_Right = (float)12.5;
    public float FUEL_1 = (float)10.6;
    public float FUEL_2 = (float)11.4;
    public float FUEL_3 = (float)12.5;
    public float FUEL_TOTAL = (float)34.5;
    //====== EICAS2数据信息 ======//
    public float N2_left = 101.5f; //{ get; private set; }
    public float N2_right = 50.5f; //{ get; private set; }
    public float FF { get; private set; }

    public float OILPRESS { get; private set; }
    public float OILTEMP { get; private set; }
    public float OILQTY { get; private set; }
    public float VIB { get; private set; }

    //====== Autopilot数据信息 ======//
    public float TargetAirSpeed { get; private set; }
    public float Course1 { get; private set; }
    public float Course2 { get; private set; }
    public float TargetHeading { get; private set; }
    public float TargetAlt { get; private set; }
    public float VertSpeed { get; private set; }
    public float IAS { get; private set; }

    // 灯光控制相关
    [Header("座舱灯光控制")]
    [SerializeField] private KeyCode increaseLightKey = KeyCode.L; // 增加灯光亮度的按键
    [SerializeField] private KeyCode decreaseLightKey = KeyCode.K; // 减少灯光亮度的按键
    [SerializeField] private float lightChangeSpeed = 0.5f; // 灯光变化速度



    //====== 系统操控信息 ======//

    [Header("系统操控控制")]
    public bool isShowFullFlightPath = false; // 是否显示完整航路
    public bool isReplaying = false;
    public float currentTime = 0f; // 当前回放时间
    public float totalRuntime = 0f; // 总运行时间

    [Header("UI 控件")]
    [SerializeField] private MyButton flightPathToggleUIButton; // 航迹显示UI
    public Slider flySlider;

    void Awake()
    {
        throttleLever1 = 1;
        throttleLever2 = 1;
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

        // 订阅航迹显示事件
        if (flightPathToggleUIButton != null)
            flightPathToggleUIButton.OnButtonReleased += ToggleFlightPathDisplay;

        flySlider.onValueChanged.AddListener(delegate { SetTime(); });
        flySlider.value = 0;
    }

    void OnDestroy()
    {
        // 取消订阅，防止内存泄漏
        XPlaneConnectManager.OnDataReceived -= HandleData;
        JoystickController.joystickControllerRotation -= renewController;
        if (flightPathToggleUIButton != null)
            flightPathToggleUIButton.OnButtonReleased -= ToggleFlightPathDisplay;
    }

    private void HandleData(float[] datas)
    {
        // 按照原有逻辑处理数据，每9个为一组，取第17号组的数据更新角度
        for (int i = 0; i < datas.Length; i += 9)
        {
            if (Math.Abs(datas[i] - 3) < 0.01)
            {
                airSpeed = datas[i + 1];
            }
            else if (Math.Abs(datas[i] - 17) < 0.01)
            {
                pitchAngle = datas[i + 1];
                rollAngle = datas[i + 2];
                rotationAngle = datas[i + 4];
            }
            else if (Math.Abs(datas[i] - 20) < 0.01)
            {
                latitude = datas[i + 1];
                longitude = datas[i + 2];
                altitude = datas[i + 6];
            }
            else if (Math.Abs(datas[i] - 41) < 0.01)
            {
                N1_Left = datas[i + 1];
                N1_Right = datas[i + 2];
            }
            else if (Math.Abs(datas[i] - 45) < 0.01)
            {
                FF_Left = datas[i + 1] / 1000;
                FF_Right = datas[i + 2] / 1000;
            }
            else if (Math.Abs(datas[i] - 47) < 0.01)
            {
                EGT_Left = datas[i + 1];
                EGT_Right = datas[i + 2];
            }
            else if (Math.Abs(datas[i] - 62) < 0.01)
            {
                FUEL_1 = datas[i + 1] / 1000;
                FUEL_2 = datas[i + 2] / 1000;
                FUEL_3 = datas[i + 3] / 1000;
                FUEL_TOTAL = FUEL_1 + FUEL_2 + FUEL_3;
            }
            else if (Math.Abs(datas[i] - 118) < 0.01)
            {
                TargetAirSpeed = datas[i + 1];
                Course1 = datas[i + 2];
                Course2 = datas[i + 2];
                TargetHeading = datas[i + 2];
                TargetAlt = datas[i + 4];
                VertSpeed = datas[i + 3];
                IAS = datas[i + 1];

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
        if (Input.GetKey(KeyCode.Alpha1))
        {
            throttleLever1 += 0.01f;
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            throttleLever1 -= 0.01f;
        }
        if (Input.GetKey(KeyCode.Alpha1))
        {
            throttleLever2 += 0.01f;
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            throttleLever2 -= 0.01f;
        }


        // 处理座舱灯光控制
        HandleLightControl();

    }


    private void renewController(float[] datas)
    {

        this.pitchControl = datas[0];
        this.rollControl = datas[1];

        //Debug.Log(pitchControl + " " + rollControl + " ");
    }

    // 航迹切换
    private void ToggleFlightPathDisplay()
    {
        isShowFullFlightPath = !isShowFullFlightPath;
        Debug.Log("航迹显示 now: " + isShowFullFlightPath);
    }


    /// <summary>
    /// 处理座舱灯光控制
    /// </summary>
    private void HandleLightControl()
    {
        // 按L键增加灯光亮度
        if (Input.GetKey(increaseLightKey))
        {
            SetCockpitLight1(cockpitLight1 + lightChangeSpeed * Time.deltaTime);
        }

        // 按K键减少灯光亮度
        if (Input.GetKey(decreaseLightKey))
        {
            SetCockpitLight1(cockpitLight1 - lightChangeSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// 设置座舱灯光1的亮度值
    /// </summary>
    /// <param name="value">灯光亮度值(0-1)</param>
    public void SetCockpitLight1(float value)
    {
        cockpitLight1 = Mathf.Clamp01(value);
    }

    private void SetTime()
    {
        currentTime = flySlider.value * totalRuntime; 
    }
}