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




//using System.Collections;
//using UnityEngine;

//public class AirplaneController : MonoBehaviour
//{
//    // 平滑参数
//    public float positionSmoothSpeed = 10f;  // 位置平滑速度 [[5]]
//    public float rotationSmoothSpeed = 10f;

//    // 地理参数
//    private const float centerLat = 28.85125f;
//    private const float centerLon = 115.896f;

//    // 用更精确的公式计算转换系数（基于 WGS84 公式）
//    private float metersPerDegreeLat;
//    private float metersPerDegreeLon;

//    // 高度参数
//    private const float initialAltitude = 151.8441f; // 初始点高度（基准高度（英尺））
//    private const float feetToMeters = 0.3048f;

//    // 状态变量
//    private Vector3 targetPosition;
//    private Vector3 velocity = Vector3.zero;

//    void Start()
//    {
//        // 使用 WGS84 公式计算更精确的每度转换系数（基于 centerLat）
//        float latRad = centerLat * Mathf.Deg2Rad;
//        metersPerDegreeLat = 111132.92f - 559.82f * Mathf.Cos(2 * latRad)
//                             + 1.175f * Mathf.Cos(4 * latRad)
//                             - 0.0023f * Mathf.Cos(6 * latRad);
//        metersPerDegreeLon = 111412.84f * Mathf.Cos(latRad)
//                             - 93.5f * Mathf.Cos(3 * latRad)
//                             + 0.118f * Mathf.Cos(5 * latRad);

//        // 使用协程等待 DataCenter 数据初始化后再调用 InitializePosition()
//        StartCoroutine(WaitForDataCenter());
//    }

//    IEnumerator WaitForDataCenter()
//    {
//        // 循环等待直到 DataCenter 不为 null 且 latitude 与 longitude 不为 0
//        while (DataCenter.Instance == null || 
//               (DataCenter.Instance.latitude == 0 && DataCenter.Instance.longitude == 0))
//        {
//            yield return null;
//        }
//        InitializePosition();
//        // Debug.Log("初始化位置成功");
//    }

//    void InitializePosition()
//    {

//        // 计算初始坐标
//        float deltaLat = DataCenter.Instance.latitude - centerLat;
//        float deltaLon = DataCenter.Instance.longitude - centerLon;
//        // 根据要求：x轴正方向为正西，所以取 -(deltaLon)
//        float x = -deltaLon * metersPerDegreeLon;
//        // 根据要求：z轴负方向为正北，所以 z = -deltaLat * metersPerDegreeLat
//        float z = -deltaLat * metersPerDegreeLat;
//        float y = (DataCenter.Instance.altitude - initialAltitude) * feetToMeters;

//        transform.position = new Vector3(x, y, z);
//    }

//    void Update()
//    {
//        if (DataCenter.Instance == null || 
//            (DataCenter.Instance.latitude == 0 && DataCenter.Instance.longitude == 0))
//            return;

//        // 计算 targetPosition
//        float deltaLat = DataCenter.Instance.latitude - centerLat;
//        float deltaLon = DataCenter.Instance.longitude - centerLon;
//        float x = -deltaLon * metersPerDegreeLon;
//        float z = -deltaLat * metersPerDegreeLat; 
//        float y = (DataCenter.Instance.altitude - initialAltitude) * feetToMeters;
//        Vector3 targetPosition = new Vector3(x, y, z);

//        // 平滑移动
//        const float stopThreshold = 0.01f;
//        if ((transform.position - targetPosition).sqrMagnitude < stopThreshold * stopThreshold)
//        {
//            transform.position = targetPosition;
//            velocity = Vector3.zero;
//        }
//        else
//        {
//            float smoothTime = 30f / positionSmoothSpeed;  // 0.1 秒 阻尼时间
//            transform.position = Vector3.SmoothDamp(
//                transform.position,
//                targetPosition,
//                ref velocity,
//                smoothTime,
//                Mathf.Infinity,
//                Time.deltaTime
//            );
//        }

//        // 旋转部分可保留原 Lerp 写法
//        float pitch = DataCenter.Instance.pitchAngle;
//        float roll  = DataCenter.Instance.rollAngle;
//        float yaw   = DataCenter.Instance.rotationAngle;
//        Quaternion targetRot = Quaternion.Euler(pitch, yaw, roll);
//        transform.rotation = Quaternion.Lerp(
//            transform.rotation,
//            targetRot,
//            rotationSmoothSpeed * Time.deltaTime
//        );
//    }


//}



//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class AirplaneController : MonoBehaviour
//{

//    public Material flightPathMaterial;


//    // —— 平滑参数 —— 
//    public float positionSmoothSpeed = 10f;
//    public float rotationSmoothSpeed = 10f;

//    // —— 地理参数 —— 
//    private const float centerLat = 28.85125f;
//    private const float centerLon = 115.896f;
//    private float metersPerDegreeLat;
//    private float metersPerDegreeLon;

//    // —— 高度参数 —— 
//    private const float initialAltitude = 151.8441f; // 英尺
//    private const float feetToMeters = 0.3048f;

//    // —— 状态变量 —— 
//    private Vector3 velocity = Vector3.zero;

//    // —— 轨迹记录 —— 
//    private class FlightState
//    {
//        public float time;
//        public Vector3 position;
//        public Vector3 eulerAngles;
//        public FlightState(float t, Vector3 pos, Vector3 eul)
//        {
//            time = t;
//            position = pos;
//            eulerAngles = eul;
//        }
//    }
//    private List<FlightState> flightStates = new List<FlightState>();
//    private LineRenderer lineRenderer;

