// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class startgame : MonoBehaviour
// {
//     public Canvas mycanvas;
//     public Button startGame;
//     public GameObject obj;
//     public Animator gameStartAnimation;
//     public float currentTime = 0f;
//     public float timerDuration = 60f;
//     public int begin = 0;
//     // Start is called before the first frame update
//     void Start()
//     {
//         gameStartAnimation = obj.GetComponent<Animator>();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         startGame.onClick.AddListener(() =>
//         {
//             mycanvas.gameObject.SetActive(false);
//             gameStartAnimation.SetBool("gameStart", true);

//             begin = 1;
//         });
//         if (begin == 1)
//         {
//             currentTime += Time.deltaTime;
//         }
//         if (currentTime >= timerDuration)
//         {
//             obj.GetComponent<Animator>().enabled = false;
//             begin = 0;
//         }
//     }
// }


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class startgame : MonoBehaviour
{
    [Header("UI 和 动画对象")]
    public Canvas mycanvas;
    public Button startGameButton;
    public GameObject obj;
    private Animator gameStartAnimation;

    [Header("计时器设置")]
    public float timerDuration = 60f;
    private float currentTime = 0f;
    private bool isRunning = false;

    [Header("数据接收脚本")]
    // 在 Inspector 中将那个脚本组件（继承 MonoBehaviour 的组件）拖到这里
    public MonoBehaviour dataReceiverScript;

    void Start()
    {
        // 获取 Animator
        gameStartAnimation = obj.GetComponent<Animator>();

        // 确保数据接收脚本初始为禁用
        if (dataReceiverScript != null)
            dataReceiverScript.enabled = false;

        // 只添加一次点击事件监听
        startGameButton.onClick.RemoveAllListeners();
        startGameButton.onClick.AddListener(OnStartGame);
    }

    void OnStartGame()
    {
        // 隐藏 UI
        mycanvas.gameObject.SetActive(false);
        // 播放开始动画
        gameStartAnimation.SetBool("gameStart", true);
        // 重置计时
        currentTime = 0f;
        isRunning = true;
    }

    void Update()
    {
        if (!isRunning)
            return;

        // 增加计时
        currentTime += Time.deltaTime;

        if (currentTime >= timerDuration)
        {
            // 停止动画
            gameStartAnimation.enabled = false;

            // 激活数据接收脚本
            if (dataReceiverScript != null)
                dataReceiverScript.enabled = true;

            // 停止计时
            isRunning = false;
        }
    }
}
