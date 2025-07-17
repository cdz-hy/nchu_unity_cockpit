using UnityEngine;
using UnityEngine.EventSystems;

public class LongPressButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // 定义事件
    public delegate void ButtonAction();
    public event ButtonAction OnButtonPressed;   // 按下事件
    public event ButtonAction OnButtonReleased;   // 松开事件

    public void OnPointerDown(PointerEventData eventData)
    {
        OnButtonPressed?.Invoke(); // 触发按钮按下事件
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnButtonReleased?.Invoke(); // 触发按钮松开事件
    }
}