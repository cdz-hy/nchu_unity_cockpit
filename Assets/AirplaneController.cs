// using System.Collections;
// using UnityEngine;

// public class AirplaneController : MonoBehaviour
// {
//     // 平滑参数
//     public float positionSmoothSpeed = 100f;  // 位置平滑速度 [[5]]
//     public float rotationSmoothSpeed = 100f;

//     // 地理参数
//     private const float centerLat = 28.85125f;
//     private const float centerLon = 115.896f;
//     // 用更精确的公式计算转换系数（基于 WGS84 公式）
//     private float metersPerDegreeLat;
//     private float metersPerDegreeLon;

//     // 高度参数
//     private float initialAltitude; // 初始高度（英尺）
//     private const float feetToMeters = 0.3048f;

//     // 状态变量
//     private Vector3 targetPosition;
//     private Vector3 velocity = Vector3.zero;
    
//     void Start()
//     {
//         // 使用 WGS84 公式计算更精确的每度转换系数（基于 centerLat）
//         float latRad = centerLat * Mathf.Deg2Rad;
//         metersPerDegreeLat = 111132.92f - 559.82f * Mathf.Cos(2 * latRad)
//                              + 1.175f * Mathf.Cos(4 * latRad)
//                              - 0.0023f * Mathf.Cos(6 * latRad);
//         metersPerDegreeLon = 111412.84f * Mathf.Cos(latRad)
//                              - 93.5f * Mathf.Cos(3 * latRad)
//                              + 0.118f * Mathf.Cos(5 * latRad);

//         // 使用协程等待 DataCenter 数据初始化后再调用 InitializePosition()
//         StartCoroutine(WaitForDataCenter());
//     }

//     IEnumerator WaitForDataCenter()
//     {
//         // 循环等待直到 DataCenter 不为 null 且 latitude 与 longitude 不为 0
//         while (DataCenter.Instance == null || 
//                (DataCenter.Instance.latitude == 0 && DataCenter.Instance.longitude == 0))
//         {
//             yield return null;
//         }
//         InitializePosition();
//         Debug.Log("初始化位置成功");
//     }

//     void InitializePosition()
//     {
//         // 记录初始高度
//         initialAltitude = DataCenter.Instance.altitude;

//         // 计算初始坐标
//         float deltaLat = DataCenter.Instance.latitude - centerLat;
//         float deltaLon = DataCenter.Instance.longitude - centerLon;
//         // 根据要求：x轴正方向为正西，所以取 -(deltaLon)
//         float x = -deltaLon * metersPerDegreeLon;
//         // 根据要求：z轴负方向为正北，所以 z = -deltaLat * metersPerDegreeLat
//         float z = -deltaLat * metersPerDegreeLat;
//         float y = (DataCenter.Instance.altitude - initialAltitude) * feetToMeters;

//         transform.position = new Vector3(x, y, z);
//     }

//     void Update()
//     {
//         if (DataCenter.Instance == null || (DataCenter.Instance.latitude == 0 && DataCenter.Instance.longitude == 0))
//             return;

//         // ====== 平滑位置更新 ======
//         float currentLat = DataCenter.Instance.latitude;
//         float currentLon = DataCenter.Instance.longitude;
//         float altitudeFeet = DataCenter.Instance.altitude;
//         Debug.Log(initialAltitude);

//         float deltaLat = currentLat - centerLat;
//         float deltaLon = currentLon - centerLon;
//         float x = -deltaLon * metersPerDegreeLon;
//         float z = -deltaLat * metersPerDegreeLat;
//         float y = (altitudeFeet - initialAltitude) * feetToMeters;
//         targetPosition = new Vector3(x, y, z);

