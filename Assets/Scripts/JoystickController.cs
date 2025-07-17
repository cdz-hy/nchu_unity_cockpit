// using System;
// using UnityEngine;

// public class JoystickController : MonoBehaviour
// {
//     public Transform steeringWheel1; // ?????????? Transform
//     public Transform steeringWheel2; // ?????????? Transform
//     public Transform rotatingObject; // ???????? Transform
//     public Camera pilotCamera; // ??????????????
//     public MeshCollider meshCollider; // ????????
//     public float currentStickAngle = 0; // ???????????
//     public float currentWheelAngle = 0; // ????????????
//     public float stickRotation = 0; // ??????????
//     public float wheelRotation = 0; // ???????????
//     public float stickRotationSpeed = 0.1f; // ???????????
//     public float wheelRotationSpeed = 0.5f; // ????????????
//     public float maxStickAngle = 10f; // ??????????????
//     public float maxWheelAngle = 110f; // ???????????????
//     public float maxObjectRotationAngle = 90f; // ????????????????
//     public float maxRotationSpeed = 40f; // ????????????????
//     public MeshCollider[] colliders; // ???????Mesh Collider???  

//     private Vector3 initialMousePosition;
//     private bool isDragging = false;

//     public Texture2D handCursor;    // ???????????  
//     public Texture2D clickCursor;   // ????????  
//     public Texture2D defaultCursor; // ?????  

//     // ??????????????????????????
//     public static event Action<float[]> joystickControllerRotation;

//     void Start()
//     {
//         colliders = GetComponentsInChildren<MeshCollider>();
//     }

//     void Update()
//     {
//         if (GetActiveCamera() == pilotCamera && Input.GetMouseButtonDown(0))
//         {
//             Ray ray = pilotCamera.ScreenPointToRay(Input.mousePosition);
//             if (meshCollider.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
//             {
//                 isDragging = true; // ?????????????
//                 initialMousePosition = Input.mousePosition; // ?????????????
//                 Cursor.SetCursor(clickCursor, Vector2.zero, CursorMode.Auto);  // ?????????? 
//             }

//         }

//         if (GetActiveCamera() == pilotCamera && Input.GetMouseButtonUp(0))
//         {
//             Ray ray = pilotCamera.ScreenPointToRay(Input.mousePosition);
//             if (meshCollider.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
//             {
//                 Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);  // ?????????? 
//             }
//             isDragging = false;
//             currentStickAngle = stickRotation;
//             currentWheelAngle = wheelRotation;
//         }

//         if (isDragging)
//         {
//             Vector3 mouseDelta = Input.mousePosition - initialMousePosition;
//             stickRotation = Mathf.Clamp(mouseDelta.y * stickRotationSpeed + currentStickAngle, -maxStickAngle, maxStickAngle);
//             wheelRotation = Mathf.Clamp(mouseDelta.x * wheelRotationSpeed + currentWheelAngle, -maxWheelAngle, maxWheelAngle);

//             // ????????
//             transform.localRotation = Quaternion.Euler(-stickRotation, 0, 0);

//             // ?????????
//             steeringWheel1.localRotation = Quaternion.Euler(0, 0, wheelRotation);
//             steeringWheel2.localRotation = Quaternion.Euler(0, 0, wheelRotation);

//             // ?????????????
//             float objectRotation = (wheelRotation / maxWheelAngle) * maxObjectRotationAngle;
//             Quaternion targetRotation = Quaternion.Euler(objectRotation, rotatingObject.localRotation.eulerAngles.y, rotatingObject.localRotation.eulerAngles.z);

//             // ??? RotateTowards ?????????????
//             rotatingObject.localRotation = Quaternion.RotateTowards(rotatingObject.localRotation, targetRotation, maxRotationSpeed * Time.deltaTime);


//             // ?????????????????????
//             float[] controllerDatas = { - stickRotation / maxStickAngle, wheelRotation / maxWheelAngle};
//             joystickControllerRotation?.Invoke(controllerDatas);


//         }
//     }

//     // ??????????????  
//     private void HandleMouseInteraction()
//     {
//         // ?????????????????????????
//         bool mouseOver = false;

//         foreach (var collider in colliders)
//         {
//             if (IsMouseOverMeshCollider(collider))
//             {
//                 mouseOver = true;
//                 // ?????????????????????
//                 if (!collider.gameObject.GetComponent<CursorChange>().isMouseOver)
//                 {
//                     collider.gameObject.GetComponent<CursorChange>().isMouseOver = true;
//                     collider.gameObject.SendMessage("OnMouseEnter");
//                 }
//                 break; // ?????????????????????
//             }
//         }