//    void Start()
//    {
//        // 计算 WGS84 每度转换系数
//        float latRad = centerLat * Mathf.Deg2Rad;
//        metersPerDegreeLat = 111132.92f - 559.82f * Mathf.Cos(2 * latRad)
//                             + 1.175f * Mathf.Cos(4 * latRad)
//                             - 0.0023f * Mathf.Cos(6 * latRad);
//        metersPerDegreeLon = 111412.84f * Mathf.Cos(latRad)
//                             - 93.5f * Mathf.Cos(3 * latRad)
//                             + 0.118f * Mathf.Cos(5 * latRad);

//        // 获取或添加 LineRenderer
//        lineRenderer = GetComponent<LineRenderer>();
//        if (lineRenderer == null)
//            lineRenderer = gameObject.AddComponent<LineRenderer>();
//        lineRenderer.positionCount = 0;
//        lineRenderer.widthCurve = AnimationCurve.Constant(0, 1, 0.7f);
//        lineRenderer.material = flightPathMaterial;


//        // 等待数据中心初始化，再设置初始位置
//        StartCoroutine(WaitForDataCenter());
//    }

//    IEnumerator WaitForDataCenter()
//    {
//        while (DataCenter.Instance == null ||
//               (DataCenter.Instance.latitude == 0 && DataCenter.Instance.longitude == 0))
//        {
//            yield return null;
//        }
//        // 记录启动时刻（t=0）
//        RecordState(0f);
//        InitializePosition();
//    }

//    void InitializePosition()
//    {
//        var st = DataCenter.Instance;
//        float dx = -(st.longitude - centerLon) * metersPerDegreeLon;
//        float dz = -(st.latitude - centerLat) * metersPerDegreeLat;
//        float dy = (st.altitude - initialAltitude) * feetToMeters;
//        transform.position = new Vector3(dx, dy, dz);
//    }

//    void Update()
//    {
//        var dc = DataCenter.Instance;
//        if (dc == null ||
//            (dc.latitude == 0 && dc.longitude == 0))
//            return;

//        // —— 回放模式 —— 
//        if (dc.isReplaying && flightStates.Count > 0)
//        {
//            // clamp currentTime
//            float t = Mathf.Clamp(dc.currentTime, 0f, flightStates[flightStates.Count - 1].time);

//            // 找到时间段 [i, i+1] 并插值
//            int idx = flightStates.FindIndex(s => s.time >= t);
//            if (idx <= 0) idx = 1;
//            if (idx >= flightStates.Count) idx = flightStates.Count - 1;

//            var prev = flightStates[idx - 1];
//            var next = flightStates[idx];
//            float lerp = (t - prev.time) / (next.time - prev.time);

//            transform.position = Vector3.Lerp(prev.position, next.position, lerp);
//            transform.rotation = Quaternion.Euler(
//                Vector3.Lerp(prev.eulerAngles, next.eulerAngles, lerp)
//            );
//            return;
//        }

//        // —— 实时记录和移动 —— 
//        float currentTime = Time.timeSinceLevelLoad;
//        // 1. 计算目标位置
//        float dx2 = -(dc.longitude - centerLon) * metersPerDegreeLon;
//        float dz2 = -(dc.latitude - centerLat) * metersPerDegreeLat;
//        float dy2 = (dc.altitude - initialAltitude) * feetToMeters;
//        Vector3 targetPos = new Vector3(dx2, dy2, dz2);

//        // 2. 平滑移动
//        const float stopThreshold = 0.01f;
//        if ((transform.position - targetPos).sqrMagnitude < stopThreshold * stopThreshold)
//        {
//            transform.position = targetPos;
//            velocity = Vector3.zero;
//        }
//        else
//        {
//            float smoothTime = 30f / positionSmoothSpeed;
//            transform.position = Vector3.SmoothDamp(
//                transform.position, targetPos, ref velocity, smoothTime, Mathf.Infinity, Time.deltaTime
//            );
//        }

//        // 3. 平滑旋转
//        Quaternion targetRot = Quaternion.Euler(dc.pitchAngle, dc.rotationAngle, dc.rollAngle);
//        transform.rotation = Quaternion.Lerp(
//            transform.rotation, targetRot, rotationSmoothSpeed * Time.deltaTime
//        );

//        // 4. 记录状态
//        RecordState(currentTime);

//        // —— 完整航路显示 —— 
//        if (dc.isShowFullFlightPath)
//            UpdateLineRenderer();
//        else
//            lineRenderer.positionCount = 0;
//    }

//    private void RecordState(float t)
//    {
//        flightStates.Add(new FlightState(
//            t,
//            transform.position,
//            transform.rotation.eulerAngles
//        ));
//    }

//    private void UpdateLineRenderer()
//    {
//        int cnt = flightStates.Count;
//        lineRenderer.positionCount = cnt;
//        for (int i = 0; i < cnt; i++)
//        {
//            lineRenderer.SetPosition(i, flightStates[i].position);
//        }
//    }
//}


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class AirplaneController : MonoBehaviour
// {

//     public Material flightPathMaterial;


//     // —— 平滑参数 —— 
//     public float positionSmoothSpeed = 10f;
//     public float rotationSmoothSpeed = 10f;

//     // —— 地理参数 —— 
//     private const float centerLat = 28.85125f;
//     private const float centerLon = 115.896f;
//     private float metersPerDegreeLat;
//     private float metersPerDegreeLon;

//     // —— 高度参数 —— 
//     private const float initialAltitude = 151.8441f; // 英尺
//     private const float feetToMeters = 0.3048f;

//     // —— 状态变量 —— 
//     private Vector3 velocity = Vector3.zero;

//     // —— 轨迹记录 —— 
//     private class FlightState
//     {
//         public float time;
//         public Vector3 position;
//         public Vector3 eulerAngles;
//         public FlightState(float t, Vector3 pos, Vector3 eul)
//         {
//             time = t;
//             position = pos;
//             eulerAngles = eul;
//         }
//     }
//     private List<FlightState> flightStates = new List<FlightState>();
//     private LineRenderer lineRenderer;

