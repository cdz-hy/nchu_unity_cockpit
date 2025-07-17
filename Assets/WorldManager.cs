// using UnityEngine;
// using System.Collections.Generic;

// public class WorldManager : MonoBehaviour
// {
//     public GameObject player; // 玩家或参照物（如相机）
//     public float shiftThreshold = 1000f; // 重置阈值（建议500-2000单位）
//     private Vector3 totalOffset;

//     void LateUpdate()
//     {
//         if (player.transform.position.magnitude > shiftThreshold)
//         {
//             ShiftWorldOrigin();
//         }
//     }

//     private void ShiftWorldOrigin()
//     {
//         Vector3 offset = player.transform.position; // 获取当前偏移量
//         totalOffset += offset; // 累计总偏移

//         // 移动场景中所有根物体（包括地形、动态物体等）
//         foreach (Transform obj in GetAllRootObjects())
//         {
//             obj.position -= offset;
//         }

//         // 特殊处理：移动粒子系统（防止拖尾残留）
//         foreach (var ps in FindObjectsOfType<ParticleSystem>())
//         {
//             ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
//             ps.transform.position -= offset;
//             ps.Play();
//         }

//         Debug.Log($"World shifted. Total offset: {totalOffset}");
//     }

//     private List<Transform> GetAllRootObjects()
//     {
//         List<Transform> roots = new List<Transform>();
//         foreach (Transform obj in transform) // 将所有需移动对象设为子物体
//         {
//             roots.Add(obj);
//         }
//         return roots;
//     }

//     // 获取真实世界位置（用于物理等计算）
//     public Vector3 GetTruePosition(Vector3 localPosition)
//     {
//         return localPosition + totalOffset;
//     }
// }


// using UnityEngine;
// using System.Collections.Generic;

// public class WorldManager : MonoBehaviour
// {
//     public GameObject player; // 玩家或参照物（如相机）
//     public float shiftThreshold = 1000f; // 重置阈值（建议500-2000单位）
//     private Vector3 totalOffset;
    
//     // 单例模式，便于其他脚本访问
//     public static WorldManager Instance { get; private set; }

//     void Awake()
//     {
//         if (Instance != null && Instance != this)
//         {
//             Destroy(this);
//         }
//         else
//         {
//             Instance = this;
//         }
//     }

//     void LateUpdate()
//     {
//         if (player.transform.position.magnitude > shiftThreshold)
//         {
//             ShiftWorldOrigin();
//         }
//     }

//     private void ShiftWorldOrigin()
//     {
//         Vector3 offset = player.transform.position; // 获取当前偏移量
//         totalOffset += offset; // 累计总偏移

//         // 移动场景中所有根物体（地形、机场等，但不包括飞机！）
//         foreach (Transform obj in GetAllRootObjects())
//         {
//             obj.position -= offset;
//         }

//         // 特殊处理：移动粒子系统（防止拖尾残留）
//         foreach (var ps in FindObjectsOfType<ParticleSystem>())
//         {
//             ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
//             ps.transform.position -= offset;
//             ps.Play();
//         }

//         Debug.Log($"World shifted. Total offset: {totalOffset}");
        
//         // 通知飞机控制器世界已偏移
//         if (AirplaneController.Instance != null)
//         {
//             AirplaneController.Instance.OnWorldShifted(offset);
//         }
//     }

//     private List<Transform> GetAllRootObjects()
//     {
//         List<Transform> roots = new List<Transform>();
//         foreach (Transform obj in transform) // 将所有需移动对象设为子物体
//         {
//             // 排除飞机对象，避免与AirplaneController冲突
//             if (obj.GetComponent<AirplaneController>() == null)
//             {
//                 roots.Add(obj);
//             }
//         }
//         return roots;
//     }

//     // 获取真实世界位置（用于物理等计算）
//     public Vector3 GetTruePosition(Vector3 localPosition)
//     {
//         return localPosition + totalOffset;
//     }
    
//     // 获取当前世界偏移量
//     public Vector3 GetWorldOffset()
//     {
//         return totalOffset;
//     }
// }


// using UnityEngine;
// using System.Collections.Generic;

// public class WorldManager : MonoBehaviour
// {
//     public GameObject player; // 玩家或参照物（如相机）
//     public float shiftThreshold = 1000f; // 重置阈值（建议500-2000单位）
//     private Vector3 totalOffset;
//     private bool isShifting = false; // 标记是否正在进行坐标切换
    