//         // ??????????????????????????
//         if (!mouseOver)
//         {
//             foreach (var collider in colliders)
//             {
//                 if (collider.gameObject.GetComponent<CursorChange>().isMouseOver)
//                 {
//                     collider.gameObject.GetComponent<CursorChange>().isMouseOver = false;
//                     collider.gameObject.SendMessage("OnMouseExit");
//                 }
//             }
//         }
//     }

//     // ???????????MeshCollider??????  
//     private bool IsMouseOverMeshCollider(MeshCollider collider)
//     {
//         Ray ray = GetActiveCamera().ScreenPointToRay(Input.mousePosition);
//         return collider.Raycast(ray, out _, Mathf.Infinity); // ???????????MeshCollider??????  
//     }

//     private Camera GetActiveCamera()
//     {
//         foreach (Camera cam in Camera.allCameras)
//         {
//             if (cam.gameObject.activeInHierarchy)
//             {
//                 return cam; // ??????????????????
//             }
//         }
//         return null; // ?????????????????????? null
//     }
// }


// using System;
// using UnityEngine;

// // ???????????????????????
// [DefaultExecutionOrder(200)]
// public class JoystickController : MonoBehaviour
// {
//     public Transform steeringWheel1; // ?????????? Transform
//     public Transform steeringWheel2; // ?????????? Transform
//     public Transform rotatingObject; // ???????? Transform
//     public Camera pilotCamera;       // ?????????????????????
//     public MeshCollider meshCollider; // ?????? MeshCollider????????????
//     public MeshCollider[] colliders;  // ?????????????? MeshCollider ???

//     public float currentStickAngle = 0f;   // ??????????
//     public float currentWheelAngle = 0f;   // ???????????
//     public float stickRotation = 0f;       // ???????????
//     public float wheelRotation = 0f;       // ????????????

//     public float stickRotationSpeed = 0.1f;    // ??????????????
//     public float wheelRotationSpeed = 0.5f;    // ???????????????
//     public float maxStickAngle = 10f;         // ??????????????
//     public float maxWheelAngle = 110f;        // ???????????????
//     public float maxObjectRotationAngle = 90f; // ????????????????
//     public float maxRotationSpeed = 40f;       // ?????????????????

//     public Texture2D handCursor;    // ??????????
//     public Texture2D clickCursor;   // ????????????
//     public Texture2D defaultCursor; // ?????????

//     private Vector3 initialMousePosition; // ????????????
//     private bool isDragging = false;      // ??????????

//     private Vector3 originalLocalPosition; // ????????????
//     private Quaternion originalLocalRotation; // ???????????

//     // ????????????????????????????????????
//     public static event Action<float[]> joystickControllerRotation;

//     void Start()
//     {
//         colliders = GetComponentsInChildren<MeshCollider>();
//         // ?????????????
//         originalLocalPosition = transform.localPosition;
//         originalLocalRotation = transform.localRotation;
//     }

//     void Update()
//     {
//         // ???????????????????????????
//         if (GetActiveCamera() == pilotCamera && Input.GetMouseButtonDown(0))
//         {
//             Ray ray = pilotCamera.ScreenPointToRay(Input.mousePosition);
//             if (meshCollider.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
//             {
//                 isDragging = true;
//                 initialMousePosition = Input.mousePosition;
//                 Cursor.SetCursor(clickCursor, Vector2.zero, CursorMode.Auto);
//             }
//         }

//         // ??????????????????????
//         if (GetActiveCamera() == pilotCamera && Input.GetMouseButtonUp(0))
//         {
//             Ray ray = pilotCamera.ScreenPointToRay(Input.mousePosition);
//             if (meshCollider.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
//                 Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);

//             isDragging = false;
//             currentStickAngle = stickRotation;
//             currentWheelAngle = wheelRotation;
//         }

//         // ?????????????????????
//         if (isDragging)
//         {
//             Vector3 mouseDelta = Input.mousePosition - initialMousePosition;
//             stickRotation = Mathf.Clamp(currentStickAngle + mouseDelta.y * stickRotationSpeed,
//                                         -maxStickAngle, maxStickAngle);
//             wheelRotation = Mathf.Clamp(currentWheelAngle + mouseDelta.x * wheelRotationSpeed,
//                                         -maxWheelAngle, maxWheelAngle);