//     private float totalRuntime = 0f; // 新增：记录总运行时间

//     void Start()
//     {
//         // 计算 WGS84 每度转换系数
//         float latRad = centerLat * Mathf.Deg2Rad;
//         metersPerDegreeLat = 111132.92f - 559.82f * Mathf.Cos(2 * latRad)
//                              + 1.175f * Mathf.Cos(4 * latRad)
//                              - 0.0023f * Mathf.Cos(6 * latRad);
//         metersPerDegreeLon = 111412.84f * Mathf.Cos(latRad)
//                              - 93.5f * Mathf.Cos(3 * latRad)
//                              + 0.118f * Mathf.Cos(5 * latRad);

//         // 获取或添加 LineRenderer
//         lineRenderer = GetComponent<LineRenderer>();
//         if (lineRenderer == null)
//             lineRenderer = gameObject.AddComponent<LineRenderer>();
//         lineRenderer.positionCount = 0;
//         lineRenderer.widthCurve = AnimationCurve.Constant(0, 1, 0.7f);
//         lineRenderer.material = flightPathMaterial;

//         // 等待数据中心初始化，再设置初始位置
//         StartCoroutine(WaitForDataCenter());
//     }

//     IEnumerator WaitForDataCenter()
//     {
//         while (DataCenter.Instance == null ||
//                (DataCenter.Instance.latitude == 0 && DataCenter.Instance.longitude == 0))
//         {
//             yield return null;
//         }
//         // 记录启动时刻（t=0）
//         RecordState(totalRuntime); // 使用 totalRuntime 作为时间戳
//         InitializePosition();
//     }

//     void InitializePosition()
//     {
//         var st = DataCenter.Instance;
//         float dx = -(st.longitude - centerLon) * metersPerDegreeLon;
//         float dz = -(st.latitude - centerLat) * metersPerDegreeLat;
//         float dy = (st.altitude - initialAltitude) * feetToMeters;
//         transform.position = new Vector3(dx, dy, dz);
//     }

//     void Update()
//     {
//         var dc = DataCenter.Instance;
//         if (dc == null ||
//             (dc.latitude == 0 && dc.longitude == 0))
//             return;

//         // 更新总运行时间
//         totalRuntime += Time.deltaTime;
//         DataCenter.Instance.totalRuntime = totalRuntime;

//         // —— 回放模式 —— 
//         if (dc.isReplaying && flightStates.Count > 0)
//         {
//             // clamp currentTime
//             float t = Mathf.Clamp(dc.currentTime, 0f, flightStates[flightStates.Count - 1].time);

//             // 找到时间段 [i, i+1] 并插值
//             int idx = flightStates.FindIndex(s => s.time >= t);
//             if (idx <= 0) idx = 1;
//             if (idx >= flightStates.Count) idx = flightStates.Count - 1;

//             var prev = flightStates[idx - 1];
//             var next = flightStates[idx];
//             float lerp = (t - prev.time) / (next.time - prev.time);

//             transform.position = Vector3.Lerp(prev.position, next.position, lerp);
//             transform.rotation = Quaternion.Euler(
//                 Vector3.Lerp(prev.eulerAngles, next.eulerAngles, lerp)
//             );
//             return;
//         }

//         // —— 实时记录和移动 —— 
//         // 1. 计算目标位置
//         float dx2 = -(dc.longitude - centerLon) * metersPerDegreeLon;
//         float dz2 = -(dc.latitude - centerLat) * metersPerDegreeLat;
//         float dy2 = (dc.altitude - initialAltitude) * feetToMeters;
//         Vector3 targetPos = new Vector3(dx2, dy2, dz2);

//         // 2. 平滑移动
//         const float stopThreshold = 0.01f;
//         if ((transform.position - targetPos).sqrMagnitude < stopThreshold * stopThreshold)
//         {
//             transform.position = targetPos;
//             velocity = Vector3.zero;
//         }
//         else
//         {
//             float smoothTime = 30f / positionSmoothSpeed;
//             transform.position = Vector3.SmoothDamp(
//                 transform.position, targetPos, ref velocity, smoothTime, Mathf.Infinity, Time.deltaTime
//             );
//         }

//         // 3. 平滑旋转
//         Quaternion targetRot = Quaternion.Euler(dc.pitchAngle, dc.rotationAngle, dc.rollAngle);
//         transform.rotation = Quaternion.Lerp(
//             transform.rotation, targetRot, rotationSmoothSpeed * Time.deltaTime
//         );

//         // 4. 记录状态
//         if (!dc.isReplaying)
//         {
//             RecordState(totalRuntime); // 使用 totalRuntime 作为时间戳
//         }


//         // —— 完整航路显示 —— 
//         if (dc.isShowFullFlightPath)
//             UpdateLineRenderer();
//         else
//             lineRenderer.positionCount = 0;
//     }

//     private void RecordState(float t)
//     {
//         flightStates.Add(new FlightState(
//             t,
//             transform.position,
//             transform.rotation.eulerAngles
//         ));
//     }

//     private void UpdateLineRenderer()
//     {
//         int cnt = flightStates.Count;
//         lineRenderer.positionCount = cnt;
//         for (int i = 0; i < cnt; i++)
//         {
//             lineRenderer.SetPosition(i, flightStates[i].position);
//         }
//     }
// }

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class AirplaneController : MonoBehaviour
// {
//     public Material flightPathMaterial;
//     public float positionSmoothSpeed = 10f;
//     public float rotationSmoothSpeed = 10f;

//     private const float centerLat = 28.85125f;
//     private const float centerLon = 115.896f;
//     private float metersPerDegreeLat;
//     private float metersPerDegreeLon;
//     private const float initialAltitude = 151.8441f;
//     private const float feetToMeters = 0.3048f;

//     private Vector3 velocity = Vector3.zero;
//     private LineRenderer lineRenderer;
//     private float totalRuntime = 0f;

