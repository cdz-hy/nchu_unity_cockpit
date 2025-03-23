using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera[] cameras; // �洢�����������������
    private int currentCameraIndex;
    private Vector3[] initialPositions;
    private Quaternion[] initialRotations;
    public FirstPersonController[] controllers; // �洢���� FirstPersonController ����������
    public CameraOrbit orbit;

    void Start()
    {
        // �洢��ʼλ�ú���ת
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
        // ��鰴�����л����
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchCamera();
        }
    }

    void SwitchCamera()
    {
        // ���õ�ǰ�����������λ�ú���ת
        cameras[currentCameraIndex].gameObject.SetActive(false);
        cameras[currentCameraIndex].transform.localPosition = initialPositions[currentCameraIndex];
        cameras[currentCameraIndex].transform.localRotation = initialRotations[currentCameraIndex];

        // ���õ�ǰ�������� rotationX �� rotationY
        if (controllers[currentCameraIndex] != null)
        {
            controllers[currentCameraIndex].Reset();
        }

        if (orbit != null) { orbit.Reset(); }

        // �л�����һ���������
        currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;

        // �����µĵ�ǰ���
        cameras[currentCameraIndex].gameObject.SetActive(true);
    }
}