using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class EngineSmoke_winglets : MonoBehaviour
{
    [Header("引用设置")]
    public Transform airplaneTransform; // 飞机物体
    
    [Header("物理参数")]
    public float forceFactor = 0.03f;     // 加速度映射系数
    public float upDrift = 0.1f;          // 恒定向上漂浮力
    public float spiralFactor = 0.15f;    // 螺旋涡流强度
    public float dragFactor = 0.005f;     // 阻力系数
    public float turbulenceFactor = 0.08f; // 湍流强度
    
    [Header("摆动参数")]
    public float swayXStrength = 0.12f;   // X轴摆动强度
    public float swayYStrength = 0.15f;   // Y轴摆动强度
    public float swayXSpeed = 1.2f;       // X轴摆动速度
    public float swayYSpeed = 0.8f;       // Y轴摆动速度
    public float spiralSpeed = 2.5f;      // 螺旋旋转速度
    
    [Header("涡流触发条件 (节/英尺)")]
    public float minAirspeed = 40.0f;     // 最小触发速度(节)，降低以便更容易触发
    public float maxAirspeed = 350.0f;    // 最大可见速度(节)，增加范围
    public float bestAirspeed = 160.0f;   // 最佳涡流速度(节)
    public float minAOA = 1.0f;           // 最小迎角(度)，降低以便更容易触发
    public float maxAOA = 25.0f;          // 最大迎角(度)，增加范围
    public float bestAOA = 8.0f;          // 最佳涡流迎角(度)
    public float minAltitude = 0.0f;      // 最小高度(英尺)
    public float maxAltitude = 20000.0f;  // 最大高度(英尺)，增加范围
    public float humidityFactor = 1.0f;   // 湿度因子(0-1)，越高涡流越明显
    public float fadeInTime = 0.3f;       // 涡流淡入时间(秒)，缩短使效果更快出现
    public float fadeOutTime = 0.8f;      // 涡流淡出时间(秒)，缩短使效果更自然
    
    [Header("限制参数")]
    public float maxForce = 0.7f;         // 最大力值限制
    public float smoothingFactor = 0.1f;  // 平滑过渡系数
    public bool applyVelocity = true;     // 是否应用速度，涡流通常需要
    
    [Header("调试")]
    public bool showDebugInfo = false;    // 显示调试信息
    public bool alwaysShow = false;       // 总是显示涡流(调试用)
    public bool useRollAngleForAOA = true; // 使用横滚角作为迎角的估计值

    [Header("晃动触发参数")]
    public bool enableTurbulenceTrigger = true; // 是否启用晃动触发
    public float turbulenceThreshold = 0.8f;    // 晃动触发阈值
    public float turbulenceMultiplier = 1.5f;   // 晃动时涡流强度倍增

    // 单位转换常量
    private const float KNOTS_TO_MS = 0.51444f;  // 节到米/秒的转换系数
    private const float FEET_TO_METERS = 0.3048f; // 英尺到米的转换系数

    private ParticleSystem ps;
    private ParticleSystem.ForceOverLifetimeModule fo;
    private ParticleSystem.VelocityOverLifetimeModule vo;
    private ParticleSystem.EmissionModule em;
    private ParticleSystem.MainModule main;
    private Vector3 lastPos;
    private Vector3 lastVel;
    private Vector3 currentForce;
    private float currentVisibility = 0f;
    private float targetVisibility = 0f;
    private float currentAOA = 0f;
    private float currentSpeed = 0f;      // 保存为节
    private float currentAltitude = 0f;   // 保存为英尺
    private float turbulenceBoost = 1.0f; // 晃动增强系数

    // 用于显示的当前力值
    private float fx, fy, fz;
    
    // 用于生成周期性摆动
    private float timeOffset;
    
    // 数据中心引用
    private DataCenter dataCenter;

    // 用于存储上一次的角度
    private float lastRollAngle = 0f;
    private float lastPitchAngle = 0f;

    void Start()
    {
        try
        {
            // 获取粒子系统组件
            ps = GetComponent<ParticleSystem>();
            if (ps == null)
            {
                Debug.LogError("未找到ParticleSystem组件，请确保此脚本附加到有ParticleSystem组件的游戏对象上");
                enabled = false;
                return;
            }
            
            // 初始化粒子系统模块
            fo = ps.forceOverLifetime;
            vo = ps.velocityOverLifetime;
            em = ps.emission;
            main = ps.main;
            
            // 获取DataCenter实例
            dataCenter = DataCenter.Instance;
            if (dataCenter == null)
            {
                Debug.LogWarning("未找到DataCenter实例，涡流效果可能无法正常工作");
            }
            
            // 确保力模块启用并设置为世界空间
            fo.enabled = true;
            fo.space = ParticleSystemSimulationSpace.World;
            
            // 默认启用速度模块，对涡流很重要
            vo.enabled = true;
            vo.space = ParticleSystemSimulationSpace.World;
            
            // 初始化变量
            currentForce = new Vector3(0, upDrift, 0);
            fx = 0;
            fy = upDrift;
            fz = 0;
        }
        catch (System.Exception e)
        {
            Debug.LogError("翼尖涡流初始化错误: " + e.Message + "\n" + e.StackTrace);
            enabled = false;
            return;
        }

        if (airplaneTransform == null)
        {
            Debug.LogWarning("未设置airplaneTransform，将使用本对象的父级Transform");
            airplaneTransform = transform.parent;
            
            if (airplaneTransform == null)
            {
                Debug.LogError("请在 Inspector 中为 EngineSmoke_winglets 分配 airplaneTransform");
                enabled = false;
                return;
            }
        }

        lastPos = airplaneTransform.position;
        lastVel = Vector3.zero;
        currentForce = Vector3.zero;
        
        // 初始化力为螺旋向上
        currentForce = new Vector3(0, upDrift, 0);
        
        // 随机初始时间偏移，使多个翼尖的涡流不同步
        timeOffset = Random.Range(0f, 10f);
        
        // 默认不可见
        SetVisibility(0);
    }

    void Update()
    {
        try
        {
            if (!enabled || airplaneTransform == null)
                return;
                
            // 如果是alwaysShow模式，直接设置可见
            if (alwaysShow)
            {
                currentVisibility = 1.0f;
                targetVisibility = 1.0f;
                SetVisibility(1.0f);
            }
            else
            {
                UpdateFlightParameters();
                UpdateVisibility();
            }
            
            // 不可见且非alwaysShow时跳过后续计算
            if (currentVisibility <= 0.01f && !alwaysShow)
                return;
        }
        catch (System.Exception e)
        {
            Debug.LogError("翼尖涡流Update错误: " + e.Message);
            return;
        }
            
        // 计算速度和加速度
        Vector3 currPos = airplaneTransform.position;
        Vector3 velocity = Vector3.zero;
        Vector3 acceleration = Vector3.zero;
        
        try
        {
            // 防止除零错误
            if (Time.deltaTime > 0.0001f)
            {
                velocity = (currPos - lastPos) / Time.deltaTime;
                acceleration = (velocity - lastVel) / Time.deltaTime;
            }

            // 更新历史数据
            lastPos = currPos;
            lastVel = velocity;
        }
        catch (System.Exception e)
        {
            Debug.LogError("翼尖涡流速度计算错误: " + e.Message);
            // 使用默认值
            velocity = Vector3.zero;
            acceleration = Vector3.zero;
        }
        
        // 计算螺旋涡流效果
        float time = Time.time + timeOffset;
        
        // 基于时间的螺旋运动
        float spiralPhase = time * spiralSpeed;
        float spiralX = Mathf.Sin(spiralPhase) * spiralFactor;
        float spiralY = Mathf.Cos(spiralPhase) * spiralFactor;
        
        // 检测是否在晃动状态，增强螺旋效果
        float turbulenceBoost = 1.0f;
        if (enableTurbulenceTrigger && currentVisibility > 0.1f)
        {
            // 计算姿态变化率
            float angleChangeRate = 0f;
            if (dataCenter != null && Time.deltaTime > 0.0001f)
            {
                float rollRate = Mathf.Abs(dataCenter.rollAngle - lastRollAngle) / Time.deltaTime;
                float pitchRate = Mathf.Abs(dataCenter.pitchAngle - lastPitchAngle) / Time.deltaTime;
                angleChangeRate = (rollRate + pitchRate) * 0.5f;
            }
            
            // 如果晃动超过阈值，增强螺旋效果
            if (angleChangeRate > turbulenceThreshold)
            {
                turbulenceBoost = Mathf.Min(angleChangeRate / turbulenceThreshold, turbulenceMultiplier);
                
                // 增强螺旋效果
                spiralX *= turbulenceBoost;
                spiralY *= turbulenceBoost;
                
                // 在晃动时增加随机性
                spiralX += Random.Range(-0.1f, 0.1f) * turbulenceBoost;
                spiralY += Random.Range(-0.1f, 0.1f) * turbulenceBoost;
            }
        }
        
        // 添加周期性摆动和螺旋
        float swayX = Mathf.Sin(time * swayXSpeed) * swayXStrength + spiralX;
        float swayY = Mathf.Cos(time * swayYSpeed) * swayYStrength + spiralY;
        
        // 添加复合摆动，使运动更加复杂
        swayX += Mathf.Sin(time * swayXSpeed * 2.3f) * (swayXStrength * 0.4f);
        swayY += Mathf.Cos(time * swayYSpeed * 1.7f) * (swayYStrength * 0.3f);

        // 计算目标力 - 考虑坐标系方向：z正向是飞机后方，y正向是向上，x正向是向左
        Vector3 targetForce = new Vector3(
            swayX, // X轴周期性摆动+螺旋
            -acceleration.y * forceFactor + upDrift + swayY, // Y轴摆动+螺旋
            currentSpeed * KNOTS_TO_MS * 0.01f  // 基于速度的后向力，转换节到米/秒
        );
        
        // 添加基于速度的阻力
        targetForce += new Vector3(
            -velocity.x * dragFactor * 0.5f,
            -velocity.y * dragFactor * 0.7f,
            -velocity.z * dragFactor
        );
        
        try
        {
            // 添加随机湍流
            targetForce += new Vector3(
                UnityEngine.Random.Range(-turbulenceFactor * 1.2f, turbulenceFactor * 1.2f),
                UnityEngine.Random.Range(-turbulenceFactor * 1.2f, turbulenceFactor * 1.2f),
                UnityEngine.Random.Range(-turbulenceFactor * 0.5f, turbulenceFactor * 0.5f)
            );

            // 平滑过渡到新的力值
            currentForce = Vector3.Lerp(currentForce, targetForce, smoothingFactor);
            
            // 限制力的大小
            fx = Mathf.Clamp(currentForce.x, -maxForce * 0.9f, maxForce * 0.9f);
            fy = Mathf.Clamp(currentForce.y, -maxForce * 0.3f, maxForce);
            fz = Mathf.Clamp(currentForce.z, 0, maxForce);
        }
        catch (System.Exception e)
        {
            Debug.LogError("计算力值时出错: " + e.Message);
            // 使用安全的默认值
            fx = 0;
            fy = upDrift;
            fz = 0;
        }
        
        try
        {
            // 应用到 Force over Lifetime
            fo.x = new ParticleSystem.MinMaxCurve(fx);
            fo.y = new ParticleSystem.MinMaxCurve(fy);
            fo.z = new ParticleSystem.MinMaxCurve(fz);
            
            // 应用速度
            if (applyVelocity && ps != null)
            {
                vo.enabled = true;
                vo.space = ParticleSystemSimulationSpace.World;
                
                // 使用计算的速度，添加螺旋效果
                float vx = velocity.x * 0.01f + spiralX * 0.5f;
                float vy = velocity.y * 0.01f + spiralY * 0.5f;
                float vz = velocity.z * 0.01f;
                
                vo.x = new ParticleSystem.MinMaxCurve(vx);
                vo.y = new ParticleSystem.MinMaxCurve(vy);
                vo.z = new ParticleSystem.MinMaxCurve(vz);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("应用粒子系统参数时出错: " + e.Message);
        }
        
        // 调试信息
        if (showDebugInfo)
        {
            Debug.Log($"翼尖涡流: 可见度={currentVisibility:F2}, AOA={currentAOA:F1}°, 速度={currentSpeed:F1}节, 高度={currentAltitude:F0}英尺, 晃动倍增={turbulenceBoost:F1}");
        }
    }
    
    // 更新飞行参数
    void UpdateFlightParameters()
    {
        try
        {
            // 从DataCenter获取数据
            if (dataCenter != null)
            {
                // 获取速度 (已经是节)
                currentSpeed = dataCenter.airSpeed;
                
                // 获取高度 (已经是英尺)
                currentAltitude = dataCenter.altitude;
                
                // 获取迎角 - 可以用横滚角作为替代
                if (useRollAngleForAOA)
                {
                    // 使用横滚角的绝对值作为迎角的估计
                    currentAOA = Mathf.Abs(dataCenter.rollAngle);
                }
                else
                {
                    // 使用俯仰角作为迎角的估计
                    currentAOA = Mathf.Abs(dataCenter.pitchAngle);
                }
            }
            else
            {
                // 如果没有DataCenter，使用简单的估算
                Vector3 currPos = airplaneTransform.position;
                
                // 估算速度并转换为节
                float speedMS = 0;
                if (Time.deltaTime > 0.0001f)
                {
                    speedMS = Vector3.Distance(currPos, lastPos) / Time.deltaTime;
                }
                currentSpeed = speedMS / KNOTS_TO_MS;
                
                // 估算高度并转换为英尺
                currentAltitude = airplaneTransform.position.y / FEET_TO_METERS;
                currentAOA = 8.0f; // 默认迎角
            }
            
            // 确保参数在有效范围内
            currentSpeed = Mathf.Max(0, currentSpeed);
            currentAltitude = Mathf.Max(0, currentAltitude);
            currentAOA = Mathf.Clamp(currentAOA, 0, 90);
        }
        catch (System.Exception e)
        {
            Debug.LogError("更新飞行参数时出错: " + e.Message);
            // 设置安全默认值
            currentSpeed = 0;
            currentAltitude = 0;
            currentAOA = 0;
        }
    }
    
    // 更新可见度
    void UpdateVisibility()
    {
        // 计算目标可见度
        if (alwaysShow)
        {
            targetVisibility = 1.0f;
            currentVisibility = 1.0f; // 确保在alwaysShow模式下立即可见
            SetVisibility(1.0f);
            return; // 直接返回，跳过后续计算
        }
        else
        {
            try {
                // 速度因子 (0-1)，使用钟形曲线，在bestAirspeed时最强
                float speedDiff = Mathf.Abs(currentSpeed - bestAirspeed);
                float speedRange = (maxAirspeed - minAirspeed) / 2;
                float speedFactor = Mathf.Clamp01(1.0f - (speedDiff / speedRange));
                speedFactor = Mathf.Pow(speedFactor, 0.7f); // 使曲线更平缓
                
                // 迎角因子 (0-1)，使用钟形曲线，在bestAOA时最强
                float aoaDiff = Mathf.Abs(currentAOA - bestAOA);
                float aoaRange = (maxAOA - minAOA) / 2;
                float aoaFactor = Mathf.Clamp01(1.0f - (aoaDiff / aoaRange));
                aoaFactor = Mathf.Pow(aoaFactor, 0.7f); // 使曲线更平缓
                
                // 高度因子 (0-1)，低空更明显
                float altitudeFactor = Mathf.Clamp01(1.0f - (currentAltitude - minAltitude) / (maxAltitude - minAltitude));
                
                // 湿度影响
                float humidity = humidityFactor;
                
                // 晃动因子 - 检测飞机是否剧烈晃动
                float turbulenceFactor = 1.0f;
                if (enableTurbulenceTrigger && dataCenter != null)
                {
                    // 计算飞机姿态变化率
                    float rollChangeRate = 0f;
                    float pitchChangeRate = 0f;
                    
                    if (Time.deltaTime > 0.0001f)
                    {
                        // 使用DataCenter中的角度变化率估算晃动
                        rollChangeRate = Mathf.Abs(dataCenter.rollAngle - lastRollAngle) / Time.deltaTime;
                        pitchChangeRate = Mathf.Abs(dataCenter.pitchAngle - lastPitchAngle) / Time.deltaTime;
                        
                        // 保存当前角度用于下一帧计算
                        lastRollAngle = dataCenter.rollAngle;
                        lastPitchAngle = dataCenter.pitchAngle;
                    }
                    
                    // 计算总体晃动强度
                    float turbulenceIntensity = (rollChangeRate + pitchChangeRate) * 0.5f;
                    
                    // 如果晃动超过阈值，增强涡流效果
                    if (turbulenceIntensity > turbulenceThreshold)
                    {
                        // 晃动越强，涡流越明显，但不超过最大倍增值
                        float multiplier = Mathf.Min(turbulenceIntensity / turbulenceThreshold, turbulenceMultiplier);
                        turbulenceFactor = multiplier;
                        
                        if (showDebugInfo)
                        {
                            Debug.Log($"检测到剧烈晃动! 强度: {turbulenceIntensity:F2}, 涡流倍增: {turbulenceFactor:F2}");
                        }
                    }
                }
                
                // 综合因子
                targetVisibility = speedFactor * aoaFactor * altitudeFactor * humidity * turbulenceFactor;
                
                // 确保值在0-1之间
                targetVisibility = Mathf.Clamp01(targetVisibility);
                
                // 速度太低或太高时不显示
                if (currentSpeed < minAirspeed || currentSpeed > maxAirspeed)
                {
                    targetVisibility = 0;
                }
                
                // 迎角太小或太大时不显示
                if (currentAOA < minAOA || currentAOA > maxAOA)
                {
                    targetVisibility = 0;
                }
            }
            catch (System.Exception e) {
                Debug.LogError("翼尖涡流计算错误: " + e.Message);
                targetVisibility = 0;
            }
        }
        
        // 平滑过渡
        if (targetVisibility > currentVisibility)
        {
            // 淡入
            currentVisibility = Mathf.MoveTowards(currentVisibility, targetVisibility, Time.deltaTime / fadeInTime);
        }
        else
        {
            // 淡出
            currentVisibility = Mathf.MoveTowards(currentVisibility, targetVisibility, Time.deltaTime / fadeOutTime);
        }
        
        // 应用可见度
        SetVisibility(currentVisibility);
    }
    
    // 设置粒子系统可见度
    void SetVisibility(float visibility)
    {
        try
        {
            if (ps != null)
            {
                // 处理alwaysShow标志
                if (alwaysShow)
                {
                    visibility = 1.0f;
                }
                
                // 调整发射率
                var emissionRate = em.rateOverTime;
                float baseRate = 10f; // 基础发射率，可以根据粒子系统设置调整
                emissionRate.constant = baseRate * visibility;
                em.rateOverTime = emissionRate;
                em.enabled = alwaysShow || visibility > 0.01f;
                
                // 调整透明度
                var startColor = main.startColor;
                Color color = startColor.color;
                color.a = visibility; // 直接设置透明度，而不是乘以现有值
                var newStartColor = new ParticleSystem.MinMaxGradient(color);
                main.startColor = newStartColor;
                
                // 处理粒子系统播放状态
                if (alwaysShow || visibility > 0.01f)
                {
                    if (!ps.isPlaying)
                    {
                        ps.Play();
                    }
                }
                else if (visibility <= 0.01f && ps.isPlaying)
                {
                    ps.Stop();
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("设置翼尖涡流可见度时出错: " + e.Message);
        }
    }

    void OnDrawGizmos()
    {
        try
        {
            if (Application.isPlaying && enabled && airplaneTransform != null)
            {
                // 显示力方向
                Gizmos.color = Color.red;
                Vector3 forceDir = new Vector3(fx, fy, fz).normalized;
                Gizmos.DrawRay(transform.position, forceDir);
                
                // 如果设置了alwaysShow，在Gizmos中显示提示
                if (alwaysShow || showDebugInfo)
                {
                    // 显示涡流可见度状态
                    float visibilityToShow = alwaysShow ? 1.0f : currentVisibility;
                    Gizmos.color = new Color(0, 1, 0, visibilityToShow);
                    Gizmos.DrawSphere(transform.position, 0.5f);
                    
                    // 显示触发条件状态
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireSphere(transform.position, visibilityToShow * 0.3f);
                    
                    #if UNITY_EDITOR
                    if (alwaysShow)
                    {
                        // 使用安全的方式添加文本标签
                        try
                        {
                            UnityEditor.Handles.Label(transform.position + Vector3.up * 0.5f, "AlwaysShow: 强制显示");
                        }
                        catch {}
                    }
                    #endif
                }
            }
        }
        catch (System.Exception e)
        {
            // Gizmos错误不需要显示，因为它们不影响游戏运行
        }
    }
}