//             // ???????
//             float[] controllerDatas = { -stickRotation / maxStickAngle, wheelRotation / maxWheelAngle };
//             joystickControllerRotation?.Invoke(controllerDatas);
//         }
//     }

//     void LateUpdate()
//     {
//         // ??????????????????????????????????
//         transform.localPosition = originalLocalPosition;
//         // ?????????????????????????????
//         transform.localRotation = originalLocalRotation * Quaternion.Euler(-stickRotation, 0f, 0f);

//         // ??????????????
//         steeringWheel1.localRotation = Quaternion.Euler(0f, 0f, wheelRotation);
//         steeringWheel2.localRotation = Quaternion.Euler(0f, 0f, wheelRotation);

//         // ?????????????
//         float objectRotation = (wheelRotation / maxWheelAngle) * maxObjectRotationAngle;
//         Quaternion target = Quaternion.Euler(objectRotation,
//                                              rotatingObject.localRotation.eulerAngles.y,
//                                              rotatingObject.localRotation.eulerAngles.z);
//         rotatingObject.localRotation = Quaternion.RotateTowards(
//             rotatingObject.localRotation,
//             target,
//             maxRotationSpeed * Time.deltaTime);
//     }

//     // ????????????????
//     private Camera GetActiveCamera()
//     {
//         foreach (Camera cam in Camera.allCameras)
//             if (cam.gameObject.activeInHierarchy)
//                 return cam;
//         return null;
//     }

//     // ??????????????????? MeshCollider ?????????????
//     private bool IsMouseOverMeshCollider(MeshCollider collider)
//     {
//         Ray ray = GetActiveCamera().ScreenPointToRay(Input.mousePosition);
//         return collider.Raycast(ray, out _, Mathf.Infinity);
//     }
// }




// using System;
// using UnityEngine;

// public class JoystickController : MonoBehaviour
// {
//     public Transform steeringWheel1;       // ?????????? Transform
//     public Transform steeringWheel2;       // ?????????? Transform
//     public Transform rotatingObject;       // ???????? Transform
//     public Camera pilotCamera;             // ??????????????
//     public MeshCollider meshCollider;      // ?????? MeshCollider
//     public MeshCollider[] colliders;       // ??????????? MeshCollider ???

//     public Texture2D handCursor;           // ???????????
//     public Texture2D clickCursor;          // ????????
//     public Texture2D defaultCursor;        // ?????

//     [Header("?????? & ????")]
//     public float stickRotationSpeed = 0.1f;     // ????????????????????
//     public float wheelRotationSpeed = 0.5f;     // ?????????????????????
//     public float maxStickAngle = 10f;           // ??????????????
//     public float maxWheelAngle = 110f;          // ???????????????
//     public float maxObjectRotationAngle = 90f;  // ????????????????
//     public float maxRotationSpeed = 40f;        // ????????????????

//     private float currentStickAngle = 0f;       // ??????????
//     private float currentWheelAngle = 0f;       // ???????????
//     private bool isDragging = false;

//     // ?????????????
//     private Vector3 initialMouseScreenPos;
//     private float initialStickAngle;
//     private float initialWheelAngle;

//     // ????????????????????????
//     public static event Action<float[]> joystickControllerRotation;

//     void Start()
//     {
//         colliders = GetComponentsInChildren<MeshCollider>();
//     }

//     void Update()
//     {
//         Camera cam = GetActiveCamera();
//         if (cam != pilotCamera) return;

//         // ??????????????????????????
//         if (Input.GetMouseButtonDown(0))
//         {
//             Ray ray = cam.ScreenPointToRay(Input.mousePosition);
//             if (HitJoystick(ray))
//             {
//                 isDragging = true;
//                 initialMouseScreenPos = Input.mousePosition;
//                 initialStickAngle = currentStickAngle;
//                 initialWheelAngle = currentWheelAngle;
//                 Cursor.SetCursor(clickCursor, Vector2.zero, CursorMode.Auto);
//             }
//         }

//         // ????????????????????
//         if (Input.GetMouseButtonUp(0) && isDragging)
//         {
//             isDragging = false;
//             float x = transform.rotation.eulerAngles.x;
//             currentStickAngle = x > 180f ? x - 360f : x;
//             float z = steeringWheel1.rotation.eulerAngles.z;
//             currentWheelAngle = z > 180f ? z - 360f : z;
//             Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
//         }