//     // 引用 WorldManager，用来拿 totalOffset
//     public WorldManager worldManager;

//     private class FlightState
//     {
//         public float time;
//         public Vector3 position;
//         public Vector3 eulerAngles;
//         public FlightState(float t, Vector3 p, Vector3 e) { time = t; position = p; eulerAngles = e; }
//     }
//     private List<FlightState> flightStates = new List<FlightState>();

//     void Start()
//     {
//         float latRad = centerLat * Mathf.Deg2Rad;
//         metersPerDegreeLat = 111132.92f - 559.82f * Mathf.Cos(2 * latRad)
//                              + 1.175f * Mathf.Cos(4 * latRad)
//                              - 0.0023f * Mathf.Cos(6 * latRad);
//         metersPerDegreeLon = 111412.84f * Mathf.Cos(latRad)
//                              - 93.5f * Mathf.Cos(3 * latRad)
//                              + 0.118f * Mathf.Cos(5 * latRad);

//         lineRenderer = GetComponent<LineRenderer>() ?? gameObject.AddComponent<LineRenderer>();
//         lineRenderer.positionCount = 0;
//         lineRenderer.widthCurve = AnimationCurve.Constant(0, 1, 0.7f);
//         lineRenderer.material = flightPathMaterial;

//         StartCoroutine(WaitForDataCenter());
//     }

//     IEnumerator WaitForDataCenter()
//     {
//         while (DataCenter.Instance == null ||
//               (DataCenter.Instance.latitude == 0 && DataCenter.Instance.longitude == 0))
//             yield return null;

//         RecordState(0f, transform.position);
//         InitializePosition();
//     }

//     void InitializePosition()
//     {
//         var st = DataCenter.Instance;
//         Vector3 rawPos = new Vector3(
//             -(st.longitude - centerLon) * metersPerDegreeLon,
//             (st.altitude - initialAltitude) * feetToMeters,
//             -(st.latitude  - centerLat) * metersPerDegreeLat
//         );

//         // 关键修改：用 posRelative 而非 rawPos
//         Vector3 posRelative = rawPos - worldManager.GetTruePosition(Vector3.zero);
//         transform.position = posRelative;
//     }

//     void Update()
//     {
//         var dc = DataCenter.Instance;
//         if (dc == null || (dc.latitude == 0 && dc.longitude == 0))
//             return;

//         totalRuntime += Time.deltaTime;
//         dc.currentTime = totalRuntime;

//         // 1. 计算 rawPos
//         Vector3 rawPos = new Vector3(
//             -(dc.longitude - centerLon) * metersPerDegreeLon,
//             (dc.altitude - initialAltitude) * feetToMeters,
//             -(dc.latitude  - centerLat) * metersPerDegreeLat
//         );

//         // 2. 转换到 posRelative
//         Vector3 posRelative = rawPos - worldManager.GetTruePosition(Vector3.zero);

//         // 3. 回放分支：不改
//         if (dc.isReplaying && flightStates.Count > 0)
//         {
//             float t = Mathf.Clamp(dc.currentTime, 0f, flightStates[flightStates.Count - 1].time);
//             int idx = flightStates.FindIndex(s => s.time >= t);
//             idx = Mathf.Clamp(idx, 1, flightStates.Count - 1);
//             var prev = flightStates[idx - 1];
//             var next = flightStates[idx];
//             float f = (t - prev.time) / (next.time - prev.time);

//             transform.position = Vector3.Lerp(prev.position, next.position, f);
//             var pe = prev.eulerAngles; var ne = next.eulerAngles;
//             transform.rotation = Quaternion.Euler(
//                 Mathf.Lerp(pe.x, ne.x, f),
//                 Mathf.Lerp(pe.y, ne.y, f),
//                 Mathf.Lerp(pe.z, ne.z, f)
//             );
//             return;
//         }

//         // 4. 平滑移动到 posRelative（替换 targetPos）
//         const float thr = 0.01f;
//         if ((transform.position - posRelative).sqrMagnitude < thr * thr)
//         {
//             transform.position = posRelative;
//             velocity = Vector3.zero;
//         }
//         else
//         {
//             float smoothTime = 30f / positionSmoothSpeed;
//             transform.position = Vector3.SmoothDamp(
//                 transform.position, posRelative,
//                 ref velocity, smoothTime, Mathf.Infinity, Time.deltaTime
//             );
//         }

//         // 5. 平滑旋转
//         Quaternion targetRot = Quaternion.Euler(dc.pitchAngle, dc.rotationAngle, dc.rollAngle);
//         transform.rotation = Quaternion.Lerp(
//             transform.rotation, targetRot, rotationSmoothSpeed * Time.deltaTime
//         );

//         // 6. 记录状态
//         if (!dc.isReplaying)
//             RecordState(totalRuntime, transform.position);

//         // 7. 更新航迹显示
//         if (dc.isShowFullFlightPath)
//             UpdateLineRenderer();
//         else
//             lineRenderer.positionCount = 0;
//     }

//     private void RecordState(float t, Vector3 pos)
//     {
//         flightStates.Add(new FlightState(t, pos, transform.rotation.eulerAngles));
//     }

//     private void UpdateLineRenderer()
//     {
//         int cnt = flightStates.Count;
//         lineRenderer.positionCount = cnt;
//         for (int i = 0; i < cnt; i++)
//             lineRenderer.SetPosition(i, flightStates[i].position);
//     }
// }


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class AirplaneController : MonoBehaviour
// {
//     public Material flightPathMaterial;

//     // —— 平滑参数 —— 
//     public float positionSmoothSpeed = 10f;
//     public float rotationSmoothSpeed = 10f;

//     // —— 地理参数 —— 
//     private const float centerLat = 28.85125f;
//     private const float centerLon = 115.896f;
//     private float metersPerDegreeLat;
//     private float metersPerDegreeLon;