//     // 单例模式
//     public static WorldManager Instance { get; private set; }

//     // 新增：需要移动的额外对象列表（用于手动添加机场等特殊对象）
//     public List<GameObject> additionalObjectsToShift = new List<GameObject>();

//     void Awake()
//     {
//         if (Instance != null && Instance != this)
//         {
//             Destroy(this);
//         }
//         else
//         {
//             Instance = this;
//         }
//     }

//     void LateUpdate()
//     {
//         if (!isShifting && player.transform.position.magnitude > shiftThreshold)
//         {
//             ShiftWorldOrigin();
//         }
//     }

//     private void ShiftWorldOrigin()
//     {
//         isShifting = true;
        
//         Vector3 offset = player.transform.position; // 获取当前偏移量
//         totalOffset += offset; // 累计总偏移

//         // 暂停物理模拟，防止抖动
//         Physics.autoSimulation = false;

//         // 移动所有子对象（地形等）
//         foreach (Transform child in transform)
//         {
//             child.position -= offset;
//         }
        
//         // 移动额外指定的对象（如机场）
//         foreach (GameObject obj in additionalObjectsToShift)
//         {
//             if (obj != null)
//                 obj.transform.position -= offset;
//         }

//         // 特殊处理：移动粒子系统
//         foreach (var ps in FindObjectsOfType<ParticleSystem>())
//         {
//             ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
//             ps.transform.position -= offset;
//             ps.Play();
//         }

//         Debug.Log($"World shifted. Total offset: {totalOffset}");
        
//         // 通知飞机控制器世界已偏移
//         if (AirplaneController.Instance != null)
//         {
//             AirplaneController.Instance.OnWorldShifted(offset);
//         }

//         // 恢复物理模拟
//         Physics.autoSimulation = true;
        
//         isShifting = false;
//     }

//     // 获取真实世界位置（基于初始原点）
//     public Vector3 GetTruePosition(Vector3 localPosition)
//     {
//         return localPosition + totalOffset;
//     }
    
//     // 获取当前世界偏移量
//     public Vector3 GetWorldOffset()
//     {
//         return totalOffset;
//     }
    
//     // 判断是否正在进行坐标切换
//     public bool IsShifting()
//     {
//         return isShifting;
//     }
// }


// using UnityEngine;
// using System.Collections.Generic;
// using System.Collections;

// public class WorldManager : MonoBehaviour
// {
//     public GameObject player; // 飞机对象（ALL）
//     public float shiftThreshold = 1000f; // 触发偏移的阈值
//     public List<GameObject> additionalObjectsToShift = new List<GameObject>(); // 机场等需要移动的对象

//     private Vector3 totalOffset;
//     private bool isShifting = false; // 标记是否正在偏移
//     public static WorldManager Instance { get; private set; }

//     void Awake()
//     {
//         Instance = this;
//         // 初始化时验证机场对象是否有效
//         ValidateScenarioObjects();
//     }

//     // 验证机场对象是否存在/是否有子对象
//     private void ValidateScenarioObjects()
//     {
//         foreach (var obj in additionalObjectsToShift)
//         {
//             if (obj == null)
//             {
//                 Debug.LogError("【初始化错误】additionalObjectsToShift中包含空对象，请检查引用！");
//                 continue;
//             }
//             if (obj.transform.childCount == 0)
//             {
//                 Debug.LogWarning($"【提示】机场对象{obj.name}没有子对象，可能不是复合模型根节点！");
//             }
//             else
//             {
//                 Debug.Log($"【初始化成功】机场对象{obj.name}包含{obj.transform.childCount}个子对象，准备处理复合模型移动");
//             }
//         }
//     }

//     void LateUpdate()
//     {
//         if (player == null || isShifting) return;

//         // 只在XZ平面计算距离（忽略高度，避免飞行高度影响偏移触发）
//         float distanceFromOrigin = new Vector2(player.transform.position.x, player.transform.position.z).magnitude;
//         if (distanceFromOrigin > shiftThreshold)
//         {
//             StartCoroutine(ShiftWorldOriginCoroutine()); // 用协程分帧处理大型模型
//         }
//     }

//     // 协程：分帧处理世界偏移，避免大型模型移动卡顿
//     private IEnumerator ShiftWorldOriginCoroutine()
//     {
//         isShifting = true;
//         Vector3 offset = player.transform.position; // 当前偏移量（飞机到原点的距离）
//         totalOffset += offset;
//         Debug.Log($"【偏移开始】总偏移量={totalOffset}，本次偏移={offset}");