//         // 如果 x、y 或 z 方向变化过大（大于100单位），直接跳转到目标位置
//         if (Mathf.Abs(targetPosition.x - transform.position.x) > 100f ||
//             Mathf.Abs(targetPosition.z - transform.position.z) > 100f ||
//             Mathf.Abs(targetPosition.y - transform.position.y) > 100f)
//         {
//             transform.position = targetPosition;
//         }
//         else
//         {
//             // 使用 SmoothDamp 实现插值，平滑时间直接设置为一帧时间（Time.deltaTime）
//             transform.position = Vector3.SmoothDamp(
//                 transform.position,
//                 targetPosition,
//                 ref velocity,
//                 Time.deltaTime,
//                 positionSmoothSpeed
//             );
//         }

//         // ====== 平滑旋转更新 ======
//         float pitch = DataCenter.Instance.pitchAngle;
//         float roll = DataCenter.Instance.rollAngle;
//         float yaw = DataCenter.Instance.rotationAngle;

//         Quaternion targetRotation = Quaternion.Euler(pitch, yaw, roll);
//         transform.rotation = Quaternion.Lerp(
//             transform.rotation,
//             targetRotation,
//             rotationSmoothSpeed * Time.deltaTime
//         );
//     }
// }

// using System.Collections;
// using UnityEngine;

// public class AirplaneController : MonoBehaviour
// {
//     // 平滑参数
//     public float positionSmoothSpeed = 10f;  // 位置平滑速度 [[5]]
//     public float rotationSmoothSpeed = 10f;

//     // 地理参数
//     private const float centerLat = 28.85125f;
//     private const float centerLon = 115.896f;

//     // 用更精确的公式计算转换系数（基于 WGS84 公式）
//     private float metersPerDegreeLat;
//     private float metersPerDegreeLon;

//     // 高度参数
//     private const float initialAltitude = 151.8441f; // 初始点高度（基准高度（英尺））
//     private const float feetToMeters = 0.3048f;

//     // 状态变量
//     private Vector3 targetPosition;
//     private Vector3 velocity = Vector3.zero;
    
//     void Start()
//     {
//         // 使用 WGS84 公式计算更精确的每度转换系数（基于 centerLat）
//         float latRad = centerLat * Mathf.Deg2Rad;
//         metersPerDegreeLat = 111132.92f - 559.82f * Mathf.Cos(2 * latRad)
//                              + 1.175f * Mathf.Cos(4 * latRad)
//                              - 0.0023f * Mathf.Cos(6 * latRad);
//         metersPerDegreeLon = 111412.84f * Mathf.Cos(latRad)
//                              - 93.5f * Mathf.Cos(3 * latRad)
//                              + 0.118f * Mathf.Cos(5 * latRad);

//         // 使用协程等待 DataCenter 数据初始化后再调用 InitializePosition()
//         StartCoroutine(WaitForDataCenter());
//     }

//     IEnumerator WaitForDataCenter()
//     {
//         // 循环等待直到 DataCenter 不为 null 且 latitude 与 longitude 不为 0
//         while (DataCenter.Instance == null || 
//                (DataCenter.Instance.latitude == 0 && DataCenter.Instance.longitude == 0))
//         {
//             yield return null;
//         }
//         InitializePosition();
//         // Debug.Log("初始化位置成功");
//     }

//     void InitializePosition()
//     {

//         // 计算初始坐标
//         float deltaLat = DataCenter.Instance.latitude - centerLat;
//         float deltaLon = DataCenter.Instance.longitude - centerLon;
//         // 根据要求：x轴正方向为正西，所以取 -(deltaLon)
//         float x = -deltaLon * metersPerDegreeLon;
//         // 根据要求：z轴负方向为正北，所以 z = -deltaLat * metersPerDegreeLat
//         float z = -deltaLat * metersPerDegreeLat;
//         float y = (DataCenter.Instance.altitude - initialAltitude) * feetToMeters;

//         transform.position = new Vector3(x, y, z);
//     }

//     // void Update()
//     // {
//     //     if (DataCenter.Instance == null || (DataCenter.Instance.latitude == 0 && DataCenter.Instance.longitude == 0))
//     //         return;

