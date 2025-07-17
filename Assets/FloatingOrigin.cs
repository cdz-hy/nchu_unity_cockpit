using UnityEngine;

public class FloatingOrigin : MonoBehaviour
{
    [Header("世界根：将地形、建筑、飞机、特效等都挂到这里")]
    public Transform worldRoot;

    [Header("参考对象：距离原点超限时触发漂移")]
    public Transform reference;

    [Header("当参考对象距离原点超过此值（米）时，触发一次漂移")]
    public float threshold = 500f;

    void Update()
    {
        if (worldRoot == null || reference == null)
            return;

        // 只在水平面上判断距离，也可以用 reference.position.magnitude
        Vector2 flatPos = new Vector2(reference.position.x, reference.position.z);

        if (flatPos.magnitude > threshold)
        {
            // 计算偏移量：将参考对象拉回到原点
            Vector3 offset = reference.position;

            // 整体平移 worldRoot
            worldRoot.position -= offset;

            // 如果 reference 本身是 worldRoot 的子物体，则它的位置会自动被偏移回来
            // 如果 reference 不在 worldRoot 下，则需要手动平移它：
            // reference.position -= offset;
        }
    }
}
