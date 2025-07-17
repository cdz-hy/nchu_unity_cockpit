using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineRotation1 : MonoBehaviour
{
    // 最大旋转速度（单位：度/秒）
    public float maxRotationSpeed = 360f; // 每秒旋转360度

    // 音频相关组件
    [Header("音频设置")]
    public AudioSource audioSource;
    public AudioClip cabinEngineSound; // 舱内引擎声
    public AudioClip externalEngineSound; // 舱外引擎声

    // 音频控制参数
    [Header("音频控制")]
    public float minVolume = 0.1f; // 最小音量
    public float maxVolume = 1.0f; // 最大音量
    public float volumeChangeSpeed = 2.0f; // 音量变化速度
    public float engineRPMThreshold = 5f; // 引擎转速阈值，低于此值停止播放
    public bool useAverageRPM = true; // 是否使用左右引擎平均转速

    // 引用CameraSwitcher
    private PlayerSwitcher playerSwitcher;
    private AudioClip currentEngineSound;
    private bool isAudioInitialized = false;

    // Start is called before the first frame update
    void Start()
    {
        // 查找PlayerSwitcher组件
        playerSwitcher = FindObjectOfType<PlayerSwitcher>();

        // 如果没有找到AudioSource组件，自动添加一个
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        // 设置AudioSource的基本属性
        audioSource.loop = true; // 循环播放
        audioSource.playOnAwake = false; // 不自动播放
        audioSource.volume = 0f; // 初始音量为0

        // 初始化音频
        InitializeAudio();
    }

    void InitializeAudio()
    {
        // 如果音频文件没有在Inspector中设置，尝试从Resources文件夹加载
        if (cabinEngineSound == null)
        {
            cabinEngineSound = Resources.Load<AudioClip>("Audio/舱内引擎声");
        }
        if (externalEngineSound == null)
        {
            externalEngineSound = Resources.Load<AudioClip>("Audio/舱外引擎声");
        }

        // 设置初始音频
        SetEngineSound();
        isAudioInitialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        // 检查 DataCenter 是否初始化
        if (DataCenter.Instance == null)
        {
            Debug.LogWarning("DataCenter未初始化！");
            return;
        }

        // 获取 ThrottleLever 值并限制其范围为 [0, 1]
        float throttleLever = Mathf.Clamp01(DataCenter.Instance.throttleLever1);

        // 将 ThrottleLever 映射到旋转速度（0 对应 0 度/秒，1 对应 maxRotationSpeed 度/秒）
        float rotationSpeed = throttleLever * maxRotationSpeed;

        // 计算当前帧的旋转角度（基于 Time.deltaTime 确保帧率无关）
        float rotationAngle = rotationSpeed * Time.deltaTime;

        // 绕 Z 轴旋转
        transform.Rotate(0, 0, rotationAngle, Space.Self);

        // 处理音频播放
        HandleAudioPlayback(throttleLever);
    }

    void HandleAudioPlayback(float throttleLever)
    {
        if (!isAudioInitialized || audioSource == null)
            return;

        // 检查是否需要切换音频
        SetEngineSound();

        // 获取引擎转速数据
        float engineRPM;
        if (useAverageRPM)
        {
            // 使用左右引擎平均转速
            engineRPM = (DataCenter.Instance.N1_Left + DataCenter.Instance.N1_Right) / 2f;
        }
        else
        {
            // 使用左引擎转速
            engineRPM = DataCenter.Instance.N1_Left;
        }

        // 将引擎转速映射到音量（N1转速通常在0-100之间）
        // 假设N1转速0-100对应音量0-1
        float normalizedRPM = Mathf.Clamp01(engineRPM / 100f);
        float targetVolume = Mathf.Lerp(minVolume, maxVolume, normalizedRPM);

        // 平滑过渡音量
        audioSource.volume = Mathf.Lerp(audioSource.volume, targetVolume, volumeChangeSpeed * Time.deltaTime);

        // 如果引擎转速大于阈值且音频没有播放，开始播放
        if (engineRPM > engineRPMThreshold && !audioSource.isPlaying)
        {
            audioSource.Play();
            Debug.Log($"开始播放引擎声音，当前转速: {engineRPM:F1}%");
        }
        // 如果引擎转速低于阈值且音频正在播放，停止播放
        else if (engineRPM <= engineRPMThreshold && audioSource.isPlaying)
        {
            audioSource.Stop();
            Debug.Log($"停止播放引擎声音，当前转速: {engineRPM:F1}%");
        }
    }

    void SetEngineSound()
    {
        if (playerSwitcher == null)
            return;

        // 获取当前状态索引
        int currentStateIndex = GetCurrentStateIndex();

        // 根据状态选择音频
        AudioClip targetSound = (currentStateIndex == 0) ? cabinEngineSound : externalEngineSound;

        // 如果音频需要切换
        if (audioSource.clip != targetSound && targetSound != null)
        {
            bool wasPlaying = audioSource.isPlaying;
            float currentVolume = audioSource.volume;

            // 切换音频
            audioSource.clip = targetSound;
            currentEngineSound = targetSound;

            // 如果之前在播放，继续播放
            if (wasPlaying)
            {
                audioSource.Play();
                audioSource.volume = currentVolume;
            }

            Debug.Log($"切换到引擎声音: {(currentStateIndex == 0 ? "舱内引擎声" : "舱外引擎声")}");
        }
    }

    int GetCurrentStateIndex()
    {
        // 使用公共方法获取当前状态索引
        if (playerSwitcher != null)
        {
            return playerSwitcher.GetCurrentStateIndex();
        }

        // 如果无法获取，默认返回0（舱内状态）
        return 0;
    }

    // 公共方法：手动设置音频文件
    public void SetAudioClips(AudioClip cabin, AudioClip external)
    {
        cabinEngineSound = cabin;
        externalEngineSound = external;
        isAudioInitialized = true;
    }

    // 公共方法：手动切换音频
    public void SwitchToCabinSound()
    {
        if (audioSource != null && cabinEngineSound != null)
        {
            audioSource.clip = cabinEngineSound;
            currentEngineSound = cabinEngineSound;
        }
    }

    public void SwitchToExternalSound()
    {
        if (audioSource != null && externalEngineSound != null)
        {
            audioSource.clip = externalEngineSound;
            currentEngineSound = externalEngineSound;
        }
    }
}