//     //     // ====== 平滑位置更新 ======
//     //     float currentLat = DataCenter.Instance.latitude;
//     //     float currentLon = DataCenter.Instance.longitude;
//     //     float altitudeFeet = DataCenter.Instance.altitude;

//     //     float deltaLat = currentLat - centerLat;
//     //     float deltaLon = currentLon - centerLon;
//     //     float x = -deltaLon * metersPerDegreeLon;
//     //     float z = -deltaLat * metersPerDegreeLat;
//     //     float y = (altitudeFeet - initialAltitude) * feetToMeters;
//     //     targetPosition = new Vector3(x, y, z);

//     //     // 如果 x、y 或 z 方向变化过大（大于100单位），直接跳转到目标位置
//     //     if (Mathf.Abs(targetPosition.x - transform.position.x) > 1000f ||
//     //         Mathf.Abs(targetPosition.z - transform.position.z) > 1000f ||
//     //         Mathf.Abs(targetPosition.y - transform.position.y) > 1000f)
//     //     {
//     //         transform.position = targetPosition;
//     //     }
//     //     else
//     //     {
//     //         // 使用 SmoothDamp 实现插值，平滑时间直接设置为一帧时间（Time.deltaTime）
//     //         transform.position = Vector3.SmoothDamp(
//     //             transform.position,
//     //             targetPosition,
//     //             ref velocity,
//     //             Time.deltaTime,
//     //             positionSmoothSpeed
//     //         );
//     //     }

//     //     // ====== 平滑旋转更新 ======
//     //     float pitch = DataCenter.Instance.pitchAngle;
//     //     float roll = DataCenter.Instance.rollAngle;
//     //     float yaw = DataCenter.Instance.rotationAngle;

//     //     Quaternion targetRotation = Quaternion.Euler(pitch, yaw, roll);
//     //     transform.rotation = Quaternion.Lerp(
//     //         transform.rotation,
//     //         targetRotation,
//     //         rotationSmoothSpeed * Time.deltaTime
//     //     );
//     // }


//     void Update()
//     {
//         if (DataCenter.Instance == null || 
//             (DataCenter.Instance.latitude == 0 && DataCenter.Instance.longitude == 0))
//             return;

//         // 计算 targetPosition（与你原来一致）
//         float deltaLat = DataCenter.Instance.latitude - centerLat;
//         float deltaLon = DataCenter.Instance.longitude - centerLon;
//         float x = -deltaLon * metersPerDegreeLon;
//         float z = -deltaLat * metersPerDegreeLat;
//         float y = (DataCenter.Instance.altitude - initialAltitude) * feetToMeters;
//         Vector3 targetPosition = new Vector3(x, y, z);

//         // 平滑移动：SmoothDamp + 阈值判断
//         const float stopThreshold = 0.01f;
//         if ((transform.position - targetPosition).sqrMagnitude < stopThreshold * stopThreshold)
//         {
//             transform.position = targetPosition;
//             velocity = Vector3.zero;
//         }
//         else
//         {
//             float smoothTime = 1f / positionSmoothSpeed;  // 0.1 秒 阻尼时间
//             transform.position = Vector3.SmoothDamp(
//                 transform.position,
//                 targetPosition,
//                 ref velocity,
//                 smoothTime,
//                 Mathf.Infinity,
//                 Time.deltaTime
//             );
//         }

//         // 旋转部分可保留原 Lerp 写法
//         float pitch = DataCenter.Instance.pitchAngle;
//         float roll  = DataCenter.Instance.rollAngle;
//         float yaw   = DataCenter.Instance.rotationAngle;
//         Quaternion targetRot = Quaternion.Euler(pitch, yaw, roll);
//         transform.rotation = Quaternion.Lerp(
//             transform.rotation,
//             targetRot,
//             rotationSmoothSpeed * Time.deltaTime
//         );
//     }


// }




using System.Collections;
using UnityEngine;

public class AirplaneController : MonoBehaviour
{
    // 平滑参数
    public float positionSmoothSpeed = 10f;  // 位置平滑速度 [[5]]
    public float rotationSmoothSpeed = 10f;

