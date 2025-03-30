using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear_1 : MonoBehaviour
{
    public int isKeyPressed = 1;
    public Gear_2 gear_2;
    public RetractFrontWheels retractFrontWheels;
    public LeftBoard leftBoard;
    public RightBoard rightBoard;
    public FrontWheel_2 frontWheel_2;
    public LeftWheel_1 leftWheel_1;
    public LeftWheel_2 leftWheel_2;
    public LeftWheel_3 leftWheel_3;
    public LeftWheel_4 leftWheel_4;
    public RightWheel_1 rightWheel_1;
    public RightWheel_2 rightWheel_2;
    public RightWheel_3 rightWheel_3;
    public RightWheel_4 rightWheel_4;

    public Texture2D handCursor;    // 自定义手型光标  
    public Texture2D clickCursor;   // 点击时的光标  
    public Texture2D defaultCursor; // 默认光标  

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (isKeyPressed == 2)
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(40, 0, 0), 0.1f);
        if (isKeyPressed == 1)
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, 0), 0.1f);
    }

    void OnMouseDown()
    {
        isKeyPressed = 3 - isKeyPressed;
        gear_2.isKeyPressed = isKeyPressed;
        retractFrontWheels.isKeyPressed = isKeyPressed;
        frontWheel_2.isKeyPressed = isKeyPressed;
        leftBoard.isKeyPressed = isKeyPressed;
        rightBoard.isKeyPressed = isKeyPressed;
        leftWheel_1.isKeyPressed = isKeyPressed;
        leftWheel_2.isKeyPressed = isKeyPressed;
        leftWheel_3.isKeyPressed = isKeyPressed;
        leftWheel_4.isKeyPressed = isKeyPressed;
        rightWheel_1.isKeyPressed = isKeyPressed;
        rightWheel_2.isKeyPressed = isKeyPressed;
        rightWheel_3.isKeyPressed = isKeyPressed;
        rightWheel_4.isKeyPressed = isKeyPressed;

        Cursor.SetCursor(clickCursor, Vector2.zero, CursorMode.Auto);  // 使用点击时的光标
    }

    void OnMouseUp()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }

    public void VoiceControll()
    {
        OnMouseDown();
    }

    public void Up()
    {
        isKeyPressed = 1;
        OnMouseDown();
    }

    public void Down()
    {
        isKeyPressed = 2;
        OnMouseDown();
    }
}
