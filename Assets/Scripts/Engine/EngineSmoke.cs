using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class EngineSmoke : MonoBehaviour
{
    [Header("引用设置")]
    public Transform airplaneTransform; // 飞机物体

    [Header("物理参数")]
    public float forceFactor = 0.03f;     // 加速度映射系数
    public float upDrift = 0.1f;          // 恒定向上漂浮力（增大）
    public float backDrift = 0.2f;        // 恒定向后漂浮力（增大）
    public float dragFactor = 0.005f;     // 阻力系数（进一步降低以允许更多摆动）
    public float turbulenceFactor = 0.08f; // 湍流强度（进一步增大）
    
    [Header("摆动参数")]
    public float swayXStrength = 0.12f;   // X轴摆动强度（增大一倍）
    public float swayYStrength = 0.15f;   // Y轴摆动强度（增大近一倍）
    public float swayXSpeed = 1.2f;       // X轴摆动速度（稍微降低使摆动更明显）
    public float swayYSpeed = 0.8f;       // Y轴摆动速度（稍微降低使摆动更明显）
    public float complexMotionFactor = 0.6f; // 复合运动强度因子（新增）
    
    [Header("限制参数")]
    public float maxForce = 0.7f;         // 最大力值限制（增大）
    public float smoothingFactor = 0.1f;  // 平滑过渡系数（略微降低使变化更快）
    public bool applyVelocity = false;    // 是否应用速度
    
    [Header("调试")]
    public bool showDebugInfo = false;    // 显示调试信息

    private ParticleSystem ps;
    private ParticleSystem.ForceOverLifetimeModule fo;
    private ParticleSystem.VelocityOverLifetimeModule vo;
    private Vector3 lastPos;
    private Vector3 lastVel;
    private Vector3 currentForce;

    // 用于显示的当前力值
    private float fx, fy, fz;
    
    // 用于生成周期性摆动
    private float timeOffset;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        fo = ps.forceOverLifetime;
        vo = ps.velocityOverLifetime;
        
        // 确保力模块启用并设置为世界空间
        fo.enabled = true;
        fo.space = ParticleSystemSimulationSpace.World;
        
        // 默认禁用速度模块，由applyVelocity控制
        vo.enabled = false;

        if (airplaneTransform == null)
        {
            Debug.LogError("请在 Inspector 中为 EngineSmoke 分配 airplaneTransform");
            enabled = false;
            return;
        }

        lastPos = airplaneTransform.position;
        lastVel = Vector3.zero;
        currentForce = Vector3.zero;
        
        // 初始化力为垂直向上和向后(Z正方向)
        currentForce = new Vector3(0, upDrift, backDrift);
        
        // 随机初始时间偏移，使多个引擎的烟雾不同步
        timeOffset = Random.Range(0f, 10f);
    }

    void Update()
    {
        if (!enabled || airplaneTransform == null)
            return;
            
        Vector3 currPos = airplaneTransform.position;
        Vector3 velocity = (currPos - lastPos) / Time.deltaTime;
        Vector3 acceleration = (velocity - lastVel) / Time.deltaTime;

        // 更新历史数据
        lastPos = currPos;
        lastVel = velocity;
        
        // 添加周期性摆动（正弦波）- 使烟雾有更明显的摆动
        float time = Time.time + timeOffset;
        
        // 主要摆动
        float swayX = Mathf.Sin(time * swayXSpeed) * swayXStrength; 
        float swayY = Mathf.Cos(time * swayYSpeed) * swayYStrength;
        
        // 添加复合摆动，使运动更加复杂
        swayX += Mathf.Sin(time * swayXSpeed * 2.3f) * (swayXStrength * complexMotionFactor);
        swayY += Mathf.Cos(time * swayYSpeed * 1.7f) * (swayYStrength * complexMotionFactor);
        
        // 添加第三层摆动，使运动更加自然
        swayX += Mathf.Sin(time * swayXSpeed * 3.7f) * (swayXStrength * complexMotionFactor * 0.3f);
        swayY += Mathf.Cos(time * swayYSpeed * 2.9f) * (swayYStrength * complexMotionFactor * 0.3f);

        // 计算目标力 - 考虑坐标系方向：z正向是飞机后方，y正向是向上，x正向是向左
        Vector3 targetForce = new Vector3(
            swayX, // 添加X轴周期性摆动（增大）
            -acceleration.y * forceFactor + upDrift + swayY, // Y正向是向上，添加摆动（增大）
            acceleration.z * forceFactor + backDrift  // Z正向是向后
        );
        
        // 添加基于速度的阻力 - 注意坐标系方向，降低阻力以允许更多摆动
        targetForce += new Vector3(
            -velocity.x * dragFactor * 0.3f, // 进一步减小X方向的阻力
            -velocity.y * dragFactor * 0.5f, // 减小Y方向的阻力
            -velocity.z * dragFactor
        );
        
        // 添加随机湍流 - 增加湍流强度
        targetForce += new Vector3(
            Random.Range(-turbulenceFactor * 1.5f, turbulenceFactor * 1.5f), // 增加X轴湍流
            Random.Range(-turbulenceFactor * 1.5f, turbulenceFactor * 1.5f), // 增加Y轴湍流
            Random.Range(-turbulenceFactor * 0.5f, turbulenceFactor * 0.5f) // Z轴湍流稍小
        );

        // 平滑过渡到新的力值
        currentForce = Vector3.Lerp(currentForce, targetForce, smoothingFactor);
        
        // 限制力的大小
        fx = Mathf.Clamp(currentForce.x, -maxForce, maxForce); // 允许更大的X轴力
        fy = Mathf.Clamp(currentForce.y, -maxForce * 0.3f, maxForce); // Y轴向下的力限制更多
        fz = Mathf.Clamp(currentForce.z, 0, maxForce); // Z轴只允许正向力（向后）
        
        // 应用到 Force over Lifetime
        fo.x = new ParticleSystem.MinMaxCurve(fx);
        fo.y = new ParticleSystem.MinMaxCurve(fy);
        fo.z = new ParticleSystem.MinMaxCurve(fz);
        
        // 只有在启用时才应用速度
        if (applyVelocity)
        {
            vo.enabled = true;
            vo.space = ParticleSystemSimulationSpace.World;
            // 应用初始速度影响
            vo.x = new ParticleSystem.MinMaxCurve(velocity.x * 0.02f); // 增加X轴速度影响
            vo.y = new ParticleSystem.MinMaxCurve(velocity.y * 0.03f); // 增加Y轴速度影响
            vo.z = new ParticleSystem.MinMaxCurve(velocity.z * 0.02f); // 增加Z轴速度影响
        }
        else
        {
            vo.enabled = false;
        }
        
        // 调试信息
        if (showDebugInfo)
        {
            Debug.Log($"烟雾力: X={fx:F4}(左+), Y={fy:F4}(上+), Z={fz:F4}(后+), SwayX={swayX:F4}, SwayY={swayY:F4}");
        }
    }

    // 在编辑器中显示当前力的方向
    void OnDrawGizmos()
    {
        if (Application.isPlaying && enabled && airplaneTransform != null)
        {
            Gizmos.color = Color.red;
            Vector3 forceDir = new Vector3(fx, fy, fz).normalized;
            Gizmos.DrawRay(transform.position, forceDir);
        }
    }
}

