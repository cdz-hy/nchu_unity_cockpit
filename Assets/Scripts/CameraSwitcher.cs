using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitcher : MonoBehaviour
{
    [System.Serializable]
    public class PlayerState
    {
        [Header("基本信息")]
        public string stateName;

        [Header("Transform参数")]
        public Vector3 position;
        public Vector3 rotation;

        [Header("第一人称控制器参数")]
        public bool useFirstPersonController = true;
        public float speed = 5f;
        public float mouseSensitivity = 2f;
        public float verticalSpeed = 3f;
        public float zoomSpeed = 25f;
        public float minFocalLength = 10f;
        public float maxFocalLength = 60f;
        public float raycastDistance = 1f;
        public bool isCollision = false;
        public bool horizontalLock = false;

        [Header("轨道相机参数")]
        public bool useOrbitCamera = false;
        public Transform orbitTarget;
        public float orbitDistance = 30f;
        public float orbitHorizontalSpeed = 20f;
        public float orbitVerticalSpeed = 20f;
        public float orbitZoomSpeed = 25f;
        public float orbitMinFocalLength = 10f;
        public float orbitMaxFocalLength = 60f;
    }

    [Header("玩家对象引用")]
    public GameObject player;
    public Camera playerCamera;

    [Header("控制器组件")]
    public FirstPersonController firstPersonController;
    public CameraOrbit cameraOrbit;

    [Header("玩家状态配置")]
    public PlayerState[] playerStates = new PlayerState[4];

    private int currentStateIndex = 0;

    void Start()
    {
        // 初始化默认状态参数
        InitializeDefaultStates();

        // 应用第一个状态
        ApplyPlayerState(currentStateIndex);
    }

    void InitializeDefaultStates()
    {
        // 状态1 - JN Player
        playerStates[0] = new PlayerState
        {
            stateName = "JN Player",
            position = new Vector3(0.502390f, 0.66f, 2.6f),
            rotation = new Vector3(0.412f, -176.286f, -0.024f),
            useFirstPersonController = true,
            speed = 1f,
            mouseSensitivity = 2f,
            verticalSpeed = 0.8f,
            zoomSpeed = 25f,
            minFocalLength = 10f,
            maxFocalLength = 60f,
            raycastDistance = 1.5f,
            isCollision = true,
            horizontalLock = false
        };

        // 状态2 - Orbit Player
        playerStates[1] = new PlayerState
        {
            stateName = "Orbit Player",
            position = new Vector3(-21.9737f, 2.967265f, 4.732489f),
            rotation = new Vector3(5.225f, 59.27f, -0.018f),
            useFirstPersonController = false,
            useOrbitCamera = true,
            orbitDistance = 30f,
            orbitHorizontalSpeed = 20f,
            orbitVerticalSpeed = 20f,
            orbitZoomSpeed = 25f,
            orbitMinFocalLength = 10f,
            orbitMaxFocalLength = 60f
        };

        // 状态3 - Tall Player
        playerStates[2] = new PlayerState
        {
            stateName = "Tall Player",
            position = new Vector3(0.001112f, -0.21468f, 43.17944f),
            rotation = new Vector3(-7.218f, 179.898f, 0.001f),
            useFirstPersonController = true,
            speed = 0f,
            mouseSensitivity = 2f,
            verticalSpeed = 3f,
            zoomSpeed = 25f,
            minFocalLength = 10f,
            maxFocalLength = 60f,
            raycastDistance = 1.5f,
            isCollision = false,
            horizontalLock = false
        };

        // 状态4 - Player
        playerStates[3] = new PlayerState
        {
            stateName = "Player",
            position = new Vector3(0f, 0f, -7.29000f),
            rotation = new Vector3(0f, 0f, 0f),
            useFirstPersonController = true,
            speed = 5f,
            mouseSensitivity = 2f,
            verticalSpeed = 3f,
            zoomSpeed = 25f,
            minFocalLength = 10f,
            maxFocalLength = 60f,
            raycastDistance = 1f,
            isCollision = false,
            horizontalLock = false
        };
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchToNextState();
        }
    }

    void SwitchToNextState()
    {
        // 重置当前控制器状态（重要：与原脚本保持一致）
        ResetCurrentControllers();

        currentStateIndex = (currentStateIndex + 1) % playerStates.Length;
        ApplyPlayerState(currentStateIndex);

        Debug.Log($"切换到状态: {playerStates[currentStateIndex].stateName}");
    }

    void ResetCurrentControllers()
    {
        // 重置第一人称控制器（调用Reset方法重置rotationX和rotationY）
        if (firstPersonController != null && firstPersonController.enabled)
        {
            firstPersonController.Reset();
        }

        // 重置轨道相机控制器
        if (cameraOrbit != null && cameraOrbit.enabled)
        {
            cameraOrbit.Reset();
        }
    }

    void ApplyPlayerState(int stateIndex)
    {
        if (stateIndex < 0 || stateIndex >= playerStates.Length)
        {
            Debug.LogError("无效的状态索引");
            return;
        }

        PlayerState state = playerStates[stateIndex];

        // 重置玩家位置和旋转
        ResetPlayerTransform(state);

        // 根据状态启用相应的控制器
        if (state.useFirstPersonController)
        {
            EnableFirstPersonController(state);
            DisableOrbitCamera();
        }
        else if (state.useOrbitCamera)
        {
            EnableOrbitCamera(state);
            DisableFirstPersonController();
        }
        else
        {
            DisableFirstPersonController();
            DisableOrbitCamera();
        }
    }

    void ResetPlayerTransform(PlayerState state)
    {
        // 重置位置和旋转
        player.transform.localPosition = state.position;
        player.transform.eulerAngles = state.rotation;

        // 重置速度（如果有Rigidbody）
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    void EnableFirstPersonController(PlayerState state)
    {
        if (firstPersonController != null)
        {
            firstPersonController.enabled = true;

            // 应用第一人称控制器参数
            firstPersonController.speed = state.speed;
            firstPersonController.mouseSensitivity = state.mouseSensitivity;
            firstPersonController.verticalSpeed = state.verticalSpeed;
            firstPersonController.zoomSpeed = state.zoomSpeed;
            firstPersonController.minFocalLength = state.minFocalLength;
            firstPersonController.maxFocalLength = state.maxFocalLength;
            firstPersonController.raycastDistance = state.raycastDistance;
            firstPersonController.isCollision = state.isCollision;
            firstPersonController.horizontalLock = state.horizontalLock;

            Debug.Log($"启用第一人称控制器 - 速度: {state.speed}, 鼠标灵敏度: {state.mouseSensitivity}");
        }
    }

    void DisableFirstPersonController()
    {
        if (firstPersonController != null)
        {
            firstPersonController.enabled = false;
        }
    }

    void EnableOrbitCamera(PlayerState state)
    {
        if (cameraOrbit != null)
        {
            cameraOrbit.enabled = true;

            // 应用轨道相机参数
            if (state.orbitTarget != null)
            {
                cameraOrbit.target = state.orbitTarget;
            }
            cameraOrbit.distance = state.orbitDistance;
            cameraOrbit.horizontalSpeed = state.orbitHorizontalSpeed;
            cameraOrbit.verticalSpeed = state.orbitVerticalSpeed;
            cameraOrbit.zoomSpeed = state.orbitZoomSpeed;
            cameraOrbit.minFocalLength = state.orbitMinFocalLength;
            cameraOrbit.maxFocalLength = state.orbitMaxFocalLength;

            Debug.Log($"启用轨道相机 - 距离: {state.orbitDistance}, 水平速度: {state.orbitHorizontalSpeed}");
        }
    }

    void DisableOrbitCamera()
    {
        if (cameraOrbit != null)
        {
            cameraOrbit.enabled = false;
        }
    }

    // 调试功能：手动切换到指定状态
    public void SwitchToState(int stateIndex)
    {
        if (stateIndex >= 0 && stateIndex < playerStates.Length)
        {
            // 重置当前控制器状态
            ResetCurrentControllers();

            currentStateIndex = stateIndex;
            ApplyPlayerState(currentStateIndex);
        }
    }

    // 获取当前状态信息
    public string GetCurrentStateName()
    {
        return playerStates[currentStateIndex].stateName;
    }
    // 获取当前状态索引
    public int GetCurrentStateIndex()
    {
        return currentStateIndex;
    }
}