//         // 1. 暂停物理模拟，防止移动时碰撞异常
//         Physics.autoSimulation = false;

//         // 2. 移动地形（WorldManager的子对象）
//         foreach (Transform terrainChild in transform)
//         {
//             MoveAllChildrenRecursive(terrainChild, -offset); // 递归移动地形所有子对象
//             yield return null; // 每移动一个地形子对象，暂停一帧
//         }

//         // 3. 移动机场（大型复合模型）
//         foreach (var scenario in additionalObjectsToShift)
//         {
//             if (scenario == null)
//             {
//                 Debug.LogError("【偏移失败】机场对象为空，跳过移动！");
//                 continue;
//             }

//             // 临时禁用可能干扰移动的组件（动画、物理等）
//             var disabledComponents = DisableInterferingComponents(scenario);

//             // 递归移动机场所有子对象（包括嵌套的模型、碰撞体等）
//             Debug.Log($"【开始移动机场】{scenario.name}，包含{scenario.transform.childCount}个子对象");
//             MoveAllChildrenRecursive(scenario.transform, -offset);

//             // 恢复禁用的组件
//             RestoreDisabledComponents(disabledComponents);

//             Debug.Log($"【机场移动完成】{scenario.name}，新位置={scenario.transform.position}");
//             yield return null; // 分帧处理，避免卡顿
//         }

//         // 4. 处理粒子系统（避免偏移后粒子残留）
//         foreach (var ps in FindObjectsOfType<ParticleSystem>())
//         {
//             ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); // 清空现有粒子
//             ps.transform.position -= offset;
//             ps.Play();
//             yield return null;
//         }

//         // 5. 恢复物理模拟
//         Physics.autoSimulation = true;

//         // 通知飞机控制器偏移完成
//         if (AirplaneController.Instance != null)
//         {
//             AirplaneController.Instance.OnWorldShifted(offset);
//         }

//         Debug.Log($"【偏移完成】所有对象移动完毕，总偏移量={totalOffset}");
//         isShifting = false;
//     }

//     // 递归移动所有子对象（包括嵌套层级）
//     private void MoveAllChildrenRecursive(Transform parent, Vector3 offset)
//     {
//         // 移动当前对象
//         parent.position += offset;

//         // 递归移动所有子对象（解决复合模型深层子节点不移动的问题）
//         foreach (Transform child in parent)
//         {
//             MoveAllChildrenRecursive(child, offset);
//         }
//     }

//     // 禁用可能干扰移动的组件（返回被禁用的组件列表，用于后续恢复）
//     private List<Component> DisableInterferingComponents(GameObject obj)
//     {
//         List<Component> disabledComponents = new List<Component>();

//         // 禁用动画组件（防止动画覆盖位置）
//         var animator = obj.GetComponent<Animator>();
//         if (animator != null && animator.enabled)
//         {
//             animator.enabled = false;
//             disabledComponents.Add(animator);
//         }

//         // 禁用刚体组件（防止物理引擎重置位置）
//         var rigidbody = obj.GetComponent<Rigidbody>();
//         if (rigidbody != null && !rigidbody.isKinematic)
//         {
//             rigidbody.isKinematic = true;
//             disabledComponents.Add(rigidbody);
//         }

//         // 禁用导航组件（防止导航更新位置）
//         var navAgent = obj.GetComponent<UnityEngine.AI.NavMeshAgent>();
//         if (navAgent != null && navAgent.enabled)
//         {
//             navAgent.enabled = false;
//             disabledComponents.Add(navAgent);
//         }

//         // 递归处理子对象的干扰组件
//         foreach (Transform child in obj.transform)
//         {
//             disabledComponents.AddRange(DisableInterferingComponents(child.gameObject));
//         }

//         return disabledComponents;
//     }

//     // 恢复之前禁用的组件
//     private void RestoreDisabledComponents(List<Component> components)
//     {
//         foreach (var comp in components)
//         {
//             if (comp is Animator anim)
//             {
//                 anim.enabled = true;
//             }
//             else if (comp is Rigidbody rb)
//             {
//                 rb.isKinematic = false;
//             }
//             else if (comp is UnityEngine.AI.NavMeshAgent nav)
//             {
//                 nav.enabled = true;
//             }
//         }
//     }

//     // 外部获取总偏移量的接口
//     public Vector3 GetWorldOffset() => totalOffset;
//     public bool IsShifting() => isShifting;
// }