//         // ???????????????????????
//         if (isDragging)
//         {
//             Vector3 delta = Input.mousePosition - initialMouseScreenPos;
//             float stickAngle = Mathf.Clamp(initialStickAngle + delta.y * stickRotationSpeed, -maxStickAngle, maxStickAngle);
//             float wheelAngle = Mathf.Clamp(initialWheelAngle + delta.x * wheelRotationSpeed, -maxWheelAngle, maxWheelAngle);

//             // ?????????
//             Quaternion parentRot = transform.parent.rotation;
//             transform.rotation = parentRot * Quaternion.Euler(-stickAngle, 0f, 0f);
//             steeringWheel1.rotation = parentRot * Quaternion.Euler(0f, 0f, wheelAngle);
//             steeringWheel2.rotation = steeringWheel1.rotation;

//             currentStickAngle = stickAngle;
//             currentWheelAngle = wheelAngle;

//             // ?????????????????
//             float objTarget = (wheelAngle / maxWheelAngle) * maxObjectRotationAngle;
//             Quaternion tgt = Quaternion.Euler(objTarget, rotatingObject.rotation.eulerAngles.y, rotatingObject.rotation.eulerAngles.z);
//             rotatingObject.rotation = Quaternion.RotateTowards(rotatingObject.rotation, tgt, maxRotationSpeed * Time.deltaTime);

//             // ???????????
//             float[] data = { -stickAngle / maxStickAngle, wheelAngle / maxWheelAngle };
//             joystickControllerRotation?.Invoke(data);
//         }

//         // ????????????
//         HandleMouseInteraction(cam);
//     }

//     // ??? Physics.Raycast ??? Collider.Raycast
//     private bool HitJoystick(Ray ray)
//     {
//         if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
//         {
//             return hit.collider == meshCollider;
//         }
//         return false;
//     }

//     // ??????/??????????
//     private void HandleMouseInteraction(Camera cam)
//     {
//         bool over = false;
//         Ray ray = cam.ScreenPointToRay(Input.mousePosition);
//         foreach (var col in colliders)
//         {
//             if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity) && hit.collider == col)
//             {
//                 var cc = col.gameObject.GetComponent<CursorChange>();
//                 if (!cc.isMouseOver)
//                 {
//                     cc.isMouseOver = true;
//                     col.gameObject.SendMessage("OnMouseEnter");
//                 }
//                 over = true;
//                 break;
//             }
//         }
//         if (!over)
//         {
//             foreach (var col in colliders)
//             {
//                 var cc = col.gameObject.GetComponent<CursorChange>();
//                 if (cc.isMouseOver)
//                 {
//                     cc.isMouseOver = false;
//                     col.gameObject.SendMessage("OnMouseExit");
//                 }
//             }
//         }
//     }

//     // ????????????
//     private Camera GetActiveCamera()
//     {
//         foreach (var cam in Camera.allCameras)
//             if (cam.gameObject.activeInHierarchy)
//                 return cam;
//         return null;
//     }
// }

// using System;
// using UnityEngine;

// public class JoystickController : MonoBehaviour
// {
//     [Header("References")]
//     public Transform steeringWheel1;     // ????????? Transform
//     public Transform steeringWheel2;     // ????????? Transform
//     public Transform rotatingObject;     // ??????? Transform
//     public Camera pilotCamera;           // ?????????
//     public MeshCollider meshCollider;    // ????????????????
//     public MeshCollider[] colliders;     // ?????????? MeshCollider

//     [Header("Cursor Textures")]
//     public Texture2D clickCursor;        // ????????
//     public Texture2D defaultCursor;      // ?????

//     [Header("Sensitivity & Limits")]
//     public float stickRotationSpeed = 0.1f;   // ?????????????? 
//     public float wheelRotationSpeed = 0.5f;   // ???????????????
//     public float maxStickAngle = 10f;         // ?????????
//     public float maxWheelAngle = 110f;        // ???????
//     public float maxObjectRotationAngle = 90f;// ???????????????
//     public float maxRotationSpeed = 40f;      // ??????????????????

//     // ?????
//     private bool isDragging = false;
//     private Vector3 initialMousePos;
//     private float initialStickAngle;
//     private float initialWheelAngle;

//     // ??????????????
//     public static event Action<float[]> joystickControllerRotation;

//     void Start()
//     {
//         // ??????????????????
//         colliders = GetComponentsInChildren<MeshCollider>();
//     }

