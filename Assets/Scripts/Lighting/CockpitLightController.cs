using UnityEngine;

/// <summary>
/// 控制座舱灯光亮度，根据DataCenter中的CockpitLight1参数(0-1)变化
/// </summary>
public class CockpitLightController : MonoBehaviour
{
    [Header("灯光参数")]
    public float maxIntensity = 0.6f;         // 最大亮度，对应CockpitLight1=1时
    public float minIntensity = 0.0f;         // 最小亮度，对应CockpitLight1=0时
    public float smoothingFactor = 0.1f;      // 平滑过渡系数 (0-1)
    
    [Header("色温设置")]
    public bool adjustColorTemperature = false; // 是否根据亮度调整色温
    public Color lowIntensityColor = new Color(1.0f, 0.9f, 0.7f); // 低亮度时偏黄色调
    public Color highIntensityColor = new Color(1.0f, 1.0f, 0.95f); // 高亮度时偏白色调
    
    [Header("调试")]
    public bool useTestValue = false;         // 是否使用测试值(不读取DataCenter)
    [Range(0, 1)]
    public float testLightValue = 0.5f;       // 测试用亮度值
    
    private Light cockpitLight;               // 灯光组件引用
    private DataCenter dataCenter;            // 数据中心引用
    private float currentIntensity;           // 当前亮度
    private float targetIntensity;            // 目标亮度
    
    void Start()
    {
        // 获取灯光组件
        cockpitLight = GetComponent<Light>();
        if (cockpitLight == null)
        {
            Debug.LogError("没有找到Light组件，请确保此脚本附加到有Light组件的游戏对象上");
            enabled = false;
            return;
        }
        
        // 获取DataCenter实例
        dataCenter = DataCenter.Instance;
        if (dataCenter == null && !useTestValue)
        {
            Debug.LogWarning("未找到DataCenter实例，将使用测试值");
            useTestValue = true;
        }
        
        // 初始化亮度
        currentIntensity = cockpitLight.intensity;
        UpdateLightIntensity();
    }
    
    void Update()
    {
        UpdateLightIntensity();
        
        // 在调试模式下，使用键盘控制灯光亮度
        if (useTestValue)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                testLightValue = Mathf.Clamp01(testLightValue + 0.01f);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                testLightValue = Mathf.Clamp01(testLightValue - 0.01f);
            }
        }
    }

    /// <summary>
    /// 更新灯光亮度
    /// </summary>
    void UpdateLightIntensity()
    {
        float lightValue = GetLightValue();
        targetIntensity = Mathf.Lerp(minIntensity, maxIntensity, lightValue);

        // 使用基于时间的平滑过渡，确保在指定时间内完成过渡
        float transitionSpeed = 1f / smoothingFactor; // smoothingFactor现在表示过渡时间（秒）
        currentIntensity = Mathf.MoveTowards(currentIntensity, targetIntensity, transitionSpeed * Time.deltaTime);

        cockpitLight.intensity = currentIntensity;

        if (adjustColorTemperature)
        {
            cockpitLight.color = Color.Lerp(lowIntensityColor, highIntensityColor, lightValue);
        }
    }

    /// <summary>
    /// 获取灯光值(0-1)
    /// </summary>
    float GetLightValue()
    {
        if (useTestValue)
        {
            return testLightValue;
        }
        else if (dataCenter != null)
        {
            // 从DataCenter读取cockpitLight1属性
            return dataCenter.cockpitLight1;
        }
        
        return 0.5f; // 默认返回中等亮度
    }
} 