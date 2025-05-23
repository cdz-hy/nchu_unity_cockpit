using UnityEngine;
using UnityEngine.UI;

public class UIImageSwitcher : MonoBehaviour
{
    public Image targetImage;
    public Sprite sprite0;    // 第零个图片
    public Sprite sprite1;    // 第一个图片
    public Sprite sprite2;    // 第二个图片
    public Sprite sprite3;    // 第三个图片
    

    private int currentSpriteIndex = 0; 

    void Start()
    {
        if (targetImage == null)
        {
            targetImage = GetComponent<Image>();
        }
        targetImage.sprite = sprite0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchSprite();
        }
    }

    void SwitchSprite()
    {
        currentSpriteIndex = (currentSpriteIndex + 1) % 4;
        RectTransform rectTransform = targetImage.GetComponent<RectTransform>();
        
        switch (currentSpriteIndex)
        {
            case 0:
                targetImage.sprite = sprite0;
                rectTransform.localScale = Vector3.one; // 恢复原始大小
                break;
            case 1:
                targetImage.sprite = sprite1;
                rectTransform.localScale = Vector3.one; // 恢复原始大小
                break;
            case 2:
                targetImage.sprite = sprite2;
                rectTransform.localScale = new Vector3(0.89f, 0.89f, 1f); // 缩小到原来的89%
                break;
            case 3:
                targetImage.sprite = sprite3;
                rectTransform.localScale = new Vector3(0.95f, 0.95f, 1f); // 恢复原始大小
                break;
        }
    }

    // 添加新方法用于检查当前是否显示sprite0
    public bool IsShowingSprite0()
    {
        return currentSpriteIndex == 0;
    }
    
    // 添加新方法用于检查当前是否显示sprite1
    public bool IsShowingSprite1()
    {
        return currentSpriteIndex == 1;
    }
    
    public bool IsShowingSprite2()
    {
        return currentSpriteIndex == 2;
    }
    
    public bool IsShowingSprite3()
    {
        return currentSpriteIndex == 3;  // 修改这里，应该检查是否为3而不是2
    }
    
}