//     void Update()
//     {
//         if (pilotCamera == null) return;
//         Camera cam = pilotCamera;

//         // ????????????????????????
//         if (Input.GetMouseButtonDown(0))
//         {
//             Ray ray = cam.ScreenPointToRay(Input.mousePosition);
//             if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity) && hit.collider == meshCollider)
//             {
//                 isDragging = true;
//                 initialMousePos = Input.mousePosition;  // ????????????? 
//                 // ???????????????
//                 initialStickAngle = GetWorldStickAngle();
//                 initialWheelAngle = GetWorldWheelAngle();
//                 Cursor.SetCursor(clickCursor, Vector2.zero, CursorMode.Auto);
//             }
//         }

//         // ?????????
//         if (Input.GetMouseButtonUp(0) && isDragging)
//         {
//             isDragging = false;
//             Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
//         }

//         // ???????????????????????????
//         if (isDragging)
//         {
//             Vector2 delta = (Vector2)Input.mousePosition - (Vector2)initialMousePos;
//             float stickAngle = Mathf.Clamp(initialStickAngle + delta.y * stickRotationSpeed, -maxStickAngle, maxStickAngle);
//             float wheelAngle = Mathf.Clamp(initialWheelAngle + delta.x * wheelRotationSpeed, -maxWheelAngle, maxWheelAngle);

//             // ?????????????
//             Quaternion parentRot = transform.parent ? transform.parent.rotation : Quaternion.identity;
//             // ?????????????????? :contentReference[oaicite:4]{index=4}
//             transform.rotation = parentRot * Quaternion.Euler(-stickAngle, 0f, 0f);
//             steeringWheel1.rotation = parentRot * Quaternion.Euler(0f, 0f, wheelAngle);
//             steeringWheel2.rotation = steeringWheel1.rotation;

//             // ??????????????
//             float objTarget = (wheelAngle / maxWheelAngle) * maxObjectRotationAngle;
//             Quaternion tgt = Quaternion.Euler(objTarget, rotatingObject.rotation.eulerAngles.y, rotatingObject.rotation.eulerAngles.z);
//             rotatingObject.rotation = Quaternion.RotateTowards(rotatingObject.rotation, tgt, maxRotationSpeed * Time.deltaTime);

//             // ????????????
//             joystickControllerRotation?.Invoke(new float[] { -stickAngle / maxStickAngle, wheelAngle / maxWheelAngle });
//         }

//         // ?????????????????
//         HandleMouseHover(cam);
//     }

//     // ??????????????????
//     private float GetWorldStickAngle()
//     {
//         float x = transform.rotation.eulerAngles.x;
//         return x > 180f ? x - 360f : x;
//     }

//     // ??????????????????
//     private float GetWorldWheelAngle()
//     {
//         float z = steeringWheel1.rotation.eulerAngles.z;
//         return z > 180f ? z - 360f : z;
//     }

//     // ???????????? Physics.Raycast ????????????? :contentReference[oaicite:5]{index=5}
//     private void HandleMouseHover(Camera cam)
//     {
//         Ray ray = cam.ScreenPointToRay(Input.mousePosition);
//         bool over = false;
//         if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
//         {
//             foreach (var col in colliders)
//             {
//                 if (hit.collider == col)
//                 {
//                     var cc = col.GetComponent<CursorChange>();
//                     if (!cc.isMouseOver)
//                     {
//                         cc.isMouseOver = true;
//                         col.SendMessage("OnMouseEnter");
//                     }
//                     over = true;
//                     break;
//                 }
//             }
//         }
//         if (!over)
//         {
//             foreach (var col in colliders)
//             {
//                 var cc = col.GetComponent<CursorChange>();
//                 if (cc.isMouseOver)
//                 {
//                     cc.isMouseOver = false;
//                     col.SendMessage("OnMouseExit");
//                 }
//             }
//         }
//     }
// }

//using System;
//using UnityEngine;
//using UnityEngine.EventSystems;

//public class JoystickController : MonoBehaviour
//{
//    public Transform steeringWheel1;
//    public Transform steeringWheel2;
//    public Transform rotatingObject;
//    public Camera pilotCamera;
//    public MeshCollider meshCollider;

//    public float currentStickAngle = 0;
//    public float currentWheelAngle = 0;

//    public float stickRotation = 0;
//    public float wheelRotation = 0;