// using UnityEngine;
// using System.Collections.Generic;
// using System.Collections;

// public class WorldManager : MonoBehaviour
// {
//     public GameObject player;
//     public float shiftThreshold = 1000f;
//     public List<GameObject> additionalObjectsToShift = new List<GameObject>();

//     private Vector3 totalOffset;
//     private bool isShifting = false;
//     public static WorldManager Instance { get; private set; }

//     void Awake()
//     {
//         Instance = this;
//         ValidateScenarioObjects();
//     }

//     void ValidateScenarioObjects()
//     {
//         foreach (var obj in additionalObjectsToShift)
//         {
//             if (obj == null)
//             {
//                 Debug.LogError("【错误】additionalObjectsToShift包含空引用！");
//                 continue;
//             }
            
//             // 检查是否有父对象（可能导致本地坐标问题）
//             if (obj.transform.parent != null)
//             {
//                 Debug.LogWarning($"【警告】机场对象{obj.name}有父对象({obj.transform.parent.name})，建议设为顶层对象！");
//             }
            
//             // 计算子对象数量（评估模型复杂度）
//             int childCount = GetTotalChildCount(obj.transform);
//             Debug.Log($"【场景初始化】机场对象{obj.name}包含{childCount}个子对象（层级深度：{GetMaxDepth(obj.transform)}）");
//         }
//     }

//     // 递归计算总子对象数量
//     private int GetTotalChildCount(Transform parent, int currentDepth = 0)
//     {
//         int count = parent.childCount;
//         foreach (Transform child in parent)
//         {
//             count += GetTotalChildCount(child, currentDepth + 1);
//         }
//         return count;
//     }

//     // 获取最大层级深度
//     private int GetMaxDepth(Transform parent, int currentDepth = 0)
//     {
//         int maxDepth = currentDepth;
//         foreach (Transform child in parent)
//         {
//             int childDepth = GetMaxDepth(child, currentDepth + 1);
//             if (childDepth > maxDepth) maxDepth = childDepth;
//         }
//         return maxDepth;
//     }

//     void LateUpdate()
//     {
//         if (player == null || isShifting) return;
        
//         // 只计算XZ平面距离（忽略高度）
//         float distance = new Vector2(player.transform.position.x, player.transform.position.z).magnitude;
//         if (distance > shiftThreshold)
//         {
//             StartCoroutine(ShiftWorldOriginCoroutine());
//         }
//     }

//     private IEnumerator ShiftWorldOriginCoroutine()
//     {
//         isShifting = true;
//         Vector3 offset = player.transform.position;
//         totalOffset += offset;
//         Debug.Log($"【世界偏移】触发偏移，偏移量={offset}，总偏移量={totalOffset}");

//         // 1. 暂停物理模拟
//         Physics.autoSimulation = false;

//         // 2. 移动地形（WorldManager的子对象）
//         foreach (Transform terrainChild in transform)
//         {
//             MoveTransformUsingWorldPosition(terrainChild, -offset);
//             yield return null;
//         }

//         // 3. 移动机场（关键：使用世界坐标移动）
//         foreach (GameObject scenario in additionalObjectsToShift)
//         {
//             if (scenario == null) continue;
            
//             // 记录移动前的世界位置（用于验证）
//             Vector3 worldPosBefore = scenario.transform.position;
            
//             // 临时禁用干扰组件
//             var disabledComponents = DisableInterferingComponents(scenario);
            
//             // 使用世界坐标移动（递归处理所有子对象）
//             Debug.Log($"【机场移动】开始处理{scenario.name}，总子对象:{GetTotalChildCount(scenario.transform)}");
//             MoveTransformUsingWorldPosition(scenario.transform, -offset);
            
//             // 恢复组件
//             RestoreDisabledComponents(disabledComponents);
            
//             // 验证移动后的世界位置
//             Vector3 worldPosAfter = scenario.transform.position;
//             Debug.Log($"【机场移动】{scenario.name} 世界坐标: {worldPosBefore} → {worldPosAfter}（预期变化:{-offset}）");
            
//             yield return null;
//         }

//         // 4. 处理粒子系统
//         foreach (var ps in FindObjectsOfType<ParticleSystem>())
//         {
//             ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
//             ps.transform.position -= offset;
//             ps.Play();
//             yield return null;
//         }

//         // 5. 恢复物理模拟
//         Physics.autoSimulation = true;

//         // 通知飞机控制器
//         if (AirplaneController.Instance != null)
//         {
//             AirplaneController.Instance.OnWorldShifted(offset);
//         }

