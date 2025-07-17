using UnityEngine;

/// <summary>
/// 为灯光添加深度写入功能，解决穿模问题
/// </summary>
public class LightDepthHelper : MonoBehaviour
{
    [Header("光晕设置")]
    public bool enableHalo = true;             // 是否启用光晕
    public float haloSize = 0.5f;              // 光晕大小
    public Material haloMaterial;              // 光晕材质
    public Texture2D haloTexture;              // 光晕贴图
    public Color haloColor = Color.white;      // 光晕颜色
    public float haloIntensity = 1.0f;         // 光晕强度
    
    [Header("深度设置")]
    public bool useDepthWrite = true;          // 是否使用深度写入
    public float depthBias = 0.01f;            // 深度偏移值
    
    [Header("高级设置")]
    public bool alwaysFaceCamera = true;       // 是否始终面向摄像机
    public float updateInterval = 0.1f;        // 更新间隔(秒)
    
    private Light lightComponent;              // 灯光组件
    private GameObject haloObject;             // 光晕对象
    private MeshRenderer haloRenderer;         // 光晕渲染器
    private Material instancedMaterial;        // 实例化的材质
    private float lastUpdateTime;              // 上次更新时间
    
    void Start()
    {
        // 获取灯光组件
        lightComponent = GetComponent<Light>();
        
        // 如果启用光晕，创建光晕对象
        if (enableHalo)
        {
            CreateHaloObject();
        }
    }
    
    void Update()
    {
        // 按照指定间隔更新
        if (Time.time - lastUpdateTime >= updateInterval)
        {
            UpdateHalo();
            lastUpdateTime = Time.time;
        }
        
        // 如果需要始终面向摄像机
        if (alwaysFaceCamera && haloObject != null && Camera.main != null)
        {
            haloObject.transform.LookAt(Camera.main.transform.position, Vector3.up);
        }
    }
    
    /// <summary>
    /// 创建光晕对象
    /// </summary>
    void CreateHaloObject()
    {
        // 如果没有提供材质，尝试加载默认材质
        if (haloMaterial == null)
        {
            haloMaterial = Resources.Load<Material>("Shaders/LightWithDepth");
            if (haloMaterial == null)
            {
                Debug.LogError("未找到光晕材质，请确保已创建 Resources/Shaders/LightWithDepth 材质");
                return;
            }
        }
        
        // 创建实例化材质
        instancedMaterial = new Material(haloMaterial);
        
        // 设置材质属性
        instancedMaterial.SetColor("_Color", haloColor);
        instancedMaterial.SetFloat("_Intensity", haloIntensity);
        instancedMaterial.SetFloat("_UseDepthWrite", useDepthWrite ? 1.0f : 0.0f);
        instancedMaterial.SetFloat("_DepthBias", depthBias);
        
        // 如果提供了贴图，应用贴图
        if (haloTexture != null)
        {
            instancedMaterial.SetTexture("_MainTex", haloTexture);
        }
        
        // 创建光晕对象
        haloObject = new GameObject("LightHalo");
        haloObject.transform.parent = transform;
        haloObject.transform.localPosition = Vector3.zero;
        haloObject.transform.localRotation = Quaternion.identity;
        
        // 添加MeshFilter和MeshRenderer
        MeshFilter meshFilter = haloObject.AddComponent<MeshFilter>();
        haloRenderer = haloObject.AddComponent<MeshRenderer>();
        
        // 创建一个四边形网格
        Mesh mesh = new Mesh();
        float size = haloSize * 0.5f;
        
        // 顶点
        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(-size, -size, 0),
            new Vector3(size, -size, 0),
            new Vector3(-size, size, 0),
            new Vector3(size, size, 0)
        };
        
        // 三角形
        int[] triangles = new int[6]
        {
            0, 2, 1,
            2, 3, 1
        };
        
        // UV坐标
        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        
        // 颜色
        Color[] colors = new Color[4]
        {
            new Color(1, 1, 1, 1),
            new Color(1, 1, 1, 1),
            new Color(1, 1, 1, 1),
            new Color(1, 1, 1, 1)
        };
        
        // 设置网格数据
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.colors = colors;
        mesh.RecalculateNormals();
        
        // 应用网格和材质
        meshFilter.mesh = mesh;
        haloRenderer.material = instancedMaterial;
        
        // 同步光源颜色和强度
        UpdateHalo();
    }
    
    /// <summary>
    /// 更新光晕属性
    /// </summary>
    void UpdateHalo()
    {
        if (haloObject != null && haloRenderer != null && lightComponent != null)
        {
            // 同步光源颜色
            Color lightColor = lightComponent.color;
            lightColor.a = lightComponent.intensity / 2.0f; // 透明度基于光源强度
            
            // 更新材质颜色
            if (instancedMaterial != null)
            {
                instancedMaterial.SetColor("_Color", lightColor);
            }
            
            // 启用或禁用光晕
            haloRenderer.enabled = lightComponent.enabled && enableHalo;
            
            // 更新深度写入设置
            if (instancedMaterial != null)
            {
                instancedMaterial.SetFloat("_UseDepthWrite", useDepthWrite ? 1.0f : 0.0f);
                instancedMaterial.SetFloat("_DepthBias", depthBias);
            }
        }
    }
    
    /// <summary>
    /// 设置光晕大小
    /// </summary>
    public void SetHaloSize(float size)
    {
        haloSize = size;
        if (haloObject != null)
        {
            haloObject.transform.localScale = new Vector3(size, size, size);
        }
    }
    
    /// <summary>
    /// 设置光晕颜色
    /// </summary>
    public void SetHaloColor(Color color)
    {
        haloColor = color;
        if (instancedMaterial != null)
        {
            instancedMaterial.SetColor("_Color", color);
        }
    }
    
    /// <summary>
    /// 设置光晕强度
    /// </summary>
    public void SetHaloIntensity(float intensity)
    {
        haloIntensity = intensity;
        if (instancedMaterial != null)
        {
            instancedMaterial.SetFloat("_Intensity", intensity);
        }
    }
    
    /// <summary>
    /// 启用或禁用深度写入
    /// </summary>
    public void SetDepthWrite(bool enable)
    {
        useDepthWrite = enable;
        if (instancedMaterial != null)
        {
            instancedMaterial.SetFloat("_UseDepthWrite", enable ? 1.0f : 0.0f);
        }
    }
}