//     // —— 高度参数 —— 
//     private const float initialAltitude = 151.8441f; // 英尺
//     private const float feetToMeters = 0.3048f;

//     // —— 状态变量 —— 
//     private Vector3 velocity = Vector3.zero;

//     // —— 轨迹记录 —— 
//     private class FlightState
//     {
//         public float time;
//         public Vector3 truePosition; // 记录真实世界位置（含总偏移）
//         public Vector3 eulerAngles;
//         public FlightState(float t, Vector3 truePos, Vector3 eul)
//         {
//             time = t;
//             truePosition = truePos;
//             eulerAngles = eul;
//         }
//     }
//     private List<FlightState> flightStates = new List<FlightState>();
//     private LineRenderer lineRenderer;

//     private float totalRuntime = 0f; // 总运行时间

//     // 引用世界管理器（用于获取偏移量）
//     private WorldManager worldManager;

//     void Start()
//     {
//         // 获取世界管理器实例
//         worldManager = WorldManager.Instance;

//         // 计算 WGS84 每度转换系数
//         float latRad = centerLat * Mathf.Deg2Rad;
//         metersPerDegreeLat = 111132.92f - 559.82f * Mathf.Cos(2 * latRad)
//                              + 1.175f * Mathf.Cos(4 * latRad)
//                              - 0.0023f * Mathf.Cos(6 * latRad);
//         metersPerDegreeLon = 111412.84f * Mathf.Cos(latRad)
//                              - 93.5f * Mathf.Cos(3 * latRad)
//                              + 0.118f * Mathf.Cos(5 * latRad);

//         // 初始化航迹渲染器
//         lineRenderer = GetComponent<LineRenderer>();
//         if (lineRenderer == null)
//             lineRenderer = gameObject.AddComponent<LineRenderer>();
//         lineRenderer.positionCount = 0;
//         lineRenderer.widthCurve = AnimationCurve.Constant(0, 1, 0.7f);
//         lineRenderer.material = flightPathMaterial;

//         // 等待数据中心初始化
//         StartCoroutine(WaitForDataCenter());
//     }

//     IEnumerator WaitForDataCenter()
//     {
//         while (DataCenter.Instance == null ||
//                (DataCenter.Instance.latitude == 0 && DataCenter.Instance.longitude == 0))
//         {
//             yield return null;
//         }
//         // 记录启动时刻状态
//         RecordState(totalRuntime);
//         InitializePosition();
//     }

//     void InitializePosition()
//     {
//         var st = DataCenter.Instance;
//         // 计算初始位置（已适配世界偏移）
//         transform.position = CalculateTargetPosition(st.latitude, st.longitude, st.altitude);
//     }

//     void Update()
//     {
//         var dc = DataCenter.Instance;
//         if (dc == null || (dc.latitude == 0 && dc.longitude == 0))
//             return;

//         // 更新总运行时间
//         totalRuntime += Time.deltaTime;
//         dc.totalRuntime = totalRuntime;

//         // —— 回放模式 —— 
//         if (dc.isReplaying && flightStates.Count > 0)
//         {
//             PlaybackFlight(dc);
//             return;
//         }

//         // —— 实时控制 —— 
//         // 1. 计算目标位置（适配世界偏移）
//         Vector3 targetPos = CalculateTargetPosition(dc.latitude, dc.longitude, dc.altitude);

//         // 2. 平滑移动
//         const float stopThreshold = 0.01f;
//         if ((transform.position - targetPos).sqrMagnitude < stopThreshold * stopThreshold)
//         {
//             transform.position = targetPos;
//             velocity = Vector3.zero;
//         }
//         else
//         {
//             float smoothTime = 30f / positionSmoothSpeed;
//             transform.position = Vector3.SmoothDamp(
//                 transform.position, targetPos, ref velocity, smoothTime, Mathf.Infinity, Time.deltaTime
//             );
//         }

//         // 3. 平滑旋转
//         Quaternion targetRot = Quaternion.Euler(dc.pitchAngle, dc.rotationAngle, dc.rollAngle);
//         transform.rotation = Quaternion.Lerp(
//             transform.rotation, targetRot, rotationSmoothSpeed * Time.deltaTime
//         );

//         // 4. 记录状态
//         if (!dc.isReplaying)
//         {
//             RecordState(totalRuntime);
//         }

//         // —— 航迹显示 —— 
//         if (dc.isShowFullFlightPath)
//             UpdateLineRenderer();
//         else
//             lineRenderer.positionCount = 0;
//     }

//     // 核心方法：计算目标位置（适配世界偏移）
//     private Vector3 CalculateTargetPosition(float lat, float lon, float alt)
//     {
//         // 1. 经纬度转绝对坐标（基于中心点）
//         float dx = -(lon - centerLon) * metersPerDegreeLon;
//         float dz = -(lat - centerLat) * metersPerDegreeLat;
//         float dy = (alt - initialAltitude) * feetToMeters;
//         Vector3 absolutePos = new Vector3(dx, dy, dz);

//         // 2. 减去世界总偏移量（关键：适配原点偏移）
//         if (worldManager != null)
//         {
//             absolutePos -= worldManager.GetTotalOffset();
//         }

//         return absolutePos;
//     }

//     // 回放飞行轨迹
//     private void PlaybackFlight(DataCenter dc)
//     {
//         float t = Mathf.Clamp(dc.currentTime, 0f, flightStates[flightStates.Count - 1].time);

//         int idx = flightStates.FindIndex(s => s.time >= t);
//         if (idx <= 0) idx = 1;
//         if (idx >= flightStates.Count) idx = flightStates.Count - 1;

//         var prev = flightStates[idx - 1];
//         var next = flightStates[idx];
//         float lerp = (t - prev.time) / (next.time - prev.time);