    // 地理参数
    private const float centerLat = 28.85125f;
    private const float centerLon = 115.896f;

    // 用更精确的公式计算转换系数（基于 WGS84 公式）
    private float metersPerDegreeLat;
    private float metersPerDegreeLon;

    // 高度参数
    private const float initialAltitude = 151.8441f; // 初始点高度（基准高度（英尺））
    private const float feetToMeters = 0.3048f;

    // 状态变量
    private Vector3 targetPosition;
    private Vector3 velocity = Vector3.zero;
    
    void Start()
    {
        // 使用 WGS84 公式计算更精确的每度转换系数（基于 centerLat）
        float latRad = centerLat * Mathf.Deg2Rad;
        metersPerDegreeLat = 111132.92f - 559.82f * Mathf.Cos(2 * latRad)
                             + 1.175f * Mathf.Cos(4 * latRad)
                             - 0.0023f * Mathf.Cos(6 * latRad);
        metersPerDegreeLon = 111412.84f * Mathf.Cos(latRad)
                             - 93.5f * Mathf.Cos(3 * latRad)
                             + 0.118f * Mathf.Cos(5 * latRad);

        // 使用协程等待 DataCenter 数据初始化后再调用 InitializePosition()
        StartCoroutine(WaitForDataCenter());
    }

    IEnumerator WaitForDataCenter()
    {
        // 循环等待直到 DataCenter 不为 null 且 latitude 与 longitude 不为 0
        while (DataCenter.Instance == null || 
               (DataCenter.Instance.latitude == 0 && DataCenter.Instance.longitude == 0))
        {
            yield return null;
        }
        InitializePosition();
        // Debug.Log("初始化位置成功");
    }

    void InitializePosition()
    {

        // 计算初始坐标
        float deltaLat = DataCenter.Instance.latitude - centerLat;
        float deltaLon = DataCenter.Instance.longitude - centerLon;
        // 根据要求：x轴正方向为正西，所以取 -(deltaLon)
        float x = -deltaLon * metersPerDegreeLon;
        // 根据要求：z轴负方向为正北，所以 z = -deltaLat * metersPerDegreeLat
        float z = -deltaLat * metersPerDegreeLat;
        float y = (DataCenter.Instance.altitude - initialAltitude) * feetToMeters;

        transform.position = new Vector3(x, y, z);
    }

    void Update()
    {
        if (DataCenter.Instance == null || 
            (DataCenter.Instance.latitude == 0 && DataCenter.Instance.longitude == 0))
            return;

        // 计算 targetPosition
        float deltaLat = DataCenter.Instance.latitude - centerLat;
        float deltaLon = DataCenter.Instance.longitude - centerLon;
        float x = -deltaLon * metersPerDegreeLon;
        float z = -deltaLat * metersPerDegreeLat; 
        float y = (DataCenter.Instance.altitude - initialAltitude) * feetToMeters;
        Vector3 targetPosition = new Vector3(x%2000, y%2000, z%2000);
        

        // 平滑移动
        const float stopThreshold = 0.01f;
        if ((transform.position - targetPosition).sqrMagnitude < stopThreshold * stopThreshold)
        {
            transform.position = targetPosition;
            velocity = Vector3.zero;
        }
        else
        {
            float smoothTime = 1f / positionSmoothSpeed;  // 0.1 秒 阻尼时间
            transform.position = Vector3.SmoothDamp(
                transform.position,
                targetPosition,
                ref velocity,
                smoothTime,
                Mathf.Infinity,
                Time.deltaTime
            );
        }

        // 旋转部分可保留原 Lerp 写法
        float pitch = DataCenter.Instance.pitchAngle;
        float roll  = DataCenter.Instance.rollAngle;
        float yaw   = DataCenter.Instance.rotationAngle;
        Quaternion targetRot = Quaternion.Euler(pitch, yaw, roll);
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRot,
            rotationSmoothSpeed * Time.deltaTime
        );
    }


}
