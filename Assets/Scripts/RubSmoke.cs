using System;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class RubSmoke : MonoBehaviour
{
    [Header("引用设置")]
    public Transform airplaneTransform;    // 飞机物体，用于获取高度

    [Header("条件参数")]
    public float maxGroundHeight = 0.5f;   // 地面高度阈值 (单位: 米)
    public float minDeceleration = 0.5f;     // 最小减速度阈值，用于激活烟雾

    // [Header("烟雾参数")]
    // public float emissionRate = 50f;       // 烟雾发射速率

    private ParticleSystem ps;
    private ParticleSystem.EmissionModule emission;

    private float lastSpeed;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        emission = ps.emission;
        emission.enabled = false;

        if (airplaneTransform == null)
        {
            Debug.LogError("请在 Inspector 中为 RubSmoke 分配 airplaneTransform");
            enabled = false;
            return;
        }

        // 初始化上一次速度
        lastSpeed = DataCenter.Instance.airSpeed;
    }

    void Update()
    {
        if (!enabled) return;

        // 1. 当前高度判断
        float height = airplaneTransform.position.y;
        bool nearGround = height >= -maxGroundHeight && height <= maxGroundHeight;

        // 2. 当前速度减速判断
        float currentSpeed = DataCenter.Instance.airSpeed;
        float deceleration = (lastSpeed - currentSpeed)/Time.deltaTime;
        bool isDecelerating = deceleration >= minDeceleration;

        // 更新 lastSpeed
        lastSpeed = currentSpeed;

        // 3. 控制烟雾发射
        bool shouldEmit = nearGround && isDecelerating;
        if (shouldEmit && !emission.enabled)
        {
            emission.enabled = true;
            // emission.rateOverTime = emissionRate;
        }
        else if (!shouldEmit && emission.enabled)
        {
            emission.enabled = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (airplaneTransform != null)
        {
            // 可视化地面高度阈值
            Gizmos.color = Color.yellow;
            Vector3 p = new Vector3(airplaneTransform.position.x, maxGroundHeight, airplaneTransform.position.z);
            Gizmos.DrawWireSphere(p, 0.2f);
        }
    }
}