//         isShifting = false;
//     }

//     // 关键方法：使用世界坐标移动Transform及其所有子对象
//     private void MoveTransformUsingWorldPosition(Transform parent, Vector3 worldOffset)
//     {
//         // 保存当前旋转（避免旋转影响位移方向）
//         Quaternion originalRotation = parent.rotation;
//         parent.rotation = Quaternion.identity;

//         // 计算新的世界坐标（关键：使用世界坐标计算）
//         Vector3 newWorldPosition = parent.position + worldOffset;
        
//         // 设置新的世界坐标（注意：不是localPosition）
//         parent.position = newWorldPosition;

//         // 恢复旋转
//         parent.rotation = originalRotation;

//         // 递归处理所有子对象
//         foreach (Transform child in parent)
//         {
//             MoveTransformUsingWorldPosition(child, worldOffset);
//         }
//     }

//     // 禁用可能干扰移动的组件
//     private List<Component> DisableInterferingComponents(GameObject obj)
//     {
//         List<Component> disabled = new List<Component>();
        
//         // 禁用动画
//         var animator = obj.GetComponent<Animator>();
//         if (animator != null && animator.enabled)
//         {
//             animator.enabled = false;
//             disabled.Add(animator);
//         }
        
//         // 禁用刚体物理
//         var rigidbody = obj.GetComponent<Rigidbody>();
//         if (rigidbody != null && !rigidbody.isKinematic)
//         {
//             rigidbody.isKinematic = true;
//             disabled.Add(rigidbody);
//         }
        
//         // 禁用导航代理
//         var navAgent = obj.GetComponent<UnityEngine.AI.NavMeshAgent>();
//         if (navAgent != null && navAgent.enabled)
//         {
//             navAgent.enabled = false;
//             disabled.Add(navAgent);
//         }
        
//         // 递归处理子对象
//         foreach (Transform child in obj.transform)
//         {
//             disabled.AddRange(DisableInterferingComponents(child.gameObject));
//         }
        
//         return disabled;
//     }

//     // 恢复禁用的组件
//     private void RestoreDisabledComponents(List<Component> components)
//     {
//         foreach (var comp in components)
//         {
//             if (comp is Animator anim) anim.enabled = true;
//             else if (comp is Rigidbody rb) rb.isKinematic = false;
//             else if (comp is UnityEngine.AI.NavMeshAgent nav) nav.enabled = true;
//         }
//     }

//     public Vector3 GetWorldOffset() => totalOffset;
//     public bool IsShifting() => isShifting;
// }


// using UnityEngine;
// using System.Collections.Generic;
// using System.Collections;

// public class WorldManager : MonoBehaviour
// {
//     public GameObject player;
//     public float shiftThreshold = 1000f;
//     public List<GameObject> additionalObjectsToShift = new List<GameObject>();
    
//     // 双精度总偏移量，避免浮点数精度问题
//     private Vector3 totalOffset;
//     private Vector3Double totalOffsetDouble;
    
//     private bool isShifting = false;
//     public static WorldManager Instance { get; private set; }
    
//     // 用于存储组件状态的字典
//     private Dictionary<GameObject, List<ComponentState>> componentStates = new Dictionary<GameObject, List<ComponentState>>();
    
//     // 组件状态结构
//     private struct ComponentState
//     {
//         public Component component;
//         public bool wasEnabled;
//         public bool wasKinematic; // 专门用于Rigidbody的isKinematic属性
        
//         public ComponentState(Component comp, bool enabled, bool kinematic = false)
//         {
//             component = comp;
//             wasEnabled = enabled;
//             wasKinematic = kinematic;
//         }
//     }

//     void Awake()
//     {
//         Instance = this;
//         ValidateScenarioObjects();
//     }

//     void ValidateScenarioObjects()
//     {
//         foreach (var obj in additionalObjectsToShift)
//         {
//             if (obj == null)
//             {
//                 Debug.LogError("【错误】additionalObjectsToShift包含空引用！");
//                 continue;
//             }
            
//             if (obj.transform.parent != null)
//             {
//                 Debug.LogWarning($"【警告】机场对象{obj.name}有父对象({obj.transform.parent.name})，建议设为顶层对象！");
//             }
            
//             int childCount = GetTotalChildCount(obj.transform);
//             int depth = GetMaxDepth(obj.transform);
//             Debug.Log($"【场景初始化】机场对象{obj.name}包含{childCount}个子对象（层级深度：{depth}）");
//         }
//     }

