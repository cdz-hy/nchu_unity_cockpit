using UnityEngine;
using UnityEngine.UI;

public class UIImageSwitcher : MonoBehaviour
{
    public Image targetImage;
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
        targetImage.sprite = sprite1;
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
        currentSpriteIndex = (currentSpriteIndex + 1) % 3;
        RectTransform rectTransform = targetImage.GetComponent<RectTransform>();
        
        switch (currentSpriteIndex)
        {
            case 0:
                targetImage.sprite = sprite1;
                rectTransform.localScale = Vector3.one; // 恢复原始大小
                break;
            case 1:
                targetImage.sprite = sprite2;
                rectTransform.localScale = new Vector3(0.89f, 0.89f, 1f); // 缩小到原来的89%
                break;
            case 2:
                targetImage.sprite = sprite3;
                rectTransform.localScale = Vector3.one; // 恢复原始大小
                break;
        }
    }

    // 添加新方法用于检查当前是否显示sprite1
    // 修改检查方法，使用currentSpriteIndex
    public bool IsShowingSprite1()
    {
        return currentSpriteIndex == 0;
    }
    
    public bool IsShowingSprite2()
    {
        return currentSpriteIndex == 1;
    }
    
    public bool IsShowingSprite3()
    {
        return currentSpriteIndex == 2;  // 修改这里，应该检查是否为2而不是0
    }
}