//    public float stickRotationSpeed = 0.1f;
//    public float wheelRotationSpeed = 0.5f;

//    public float maxStickAngle = 10f;
//    public float maxWheelAngle = 110f;

//    public float maxObjectRotationAngle = 90f;
//    public float maxRotationSpeed = 40f;

//    private Vector3 initialMousePosition;
//    private bool isDragging = false;

//    // 新增：操作模式相关变量
//    private bool isFreeControlMode = false;  // 是否为自由操作模式
//    private float lastClickTime = 0f;        // 上次点击时间
//    private float doubleClickThreshold = 0.3f; // 双击时间阈值
//    private Vector3 freeControlInitialMousePos; // 自由操作模式的初始鼠标位置

//    public Texture2D handCursor;
//    public Texture2D clickCursor;
//    public Texture2D defaultCursor;

//    public static event Action<float[]> joystickControllerRotation;

//    // 用于射线检测的层掩码
//    private LayerMask m_LayerMask;

//    void Start()
//    {
//        // 启用物理自动同步
//        Physics.autoSyncTransforms = true;

//        // 解除光标锁定
//        Cursor.lockState = CursorLockMode.None;
//        Cursor.visible = true;

//        // 设置射线检测层掩码（排除UI层）
//        m_LayerMask = LayerMask.GetMask("Default"); // 根据实际图层名称调整

//        // 存储初始鼠标位置
//        initialMousePosition = Input.mousePosition;
//    }

//    void Update()
//    {
//        // 检查当前活动摄像机是否为pilotCamera
//        if (GetActiveCamera() == pilotCamera)
//        {
//            HandleMouseInput();

//            // 处理拖拽控制
//            if (isDragging || isFreeControlMode)
//            {
//                HandleJoystickControl();
//            }
//        }
//    }

//    private void HandleMouseInput()
//    {
//        if (Input.GetMouseButtonDown(0))
//        {
//            // 检查鼠标是否在UI上
//            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
//            {
//                return;
//            }

//            // 检测双击
//            float currentTime = Time.time;
//            if (currentTime - lastClickTime < doubleClickThreshold)
//            {
//                // 双击检测成功，切换操作模式
//                ToggleControlMode();
//                lastClickTime = 0f; // 重置点击时间，避免三击误触发
//                return;
//            }
//            lastClickTime = currentTime;

//            // 如果不是自由操作模式，执行原始的拖拽检测
//            if (!isFreeControlMode)
//            {
//                // 发射射线检测碰撞
//                Ray ray = pilotCamera.ScreenPointToRay(Input.mousePosition);
//                if (meshCollider.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
//                {
//                    isDragging = true;
//                    initialMousePosition = Input.mousePosition;
//                    Cursor.SetCursor(clickCursor, Vector2.zero, CursorMode.Auto);
//                }
//            }
//        }

//        if (Input.GetMouseButtonUp(0))
//        {
//            if (isDragging)
//            {
//                isDragging = false;
//                currentStickAngle = stickRotation;
//                currentWheelAngle = wheelRotation;

//                // 检查是否在UI上释放
//                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
//                {
//                    return;
//                }

//                // 检测释放时的碰撞
//                Ray ray = pilotCamera.ScreenPointToRay(Input.mousePosition);
//                if (meshCollider.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
//                {
//                    Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
//                }
//                else
//                {
//                    // 如果不在任何碰撞体上，恢复默认光标
//                    Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
//                }
//            }
//        }
//    }

//    private void ToggleControlMode()
//    {
//        isFreeControlMode = !isFreeControlMode;

//        if (isFreeControlMode)
//        {
//            // 进入自由操作模式
//            freeControlInitialMousePos = Input.mousePosition;
//            currentStickAngle = stickRotation;
//            currentWheelAngle = wheelRotation;
//            Cursor.SetCursor(clickCursor, Vector2.zero, CursorMode.Auto);
//            Debug.Log("进入自由操作模式");
//        }
//        else
//        {
//            // 退出自由操作模式
//            currentStickAngle = stickRotation;
//            currentWheelAngle = wheelRotation;
//            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
//            Debug.Log("退出自由操作模式");
//        }
//    }

//    private void HandleJoystickControl()
//    {
//        Vector3 mouseDelta;

//        if (isFreeControlMode)
//        {
//            // 自由操作模式：使用自由操作模式的初始位置
//            mouseDelta = Input.mousePosition - freeControlInitialMousePos;
//        }
//        else
//        {
//            // 原始模式：使用拖拽开始时的初始位置
//            mouseDelta = Input.mousePosition - initialMousePosition;
//        }