//     private int GetTotalChildCount(Transform parent)
//     {
//         int count = parent.childCount;
//         foreach (Transform child in parent)
//             count += GetTotalChildCount(child);
//         return count;
//     }

//     private int GetMaxDepth(Transform parent, int currentDepth = 0)
//     {
//         int maxDepth = currentDepth;
//         foreach (Transform child in parent)
//         {
//             int childDepth = GetMaxDepth(child, currentDepth + 1);
//             if (childDepth > maxDepth) maxDepth = childDepth;
//         }
//         return maxDepth;
//     }

//     void LateUpdate()
//     {
//         if (player == null || isShifting) return;
        
//         float distance = new Vector2(player.transform.position.x, player.transform.position.z).magnitude;
//         if (distance > shiftThreshold)
//         {
//             StartCoroutine(ShiftWorldOriginCoroutine());
//         }
//     }

//     private IEnumerator ShiftWorldOriginCoroutine()
//     {
//         isShifting = true;
//         Vector3 offset = player.transform.position;
        
//         // 更新双精度总偏移量
//         totalOffsetDouble += new Vector3Double(offset);
//         totalOffset = (Vector3)totalOffsetDouble;
        
//         Debug.Log($"【世界偏移】触发偏移，偏移量={offset}，总偏移量={totalOffset}");

//         // 暂停物理模拟
//         Physics.autoSimulation = false;

//         // 使用对象池避免GC
//         List<Transform> allTransforms = ListPool<Transform>.Get();
        
//         // 移动地形
//         CollectAllChildren(transform, allTransforms);
//         ApplyOffsetToTransforms(allTransforms, -offset);
//         ListPool<Transform>.Release(allTransforms);
//         yield return null;

//         // 移动机场等额外对象
//         foreach (GameObject scenario in additionalObjectsToShift)
//         {
//             if (scenario == null) continue;
            
//             Vector3 worldPosBefore = scenario.transform.position;
            
//             // 保存并禁用干扰组件
//             SaveAndDisableComponents(scenario);
            
//             // 收集所有子对象
//             allTransforms.Clear();
//             CollectAllChildren(scenario.transform, allTransforms);
            
//             // 应用偏移
//             ApplyOffsetToTransforms(allTransforms, -offset);
            
//             // 恢复组件状态
//             RestoreComponents(scenario);
            
//             Vector3 worldPosAfter = scenario.transform.position;
//             Debug.Log($"【机场移动】{scenario.name} 世界坐标: {worldPosBefore} → {worldPosAfter}（预期变化:{-offset}）");
            
//             ListPool<Transform>.Release(allTransforms);
//             yield return null;
//         }

//         // 处理粒子系统
//         foreach (var ps in FindObjectsOfType<ParticleSystem>())
//         {
//             ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
//             ps.transform.position -= offset;
//             ps.Play();
//         }

//         // 恢复物理模拟
//         Physics.autoSimulation = true;

//         // 通知飞机控制器
//         if (AirplaneController.Instance != null)
//         {
//             AirplaneController.Instance.OnWorldShifted(offset);
//         }

//         isShifting = false;
//     }

//     private void CollectAllChildren(Transform parent, List<Transform> result)
//     {
//         result.Add(parent);
//         foreach (Transform child in parent)
//             CollectAllChildren(child, result);
//     }

//     private void ApplyOffsetToTransforms(List<Transform> transforms, Vector3 offset)
//     {
//         foreach (Transform t in transforms)
//         {
//             // 保存当前旋转
//             Quaternion originalRotation = t.rotation;
//             t.rotation = Quaternion.identity;
            
//             // 直接设置世界坐标
//             t.position += offset;
            
//             // 恢复旋转
//             t.rotation = originalRotation;
//         }
//     }

//     private void SaveAndDisableComponents(GameObject obj)
//     {
//         var states = new List<ComponentState>();
        
//         // 保存并禁用Animator
//         var animator = obj.GetComponent<Animator>();
//         if (animator != null)
//         {
//             states.Add(new ComponentState(animator, animator.enabled));
//             animator.enabled = false;
//         }
        
//         // 保存并设置Rigidbody为运动学
//         var rigidbody = obj.GetComponent<Rigidbody>();
//         if (rigidbody != null)
//         {
//             states.Add(new ComponentState(rigidbody, rigidbody.isKinematic, rigidbody.isKinematic));
//             rigidbody.isKinematic = true;
//         }
        
