using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class SpeechScript : MonoBehaviour
{
    //������д�Լ��İٶ�����ʶ�����key
    public string api_key = "fDS1lKZPgCHkaistOz8o72yB";
    public string secret_Key = "KwApTEf0yGm5VOHHzF5dugBuZhvM6Qo7";
    string accessToken = string.Empty;

    bool ishaveMic = false; //����Ƿ�������˷�
    string currentDeviceName = string.Empty; //��ǰ¼���豸����
    //string currentDeviceName = "a1"; //��ǰ¼���豸����
    int recordFrequency = 8000; //¼��Ƶ��
    int recordMaxTime = 20;//���¼��ʱ��
    AudioClip saveAudioClip;//�洢��ǰ¼����Ƭ��
    AudioSource source;
    public LongPressButton longPressButton;
    public VoiceCommand voiceCommand;

    string resulrStr;//�洢ʶ����
    Text resultText;//��ʾʶ����
    Button startBtn;//��ʼʶ��ť

    void Start()
    {
        //saveAudioClip = this.transform.GetComponent<AudioClip>();
        source = this.transform.GetComponent<AudioSource>();
        //resultText = GameObject.Find("Canvas/panel/speech").GetComponent<Text>();


        StartCoroutine(_GetAccessToken());//��ȡaccessToken

        longPressButton.OnButtonPressed += HandleButtonPressed;
        longPressButton.OnButtonReleased += HandleButtonReleased;
    }


    /// <summary>
    /// ��ʼ¼��
    /// </summary>
    void HandleButtonPressed()
    {
        saveAudioClip = Microphone.Start(currentDeviceName, false, recordMaxTime, recordFrequency);
        Debug.Log("��ʼ¼��");
    }

    /// <summary>
    /// ����¼��
    /// </summary>
    void HandleButtonReleased()
    {
        Microphone.End(currentDeviceName);
        Debug.Log("����");
        source.PlayOneShot(saveAudioClip);//��������
        Debug.Log("����¼��");
        StartCoroutine(RequestASR());//��������ʶ��
    }

    /// <summary>
    /// ��������ʶ��
    /// </summary>
    /// <returns></returns>
    IEnumerator RequestASR()
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            yield return _GetAccessToken();
        }
        resulrStr = string.Empty;

        //����ǰ¼������ΪPCM16
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
                resulrStr = "ʶ����Ϊ��";
            }
            Debug.Log("ʶ���ı�Ϊ:" + resulrStr);
            voiceCommand.ExecuteCommand(resulrStr);
        }
    }

    /// <summary>
    /// ��ȡaccessToken��������
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
                Debug.Log("Token�Ѿ�ƥ��");
                accessToken = match.Groups[1].ToString();
            }
            else
            {
                Debug.Log("��֤����,��ȡAccessTokenʧ��!!!");
            }
        }
    }
}


