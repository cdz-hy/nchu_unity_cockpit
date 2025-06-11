using UnityEngine;

/// <summary>
/// 在运行时生成默认的光晕贴图
/// </summary>
public class DefaultHalo : MonoBehaviour
{
    [Header("贴图设置")]
    public int textureSize = 256;              // 贴图大小
    public bool saveAsAsset = false;           // 是否保存为资源文件
    
    private Texture2D generatedTexture;        // 生成的贴图
    
    void Awake()
    {
        // 生成默认光晕贴图
        GenerateDefaultHaloTexture();
        
        // 如果需要，可以在这里添加代码将贴图保存为资源文件
    }
    
    /// <summary>
    /// 生成默认的光晕贴图
    /// </summary>
    void GenerateDefaultHaloTexture()
    {
        // 创建纹理
        generatedTexture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
        generatedTexture.name = "DefaultHalo";
        generatedTexture.wrapMode = TextureWrapMode.Clamp;
        generatedTexture.filterMode = FilterMode.Bilinear;
        
        // 生成径向渐变
        Color[] pixels = new Color[textureSize * textureSize];
        float center = textureSize / 2.0f;
        float maxDistance = center;
        
        for (int y = 0; y < textureSize; y++)
        {
            for (int x = 0; x < textureSize; x++)
            {
                // 计算到中心的距离
                float dx = x - center;
                float dy = y - center;
                float distance = Mathf.Sqrt(dx * dx + dy * dy);
                
                // 计算alpha值 (从中心向外渐变)
                float alpha = 1.0f - Mathf.Clamp01(distance / maxDistance);
                // 使用平方函数使边缘更加柔和
                alpha = alpha * alpha;
                
                // 设置像素颜色
                pixels[y * textureSize + x] = new Color(1, 1, 1, alpha);
            }
        }
        
        // 应用像素到纹理
        generatedTexture.SetPixels(pixels);
        generatedTexture.Apply();
        
        // 将生成的贴图保存到Resources文件夹
        if (saveAsAsset)
        {
            #if UNITY_EDITOR
            // 这段代码只在Unity编辑器中运行
            string path = "Assets/Resources/Textures/DefaultHalo.asset";
            UnityEditor.AssetDatabase.CreateAsset(generatedTexture, path);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            Debug.Log("光晕贴图已保存到: " + path);
            #endif
        }
    }
    
    /// <summary>
    /// 获取生成的光晕贴图
    /// </summary>
    public Texture2D GetGeneratedTexture()
    {
        return generatedTexture;
    }
} 