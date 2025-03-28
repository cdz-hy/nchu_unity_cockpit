using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class SpeechScript : MonoBehaviour
{
    //自行填写自己的百度语音识别相关key
    public string api_key = "fDS1lKZPgCHkaistOz8o72yB";
    public string secret_Key = "KwApTEf0yGm5VOHHzF5dugBuZhvM6Qo7";
    string accessToken = string.Empty;

    bool ishaveMic = false; //检测是否连接麦克风
    string currentDeviceName = string.Empty; //当前录音设备名称
    //string currentDeviceName = "a1"; //当前录音设备名称
    int recordFrequency = 8000; //录音频率
    int recordMaxTime = 20;//最大录音时长
    AudioClip saveAudioClip;//存储当前录音的片段
    AudioSource source;
    public LongPressButton longPressButton;
    public VoiceCommand voiceCommand;

    string resulrStr;//存储识别结果
    Text resultText;//显示识别结果
    Button startBtn;//开始识别按钮

    void Start()
    {
        //saveAudioClip = this.transform.GetComponent<AudioClip>();
        source = this.transform.GetComponent<AudioSource>();
        //resultText = GameObject.Find("Canvas/panel/speech").GetComponent<Text>();


        StartCoroutine(_GetAccessToken());//获取accessToken

        longPressButton.OnButtonPressed += HandleButtonPressed;
        longPressButton.OnButtonReleased += HandleButtonReleased;
    }


    /// <summary>
    /// 开始录音
    /// </summary>
    void HandleButtonPressed()
    {
        saveAudioClip = Microphone.Start(currentDeviceName, false, recordMaxTime, recordFrequency);
        Debug.Log("开始录音");
    }

    /// <summary>
    /// 结束录音
    /// </summary>
    void HandleButtonReleased()
    {
        Microphone.End(currentDeviceName);
        Debug.Log("……");
        source.PlayOneShot(saveAudioClip);//播放语音
        Debug.Log("结束录音");
        StartCoroutine(RequestASR());//请求语音识别
    }

    /// <summary>
    /// 请求语音识别
    /// </summary>
    /// <returns></returns>
    IEnumerator RequestASR()
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            yield return _GetAccessToken();
        }
        resulrStr = string.Empty;

        //处理当前录音数据为PCM16
        float[] samples = new float[recordFrequency * 10 * saveAudioClip.channels];
        saveAudioClip.GetData(samples, 0);
        var samplesShort = new short[samples.Length];
        for (var index = 0; index < samples.Length; index++)
        {
            samplesShort[index] = (short)(samples[index] * short.MaxValue);
        }
        byte[] datas = new byte[samplesShort.Length * 2];
        Buffer.BlockCopy(samplesShort, 0, datas, 0, datas.Length);

        string url = string.Format("{0}?cuid={1}&token={2}", "https://vop.baidu.com/server_api", SystemInfo.deviceUniqueIdentifier, accessToken);

        WWWForm wwwForm = new WWWForm();
        wwwForm.AddBinaryData("audio", datas);

        UnityWebRequest unityWebRequest = UnityWebRequest.Post(url, wwwForm);

        unityWebRequest.SetRequestHeader("Content-Type", "audio/pcm;rate=" + recordFrequency);

        yield return unityWebRequest.SendWebRequest();

        if (string.IsNullOrEmpty(unityWebRequest.error))
        {
            resulrStr = unityWebRequest.downloadHandler.text;
            if (Regex.IsMatch(resulrStr, @"err_msg.:.success"))
            {
                Match match = Regex.Match(resulrStr, "result.:..(.*?)..]");
                if (match.Success)
                {
                    resulrStr = match.Groups[1].ToString();
                }
            }
            else
            {
                resulrStr = "识别结果为空";
            }
            Debug.Log("识别文本为:" + resulrStr);
            voiceCommand.ExecuteCommand(resulrStr);
        }
    }

    /// <summary>
    /// 获取accessToken请求令牌
    /// </summary>
    /// <returns></returns>
    IEnumerator _GetAccessToken()
    {
        var uri =
            string.Format(
                "https://openapi.baidu.com/oauth/2.0/token?grant_type=client_credentials&client_id={0}&client_secret={1}",
                api_key, secret_Key);
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(uri);
        yield return unityWebRequest.SendWebRequest();
        if (unityWebRequest.isDone)
        {
            Match match = Regex.Match(unityWebRequest.downloadHandler.text, @"access_token.:.(.*?).,");
            if (match.Success)
            {
                Debug.Log("Token已经匹配");
                accessToken = match.Groups[1].ToString();
            }
            else
            {
                Debug.Log("验证错误,获取AccessToken失败!!!");
            }
        }
    }
}


