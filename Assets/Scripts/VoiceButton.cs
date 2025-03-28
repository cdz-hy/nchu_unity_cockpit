using UnityEngine;
using UnityEngine.EventSystems;

public class LongPressButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // �����¼�
    public delegate void ButtonAction();
    public event ButtonAction OnButtonPressed;   // �����¼�
    public event ButtonAction OnButtonReleased;   // �ɿ��¼�

    public void OnPointerDown(PointerEventData eventData)
    {
        OnButtonPressed?.Invoke(); // ������ť�����¼�
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnButtonReleased?.Invoke(); // ������ť�ɿ��¼�
    }
}