//         // 保存并禁用NavMeshAgent
//         var navAgent = obj.GetComponent<UnityEngine.AI.NavMeshAgent>();
//         if (navAgent != null)
//         {
//             states.Add(new ComponentState(navAgent, navAgent.enabled));
//             navAgent.enabled = false;
//         }
        
//         // 递归处理子对象
//         foreach (Transform child in obj.transform)
//         {
//             SaveAndDisableComponents(child.gameObject);
//         }
        
//         componentStates[obj] = states;
//     }

//     private void RestoreComponents(GameObject obj)
//     {
//         if (componentStates.TryGetValue(obj, out var states))
//         {
//             foreach (var state in states)
//             {
//                 // 使用as操作符进行安全转换
//                 if (state.component is Animator anim)
//                     anim.enabled = state.wasEnabled;
//                 else if (state.component is Rigidbody rb)
//                     rb.isKinematic = state.wasKinematic;
//                 else if (state.component is UnityEngine.AI.NavMeshAgent nav)
//                     nav.enabled = state.wasEnabled;
//             }
            
//             componentStates.Remove(obj);
//         }
        
//         // 递归处理子对象
//         foreach (Transform child in obj.transform)
//         {
//             RestoreComponents(child.gameObject);
//         }
//     }

//     public Vector3 GetWorldOffset() => totalOffset;
//     public bool IsShifting() => isShifting;
// }

// // 双精度Vector3实现（用于高精度偏移计算）
// public struct Vector3Double
// {
//     public double x, y, z;
    
//     public Vector3Double(double x, double y, double z)
//     {
//         this.x = x;
//         this.y = y;
//         this.z = z;
//     }
    
//     public Vector3Double(Vector3 v)
//     {
//         x = v.x;
//         y = v.y;
//         z = v.z;
//     }
    
//     public static Vector3Double operator +(Vector3Double a, Vector3Double b)
//     {
//         return new Vector3Double(a.x + b.x, a.y + b.y, a.z + b.z);
//     }
    
//     public static implicit operator Vector3(Vector3Double v)
//     {
//         return new Vector3((float)v.x, (float)v.y, (float)v.z);
//     }
// }

// // 简单的对象池实现，减少GC
// public static class ListPool<T>
// {
//     private static readonly Stack<List<T>> pool = new Stack<List<T>>();
    
//     public static List<T> Get()
//     {
//         return pool.Count > 0 ? pool.Pop() : new List<T>();
//     }
    
//     public static void Release(List<T> list)
//     {
//         list.Clear();
//         pool.Push(list);
//     }
// }



// using UnityEngine;
// using System.Collections.Generic;

// public class WorldManager : MonoBehaviour
// {
//     public GameObject player; // 飞机对象（挂载AirplaneController的物体）
//     public float shiftThreshold = 1000f; // 触发偏移的阈值
//     public List<GameObject> worldObjects = new List<GameObject>(); // 需移动的场景物体（地形、机场等）

//     private Vector3 totalOffset; // 累计总偏移量
//     public static WorldManager Instance { get; private set; }

//     void Awake()
//     {
//         if (Instance == null)
//             Instance = this;
//         else
//             Destroy(gameObject);
//     }

//     void LateUpdate()
//     {
//         if (player == null) return;

//         // 当飞机远离当前原点时触发偏移
//         if (player.transform.position.magnitude > shiftThreshold)
//         {
//             ShiftWorldOrigin();
//         }
//     }

//     private void ShiftWorldOrigin()
//     {
//         Vector3 offset = player.transform.position; // 偏移量 = 飞机当前位置
//         totalOffset += offset; // 累计总偏移

//         // 移动所有场景物体（反向移动）
//         foreach (var obj in worldObjects)
//         {
//             if (obj != null)
//             {
//                 obj.transform.position -= offset; // 核心：向飞机反方向移动
//             }
//         }

//         // 处理粒子系统
//         foreach (var ps in FindObjectsOfType<ParticleSystem>())
//         {
//             ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
//             ps.transform.position -= offset;
//             ps.Play();
//         }

//         Debug.Log($"世界偏移：偏移量={offset}，总偏移={totalOffset}");
//     }

//     // 提供总偏移量（供飞机计算位置）
//     public Vector3 GetTotalOffset()
//     {
//         return totalOffset;
//     }
// }


// using UnityEngine;
// using System.Collections.Generic;