//        // 计算操纵杆旋转角度
//        stickRotation = Mathf.Clamp(-mouseDelta.y * stickRotationSpeed + currentStickAngle, -maxStickAngle, maxStickAngle);
//        // 计算方向盘旋转角度
//        wheelRotation = Mathf.Clamp(mouseDelta.x * wheelRotationSpeed + currentWheelAngle, -maxWheelAngle, maxWheelAngle);

//        // 旋转操纵杆
//        transform.localRotation = Quaternion.Euler(stickRotation, 0, 0);

//        // 旋转方向盘
//        steeringWheel1.localRotation = Quaternion.Euler(0, 0, wheelRotation);
//        steeringWheel2.localRotation = Quaternion.Euler(0, 0, wheelRotation);

//        // 计算配平手轮目标旋转角度
//        float objectRotation = (wheelRotation / maxWheelAngle) * maxObjectRotationAngle;
//        Quaternion targetRotation = Quaternion.Euler(
//            objectRotation,
//            rotatingObject.localRotation.eulerAngles.y,
//            rotatingObject.localRotation.eulerAngles.z
//        );

//        // 限制最大旋转速度
//        rotatingObject.localRotation = Quaternion.RotateTowards(
//            rotatingObject.localRotation,
//            targetRotation,
//            maxRotationSpeed * Time.deltaTime
//        );

//        // 触发事件，传递数据数组
//        float[] controllerDatas = {
//            stickRotation / maxStickAngle,
//            wheelRotation / maxWheelAngle
//        };
//        joystickControllerRotation?.Invoke(controllerDatas);
//    }

//    // ... existing code ...

//    // 检测鼠标是否在指定层上的碰撞体上
//    private bool IsMouseOverValidLayer(MeshCollider collider)
//    {
//        Ray ray = GetActiveCamera().ScreenPointToRay(Input.mousePosition);
//        return collider.Raycast(ray, out RaycastHit hit, Mathf.Infinity)
//            && (hit.collider.gameObject.layer & m_LayerMask) != 0;
//    }

//    private Camera GetActiveCamera()
//    {
//        foreach (Camera cam in Camera.allCameras)
//        {
//            if (cam.gameObject.activeInHierarchy)
//            {
//                return cam;
//            }
//        }
//        return null;
//    }
//}



using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;

public class JoystickController : MonoBehaviour
{
    // —— PInvoke Windows Joystick API ——
    private const uint JOYSTICKID1 = 0;
    private const int JOY_RETURNRAWDATA = 0x00000004;
    private const int JOY_RETURNBUTTONS = 0x00000080;

    [StructLayout(LayoutKind.Sequential)]
    private struct JOYINFOEX
    {
        public uint dwSize;
        public uint dwFlags;
        public uint dwXpos;
        public uint dwYpos;
        public uint dwZpos;
        public uint dwRpos;
        public uint dwUpos;
        public uint dwVpos;
        public uint dwButtons;
        public uint dwButtonNumber;
        public uint dwPOV;
        public uint dwReserved1;
        public uint dwReserved2;
    }

    [DllImport("winmm.dll")]
    private static extern int joyGetPosEx(uint uJoyID, ref JOYINFOEX pji);

    private JOYINFOEX joystickInfo;

    // —— References & UI ——
    public Transform steeringWheel1;
    public Transform steeringWheel2;
    public Transform rotatingObject;
    public Camera pilotCamera;
    public MeshCollider meshCollider;

    // —— Current state ——
    public float currentStickAngle = 0;
    public float currentWheelAngle = 0;
    private float stickRotation = 0;
    private float wheelRotation = 0;

    // —— Configurable parameters ——
    public float stickRotationSpeed = 0.1f;
    public float wheelRotationSpeed = 0.5f;
    public float maxStickAngle = 10f;
    public float maxWheelAngle = 110f;
    public float maxObjectRotationAngle = 90f;
    public float maxRotationSpeed = 40f;

    private Vector3 initialMousePosition;
    private bool isDragging = false;

    public Texture2D handCursor;
    public Texture2D clickCursor;
    public Texture2D defaultCursor;

    public static event Action<float[]> joystickControllerRotation;

    private LayerMask m_LayerMask;

    public FirstPersonController firstPersonController;