//         // 回放位置需转换为当前偏移后的本地位置
//         Vector3 targetPos = Vector3.Lerp(prev.truePosition, next.truePosition, lerp);
//         if (worldManager != null)
//         {
//             targetPos -= worldManager.GetTotalOffset(); // 适配当前偏移
//         }

//         transform.position = targetPos;
//         transform.rotation = Quaternion.Euler(
//             Vector3.Lerp(prev.eulerAngles, next.eulerAngles, lerp)
//         );
//     }

//     // 记录真实世界位置（含总偏移）
//     private void RecordState(float t)
//     {
//         // 计算真实位置（本地位置 + 总偏移）
//         Vector3 truePosition = transform.position;
//         if (worldManager != null)
//         {
//             truePosition += worldManager.GetTotalOffset();
//         }

//         flightStates.Add(new FlightState(
//             t,
//             truePosition, // 保存真实位置，而非本地位置
//             transform.rotation.eulerAngles
//         ));
//     }

//     // 更新航迹显示（转换为当前偏移后的位置）
//     private void UpdateLineRenderer()
//     {
//         int cnt = flightStates.Count;
//         lineRenderer.positionCount = cnt;

//         Vector3 currentOffset = worldManager != null ? worldManager.GetTotalOffset() : Vector3.zero;

//         for (int i = 0; i < cnt; i++)
//         {
//             // 真实位置 - 当前偏移 = 本地显示位置
//             lineRenderer.SetPosition(i, flightStates[i].truePosition - currentOffset);
//         }
//     }
// }



// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class AirplaneController : MonoBehaviour
// {
//     public Material flightPathMaterial;

//     // —— 平滑参数 —— 
//     public float positionSmoothSpeed = 10f;
//     public float rotationSmoothSpeed = 10f;

//     // —— 地理参数 —— 
//     private const float centerLat = 28.85125f;
//     private const float centerLon = 115.896f;
//     private float metersPerDegreeLat;
//     private float metersPerDegreeLon;

//     // —— 高度参数 —— 
//     private const float initialAltitude = 151.8441f; // 英尺
//     private const float feetToMeters = 0.3048f;

//     // —— 状态变量 —— 
//     private Vector3 velocity = Vector3.zero;
//     private Vector3 lastFrameOffset = Vector3.zero; // 记录上一帧的世界偏移量

//     // —— 轨迹记录 —— 
//     private class FlightState
//     {
//         public float time;
//         public Vector3 truePosition; // 记录真实世界位置（含总偏移）
//         public Vector3 eulerAngles;
//         public FlightState(float t, Vector3 truePos, Vector3 eul)
//         {
//             time = t;
//             truePosition = truePos;
//             eulerAngles = eul;
//         }
//     }
//     private List<FlightState> flightStates = new List<FlightState>();
//     private LineRenderer lineRenderer;

//     private float totalRuntime = 0f; // 总运行时间

//     // 引用世界管理器（用于获取偏移量）
//     private WorldManager worldManager;

//     void Start()
//     {
//         // 获取世界管理器实例
//         worldManager = WorldManager.Instance;

//         // 计算 WGS84 每度转换系数
//         float latRad = centerLat * Mathf.Deg2Rad;
//         metersPerDegreeLat = 111132.92f - 559.82f * Mathf.Cos(2 * latRad)
//                              + 1.175f * Mathf.Cos(4 * latRad)
//                              - 0.0023f * Mathf.Cos(6 * latRad);
//         metersPerDegreeLon = 111412.84f * Mathf.Cos(latRad)
//                              - 93.5f * Mathf.Cos(3 * latRad)
//                              + 0.118f * Mathf.Cos(5 * latRad);

//         // 初始化航迹渲染器
//         lineRenderer = GetComponent<LineRenderer>();
//         if (lineRenderer == null)
//             lineRenderer = gameObject.AddComponent<LineRenderer>();
//         lineRenderer.positionCount = 0;
//         lineRenderer.widthCurve = AnimationCurve.Constant(0, 1, 0.7f);
//         lineRenderer.material = flightPathMaterial;

//         // 等待数据中心初始化
//         StartCoroutine(WaitForDataCenter());
//     }

//     IEnumerator WaitForDataCenter()
//     {
//         while (DataCenter.Instance == null ||
//                (DataCenter.Instance.latitude == 0 && DataCenter.Instance.longitude == 0))
//         {
//             yield return null;
//         }
//         // 记录启动时刻状态
//         RecordState(totalRuntime);
//         InitializePosition();
//     }

//     void InitializePosition()
//     {
//         var st = DataCenter.Instance;
//         // 计算初始位置（已适配世界偏移）
//         transform.position = CalculateTargetPosition(st.latitude, st.longitude, st.altitude);
        
//         // 初始化上一帧偏移量
//         lastFrameOffset = worldManager != null ? worldManager.GetTotalOffset() : Vector3.zero;
//     }

//     void Update()
//     {
//         var dc = DataCenter.Instance;
//         if (dc == null || (dc.latitude == 0 && dc.longitude == 0))
//             return;

//         // 更新总运行时间
//         totalRuntime += Time.deltaTime;
//         dc.totalRuntime = totalRuntime;

//         // —— 回放模式 —— 
//         if (dc.isReplaying && flightStates.Count > 0)
//         {
//             PlaybackFlight(dc);
//             return;
//         }

//         // —— 实时控制 —— 
//         // 1. 计算目标位置（适配世界偏移）
//         Vector3 targetPos = CalculateTargetPosition(dc.latitude, dc.longitude, dc.altitude);

