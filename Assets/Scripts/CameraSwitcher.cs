using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera[] cameras; // 存储所有相机的引用数组
    private int currentCameraIndex;
    private Vector3[] initialPositions;
    private Quaternion[] initialRotations;
    public FirstPersonController[] controllers; // 存储所有 FirstPersonController 的引用数组
    public CameraOrbit orbit;

    void Start()
    {
        // 存储初始位置和旋转
        initialPositions = new Vector3[cameras.Length];
        initialRotations = new Quaternion[cameras.Length];

        for (int i = 0; i < cameras.Length; i++)
        {
            initialPositions[i] = cameras[i].transform.position;
            initialRotations[i] = cameras[i].transform.rotation;
            cameras[i].gameObject.SetActive(i == 0);
        }
        currentCameraIndex = 0;
    }

    void Update()
    {
        // 检查按键以切换相机
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchCamera();
        }
    }

    void SwitchCamera()
    {
        // 禁用当前相机并重置其位置和旋转
        cameras[currentCameraIndex].gameObject.SetActive(false);
        cameras[currentCameraIndex].transform.localPosition = initialPositions[currentCameraIndex];
        cameras[currentCameraIndex].transform.localRotation = initialRotations[currentCameraIndex];

        // 重置当前控制器的 rotationX 和 rotationY
        if (controllers[currentCameraIndex] != null)
        {
            controllers[currentCameraIndex].Reset();
        }

        if (orbit != null) { orbit.Reset(); }

        // 切换到下一个相机索引
        currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;

        // 激活新的当前相机
        cameras[currentCameraIndex].gameObject.SetActive(true);
    }
}