    void Start()
    {
        Physics.autoSyncTransforms = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        m_LayerMask = LayerMask.GetMask("Default");
        initialMousePosition = Input.mousePosition;

        // 初始化 joystickInfo
        joystickInfo.dwSize = (uint)Marshal.SizeOf(typeof(JOYINFOEX));
        joystickInfo.dwFlags = JOY_RETURNRAWDATA | JOY_RETURNBUTTONS;
    }

    void Update()
    {
        // —— 如果检测到硬件摇杆接入，优先使用它的输入 ——
        string[] names = Input.GetJoystickNames();
        bool joystickConnected = names.Length > 0 && !string.IsNullOrEmpty(names[0]);
        if (joystickConnected)
        {
            if (firstPersonController != null)
            {
                firstPersonController.speed = 0;
            }
            EventSystem.current.sendNavigationEvents = !joystickConnected;
            int result = joyGetPosEx(JOYSTICKID1, ref joystickInfo);
            if (result == 0)
            {
                // 原始值范围 0..65535，归一化到 -1..1
                float normX = (joystickInfo.dwXpos / 65535f) * 2f - 1f;
                float normY = (joystickInfo.dwYpos / 65535f) * 2f - 1f;
                // 前后方向反转（pitch 轴正为抬起），映射到旋转角度
                stickRotation = Mathf.Clamp(normY * maxStickAngle, -maxStickAngle, maxStickAngle);
                // 左右不反转
                wheelRotation = Mathf.Clamp(normX * maxWheelAngle, -maxWheelAngle, maxWheelAngle);

                // 应用到转动
                transform.localRotation = Quaternion.Euler(stickRotation, 0, 0);
                steeringWheel1.localRotation = Quaternion.Euler(0, 0, wheelRotation);
                steeringWheel2.localRotation = Quaternion.Euler(0, 0, wheelRotation);
                float objectRot = (wheelRotation / maxWheelAngle) * maxObjectRotationAngle;
                rotatingObject.localRotation = Quaternion.RotateTowards(
                    rotatingObject.localRotation,
                    Quaternion.Euler(objectRot, rotatingObject.localRotation.eulerAngles.y, rotatingObject.localRotation.eulerAngles.z),
                    maxRotationSpeed * Time.deltaTime
                );

                // 触发回调
                joystickControllerRotation?.Invoke(new float[] { stickRotation / maxStickAngle, wheelRotation / maxWheelAngle });

                return;
            }
        }

        // —— 否则回到原有鼠标拖拽逻辑 ——
        if (GetActiveCamera() == pilotCamera && Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
            Ray ray = pilotCamera.ScreenPointToRay(Input.mousePosition);
            if (meshCollider.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                isDragging = true;
                initialMousePosition = Input.mousePosition;
                Cursor.SetCursor(clickCursor, Vector2.zero, CursorMode.Auto);
            }
        }

        if (GetActiveCamera() == pilotCamera && Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            currentStickAngle = stickRotation;
            currentWheelAngle = wheelRotation;
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        }

        if (isDragging)
        {
            Vector3 mouseDelta = Input.mousePosition - initialMousePosition;
            stickRotation = Mathf.Clamp(-mouseDelta.y * stickRotationSpeed + currentStickAngle, -maxStickAngle, maxStickAngle);
            wheelRotation = Mathf.Clamp(mouseDelta.x * wheelRotationSpeed + currentWheelAngle, -maxWheelAngle, maxWheelAngle);

            transform.localRotation = Quaternion.Euler(stickRotation, 0, 0);
            steeringWheel1.localRotation = Quaternion.Euler(0, 0, wheelRotation);
            steeringWheel2.localRotation = Quaternion.Euler(0, 0, wheelRotation);

            float objectRotation = (wheelRotation / maxWheelAngle) * maxObjectRotationAngle;
            rotatingObject.localRotation = Quaternion.RotateTowards(
                rotatingObject.localRotation,
                Quaternion.Euler(objectRotation, rotatingObject.localRotation.eulerAngles.y, rotatingObject.localRotation.eulerAngles.z),
                maxRotationSpeed * Time.deltaTime
            );

            joystickControllerRotation?.Invoke(new float[] { stickRotation / maxStickAngle, wheelRotation / maxWheelAngle });
        }
    }

    private Camera GetActiveCamera()
    {
        foreach (Camera cam in Camera.allCameras)
            if (cam.gameObject.activeInHierarchy) return cam;
        return null;
    }
}