//         // 2. 检测世界偏移并立即应用新位置
//         Vector3 currentOffset = worldManager != null ? worldManager.GetTotalOffset() : Vector3.zero;
//         if (currentOffset != lastFrameOffset)
//         {
//             // 世界发生了偏移：立即更新位置，不使用平滑
//             transform.position = targetPos;
//             velocity = Vector3.zero;
//         }
//         else
//         {
//             // 正常状态：平滑移动
//             const float stopThreshold = 0.01f;
//             if ((transform.position - targetPos).sqrMagnitude < stopThreshold * stopThreshold)
//             {
//                 transform.position = targetPos;
//                 velocity = Vector3.zero;
//             }
//             else
//             {
//                 float smoothTime = 30f / positionSmoothSpeed;
//                 transform.position = Vector3.SmoothDamp(
//                     transform.position, targetPos, ref velocity, smoothTime, Mathf.Infinity, Time.deltaTime
//                 );
//             }
//         }

//         // 更新上一帧偏移量
//         lastFrameOffset = currentOffset;

//         // 3. 平滑旋转（旋转可以保持平滑）
//         Quaternion targetRot = Quaternion.Euler(dc.pitchAngle, dc.rotationAngle, dc.rollAngle);
//         transform.rotation = Quaternion.Lerp(
//             transform.rotation, targetRot, rotationSmoothSpeed * Time.deltaTime
//         );

//         // 4. 记录状态
//         if (!dc.isReplaying)
//         {
//             RecordState(totalRuntime);
//         }

//         // —— 航迹显示 —— 
//         if (dc.isShowFullFlightPath)
//             UpdateLineRenderer();
//         else
//             lineRenderer.positionCount = 0;
//     }

//     // 核心方法：计算目标位置（适配世界偏移）
//     private Vector3 CalculateTargetPosition(float lat, float lon, float alt)
//     {
//         // 1. 经纬度转绝对坐标（基于中心点）
//         float dx = -(lon - centerLon) * metersPerDegreeLon;
//         float dz = -(lat - centerLat) * metersPerDegreeLat;
//         float dy = (alt - initialAltitude) * feetToMeters;
//         Vector3 absolutePos = new Vector3(dx, dy, dz);

//         // 2. 减去世界总偏移量（关键：适配原点偏移）
//         if (worldManager != null)
//         {
//             absolutePos -= worldManager.GetTotalOffset();
//         }

//         return absolutePos;
//     }

//     // 回放飞行轨迹
//     private void PlaybackFlight(DataCenter dc)
//     {
//         float t = Mathf.Clamp(dc.currentTime, 0f, flightStates[flightStates.Count - 1].time);

//         int idx = flightStates.FindIndex(s => s.time >= t);
//         if (idx <= 0) idx = 1;
//         if (idx >= flightStates.Count) idx = flightStates.Count - 1;

//         var prev = flightStates[idx - 1];
//         var next = flightStates[idx];
//         float lerp = (t - prev.time) / (next.time - prev.time);

//         // 回放位置需转换为当前偏移后的本地位置
//         Vector3 targetPos = Vector3.Lerp(prev.truePosition, next.truePosition, lerp);
//         if (worldManager != null)
//         {
//             targetPos -= worldManager.GetTotalOffset(); // 适配当前偏移
//         }

//         transform.position = targetPos;
//         transform.rotation = Quaternion.Euler(
//             Vector3.Lerp(prev.eulerAngles, next.eulerAngles, lerp)
//         );
//     }

//     // 记录真实世界位置（含总偏移）
//     private void RecordState(float t)
//     {
//         // 计算真实位置（本地位置 + 总偏移）
//         Vector3 truePosition = transform.position;
//         if (worldManager != null)
//         {
//             truePosition += worldManager.GetTotalOffset();
//         }

//         flightStates.Add(new FlightState(
//             t,
//             truePosition, // 保存真实位置，而非本地位置
//             transform.rotation.eulerAngles
//         ));
//     }

//     // 更新航迹显示（转换为当前偏移后的位置）
//     private void UpdateLineRenderer()
//     {
//         int cnt = flightStates.Count;
//         lineRenderer.positionCount = cnt;

//         Vector3 currentOffset = worldManager != null ? worldManager.GetTotalOffset() : Vector3.zero;

