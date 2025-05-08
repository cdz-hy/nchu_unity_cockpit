// using UnityEngine;

// [RequireComponent(typeof(ParticleSystem))]
// public class DynamicSmokeEffect : MonoBehaviour
// {
//     private ParticleSystem ps;
//     private ParticleSystem.ForceOverLifetimeModule forceModule;

//     // 用于记录上一帧的位置
//     private Vector3 previousPosition;

//     // 用于计算时间间隔
//     private float previousTime;

//     // 力度缩放系数（可以根据需要调整）
//     public float forceScale = 1.0f;


//     [SerializeField] private float smoothTime = 0.3f;
//     private Vector3 smoothedVelocity = Vector3.zero;

//     void Start()
//     {
//         // 获取并缓存ParticleSystem组件
//         ps = GetComponent<ParticleSystem>();
//         forceModule = ps.forceOverLifetime;

//         // 初始化上一帧位置为当前位置
//         previousPosition = transform.position;
//         previousTime = Time.time;
//     }

//     void Update()
//     {
//         // 获取当前时间和位置
//         float currentTime = Time.time;
//         Vector3 currentPosition = transform.position;

//         // 计算时间差值（避免除以0）
//         float deltaTime = currentTime - previousTime;
//         if (deltaTime <= 0f)
//             return;

//         // 计算瞬时速度（单位：m/s）
//         // Vector3 velocity = (currentPosition - previousPosition) / deltaTime;

//         Vector3 rawVelocity = (currentPosition - previousPosition) / deltaTime;
//         smoothedVelocity = Vector3.Lerp(smoothedVelocity, rawVelocity, Time.deltaTime / smoothTime);
        

//         // 应用速度到粒子系统的 Force Over Lifetime 模块
//         // ApplyForce(velocity);
//         ApplyForce(smoothedVelocity);


//         // 更新上一帧的数据
//         previousPosition = currentPosition;
//         previousTime = currentTime;
//     }

//     void ApplyForce(Vector3 velocity)
//     {
//         // 设置XYZ三个轴的力，使用 MinMaxCurve 单值即可
//         // 可选：你可以对速度进行归一化后乘以一个固定强度，或者直接按比例使用原始速度大小
//         forceModule.x = new ParticleSystem.MinMaxCurve(velocity.x * forceScale);
//         forceModule.y = new ParticleSystem.MinMaxCurve(velocity.y * forceScale);
//         forceModule.z = new ParticleSystem.MinMaxCurve(velocity.z * forceScale);
//     }
// }


using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class EngineSmoke : MonoBehaviour
{
    public Transform airplaneTransform; // 飞机物体
    public float forceFactor = 0.3f;     // 加速度映射系数
    public float upDrift = 0.1f;         // 恒定向上漂浮力
    public float baseZForce = -1.0f;     // 初始 Z 向力（向后）

    private ParticleSystem ps;
    private ParticleSystem.ForceOverLifetimeModule fo;
    private Vector3 lastPos;
    private Vector3 lastVel;

    // 用于显示的当前力值
    private float fx, fy, fz;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        fo = ps.forceOverLifetime;
        fo.enabled = true;
        fo.space = ParticleSystemSimulationSpace.World;

        if (airplaneTransform == null)
        {
            Debug.LogError("请在 Inspector 中为 EngineSmoke 分配 airplaneTransform");
            enabled = false;
            return;
        }

        lastPos = airplaneTransform.position;
        lastVel = Vector3.zero;
    }

    void Update()
    {
        Vector3 currPos = airplaneTransform.position;
        Vector3 velocity = (currPos - lastPos) / Time.deltaTime;
        Vector3 acceleration = (velocity - lastVel) / Time.deltaTime;

        lastPos = currPos;
        lastVel = velocity;

        // 原始力计算
        fx = -acceleration.x * forceFactor;
        fy = -acceleration.y * forceFactor + upDrift;

        // 限制在 [-1.3, 1.3]
        fx = Mathf.Clamp(fx, -1.3f, 1.3f);
        fy = Mathf.Clamp(fy, -1.3f, 1.3f);

        // 应用到 Force over Lifetime
        fo.x = new ParticleSystem.MinMaxCurve(fx);
        fo.y = new ParticleSystem.MinMaxCurve(fy);
    }

}

