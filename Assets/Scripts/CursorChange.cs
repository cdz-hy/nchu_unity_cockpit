using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CursorChange : MonoBehaviour
{
    public bool isMouseOver = false;
    public Texture2D handCursor;    // 如果你要使用自定义光标
    public Texture2D clickCursor;   // 点击时的光标
    public Texture2D defaultCursor; // 默认光标

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 当鼠标进入BoxCollider区域时，改变鼠标光标为手形
    void OnMouseEnter()
    {
        // 如果鼠标进入BoxCollider区域，且没有按下左键，显示手形光标
        if (!Input.GetMouseButton(0))
        {
            Cursor.SetCursor(handCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    // 当鼠标离开BoxCollider区域时，恢复默认光标
    void OnMouseExit()
    {
        // 离开BoxCollider区域时，如果没有按下左键，恢复默认光标
        if (!Input.GetMouseButton(0))
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        }
    }
}