//         for (int i = 0; i < cnt; i++)
//         {
//             // 真实位置 - 当前偏移 = 本地显示位置
//             lineRenderer.SetPosition(i, flightStates[i].truePosition - currentOffset);
//         }
//     }
// }



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneController : MonoBehaviour
{
    public Material flightPathMaterial;

    // —— 平滑参数 —— 
    public float positionSmoothSpeed = 10f;
    public float rotationSmoothSpeed = 10f;

    // —— 地理参数 —— 
    private const float centerLat = 28.85125f;
    private const float centerLon = 115.896f;
    private float metersPerDegreeLat;
    private float metersPerDegreeLon;

    // —— 高度参数 —— 
    private const float initialAltitude = 151.8441f; // 英尺
    private const float feetToMeters = 0.3048f;

    // —— 状态变量 —— 
    private Vector3 velocity = Vector3.zero;
    private Vector3 lastFrameOffset = Vector3.zero;

    // —— 轨迹记录 —— 
    private class FlightState
    {
        public float time;
        public Vector3 trueWorldPosition; // 存储真实世界坐标（不受偏移影响）
        public Vector3 eulerAngles;
        public FlightState(float t, Vector3 truePos, Vector3 eul)
        {
            time = t;
            trueWorldPosition = truePos;
            eulerAngles = eul;
        }
    }
    private List<FlightState> flightStates = new List<FlightState>();
    private LineRenderer lineRenderer;

    private float totalRuntime = 0f;
    private bool isReplaying = false; // 新增：标记当前是否在回放

    // 引用世界管理器
    private WorldManager worldManager;

    void Start()
    {
        worldManager = WorldManager.Instance;

        // 计算经纬度转换系数
        float latRad = centerLat * Mathf.Deg2Rad;
        metersPerDegreeLat = 111132.92f - 559.82f * Mathf.Cos(2 * latRad)
                             + 1.175f * Mathf.Cos(4 * latRad)
                             - 0.0023f * Mathf.Cos(6 * latRad);
        metersPerDegreeLon = 111412.84f * Mathf.Cos(latRad)
                             - 93.5f * Mathf.Cos(3 * latRad)
                             + 0.118f * Mathf.Cos(5 * latRad);

        // 初始化航迹渲染器
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.widthCurve = AnimationCurve.Constant(0, 1, 0.7f);
        lineRenderer.material = flightPathMaterial;

        StartCoroutine(WaitForDataCenter());
    }

    IEnumerator WaitForDataCenter()
    {
        while (DataCenter.Instance == null ||
               (DataCenter.Instance.latitude == 0 && DataCenter.Instance.longitude == 0))
        {
            yield return null;
        }
        RecordState(totalRuntime);
        InitializePosition();
        lastFrameOffset = worldManager != null ? worldManager.GetTotalOffset() : Vector3.zero;
    }

    void InitializePosition()
    {
        var st = DataCenter.Instance;
        transform.position = CalculateTargetPosition(st.latitude, st.longitude, st.altitude);
    }

    void Update()
    {
        var dc = DataCenter.Instance;
        if (dc == null || (dc.latitude == 0 && dc.longitude == 0))
            return;

        totalRuntime += Time.deltaTime;
        dc.totalRuntime = totalRuntime;

        // —— 检测回放状态变化（关键修改1）—— 
        if (dc.isReplaying != isReplaying)
        {
            isReplaying = dc.isReplaying;
            if (isReplaying)
            {
                // 开始回放时，记录当前位置作为回放起点
                RecordState(totalRuntime);
            }
        }

        // —— 回放模式 —— 
        if (isReplaying && flightStates.Count > 0)
        {
            PlaybackFlight(dc);
            return; // 关键修改2：回放时直接返回，不执行后续记录逻辑
        }

        // —— 实时控制 —— 
        Vector3 targetPos = CalculateTargetPosition(dc.latitude, dc.longitude, dc.altitude);
        Vector3 currentOffset = worldManager != null ? worldManager.GetTotalOffset() : Vector3.zero;

        if (currentOffset != lastFrameOffset)
        {
            transform.position = targetPos;
            velocity = Vector3.zero;
        }
        else
        {
            const float stopThreshold = 0.01f;
            if ((transform.position - targetPos).sqrMagnitude < stopThreshold * stopThreshold)
            {
                transform.position = targetPos;
                velocity = Vector3.zero;
            }
            else
            {
                float smoothTime = 30f / positionSmoothSpeed;
                transform.position = Vector3.SmoothDamp(
                    transform.position, targetPos, ref velocity, smoothTime, Mathf.Infinity, Time.deltaTime
                );
            }
        }

        lastFrameOffset = currentOffset;

        // 平滑旋转
        Quaternion targetRot = Quaternion.Euler(dc.pitchAngle, dc.rotationAngle, dc.rollAngle);
        transform.rotation = Quaternion.Lerp(
            transform.rotation, targetRot, rotationSmoothSpeed * Time.deltaTime
        );

        // 记录状态（仅在非回放模式下记录）
        if (!isReplaying)
        {
            RecordState(totalRuntime);
        }

        // —— 航迹显示 —— 
        if (dc.isShowFullFlightPath)
            UpdateLineRenderer();
        else
            lineRenderer.positionCount = 0;
    }

    private Vector3 CalculateTargetPosition(float lat, float lon, float alt)
    {
        // 经纬度转换逻辑
        float dx = -(lon - centerLon) * metersPerDegreeLon;
        float dz = -(lat - centerLat) * metersPerDegreeLat;
        float dy = (alt - initialAltitude) * feetToMeters;
        Vector3 absolutePos = new Vector3(dx, dy, dz);

        if (worldManager != null)
        {
            absolutePos -= worldManager.GetTotalOffset();
        }

        return absolutePos;
    }

    // 回放飞行轨迹
    private void PlaybackFlight(DataCenter dc)
    {
        float t = Mathf.Clamp(dc.currentTime, 0f, flightStates[flightStates.Count - 1].time);

        int idx = flightStates.FindIndex(s => s.time >= t);
        if (idx <= 0) idx = 1;
        if (idx >= flightStates.Count) idx = flightStates.Count - 1;

        var prev = flightStates[idx - 1];
        var next = flightStates[idx];
        float lerp = (t - prev.time) / (next.time - prev.time);

        // 回放时使用真实坐标转换为当前本地坐标
        Vector3 targetTruePos = Vector3.Lerp(prev.trueWorldPosition, next.trueWorldPosition, lerp);
        Vector3 currentOffset = worldManager != null ? worldManager.GetTotalOffset() : Vector3.zero;
        transform.position = targetTruePos - currentOffset;

        transform.rotation = Quaternion.Euler(
            Vector3.Lerp(prev.eulerAngles, next.eulerAngles, lerp)
        );
    }

    // 记录真实世界位置
    private void RecordState(float t)
    {
        // 计算真实坐标 = 本地位置 + 总偏移量
        Vector3 trueWorldPosition = transform.position;
        if (worldManager != null)
        {
            trueWorldPosition += worldManager.GetTotalOffset();
        }

        flightStates.Add(new FlightState(
            t,
            trueWorldPosition,
            transform.rotation.eulerAngles
        ));
    }

    // 更新航迹显示
    private void UpdateLineRenderer()
    {
        int cnt = flightStates.Count;
        lineRenderer.positionCount = cnt;

        Vector3 currentOffset = worldManager != null ? worldManager.GetTotalOffset() : Vector3.zero;

        for (int i = 0; i < cnt; i++)
        {
            // 真实坐标 - 当前偏移 = 正确的本地显示坐标
            lineRenderer.SetPosition(i, flightStates[i].trueWorldPosition - currentOffset);
        }
    }
}