// public class WorldManager : MonoBehaviour
// {
//     public GameObject player; // 飞机对象（挂载AirplaneController的物体）
//     public float shiftThreshold = 1000f; // 触发偏移的阈值
//     public List<GameObject> worldObjects = new List<GameObject>(); // 需移动的场景物体（地形、机场等）

//     private Vector3 totalOffset; // 累计总偏移量
//     public static WorldManager Instance { get; private set; }

//     // 偏移事件（供飞机控制器监听）
//     public System.Action OnWorldShiftStart; // 偏移开始时触发
//     public System.Action OnWorldShiftEnd;   // 偏移结束时触发

//     void Awake()
//     {
//         if (Instance == null)
//             Instance = this;
//         else
//             Destroy(gameObject);
//     }

//     void LateUpdate()
//     {
//         if (player == null) return;

//         // 当飞机远离当前原点时触发偏移
//         if (player.transform.position.magnitude > shiftThreshold)
//         {
//             ShiftWorldOrigin();
//         }
//     }

//     private void ShiftWorldOrigin()
//     {
//         // 1. 通知开始偏移：飞机准备强制更新位置
//         OnWorldShiftStart?.Invoke();

//         // 2. 计算偏移量并移动世界
//         Vector3 offset = player.transform.position; // 偏移量 = 飞机当前位置
//         totalOffset += offset; // 累计总偏移

//         // 移动所有场景物体（反向移动）
//         foreach (var obj in worldObjects)
//         {
//             if (obj != null)
//             {
//                 obj.transform.position -= offset; // 核心：向飞机反方向移动
//             }
//         }

//         // 处理粒子系统
//         foreach (var ps in FindObjectsOfType<ParticleSystem>())
//         {
//             ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
//             ps.transform.position -= offset;
//             ps.Play();
//         }

//         Debug.Log($"世界偏移：偏移量={offset}，总偏移={totalOffset}");

//         // 3. 通知偏移结束：飞机恢复平滑移动
//         OnWorldShiftEnd?.Invoke();
//     }

//     // 提供总偏移量（供飞机计算位置）
//     public Vector3 GetTotalOffset()
//     {
//         return totalOffset;
//     }
// }



using UnityEngine;
using System.Collections.Generic;

public class WorldManager : MonoBehaviour
{
    public GameObject player; // 飞机对象
    public float shiftThreshold = 1000f; // 触发偏移的阈值
    public List<GameObject> worldObjects = new List<GameObject>(); // 需移动的场景物体

    private Vector3 totalOffset; // 累计总偏移量（X/Z有效，Y始终为0）
    public static WorldManager Instance { get; private set; }

    // 偏移事件
    public System.Action OnWorldShiftStart;
    public System.Action OnWorldShiftEnd;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void LateUpdate()
    {
        if (player == null) return;

        // 只根据水平距离判断是否偏移（忽略Y轴）
        float horizontalDistance = new Vector2(player.transform.position.x, player.transform.position.z).magnitude;
        if (horizontalDistance > shiftThreshold)
        {
            ShiftWorldOrigin();
        }
    }

    private void ShiftWorldOrigin()
    {
        OnWorldShiftStart?.Invoke();

        // 关键修复：只取X/Z轴偏移，Y轴设为0（避免影响场景高度）
        Vector3 playerPos = player.transform.position;
        Vector3 offset = new Vector3(playerPos.x, 0, playerPos.z); // Y轴偏移强制为0

        // 累计总偏移（Y轴始终为0）
        totalOffset += offset;

        // 移动场景物体（仅X/Z轴反向移动，Y轴不变）
        foreach (var obj in worldObjects)
        {
            if (obj != null)
            {
                // 只修改X/Z坐标，Y坐标保持原高度
                Vector3 newPos = obj.transform.position;
                newPos.x -= offset.x;
                newPos.z -= offset.z;
                obj.transform.position = newPos;
            }
        }

        // // 处理粒子系统（同样只修改X/Z）
        // foreach (var ps in FindObjectsOfType<ParticleSystem>())
        // {
        //     ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        //     Vector3 psNewPos = ps.transform.position;
        //     psNewPos.x -= offset.x;
        //     psNewPos.z -= offset.z;
        //     ps.transform.position = psNewPos;
        //     ps.Play();
        // }

        Debug.Log($"世界偏移（仅水平）：偏移量={offset}，总偏移={totalOffset}");

        OnWorldShiftEnd?.Invoke();
    }

    // 提供总偏移量（Y轴始终为0）
    public Vector3 GetTotalOffset()
    {
        return totalOffset;
    }
}