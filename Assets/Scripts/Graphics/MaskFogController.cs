using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MaskFogController : MonoBehaviour
{
    public Transform player;      // 玩家或光源
    public float range = 0.2f;
    public float softness = 0.1f;
    
    private Material mat;

    void Start()
    {
        // 获取此对象材质（Assume: using the "Custom/2DMaskFog" shader）
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        if (player == null || Camera.main == null) return;

        // 将玩家世界坐标转换成 屏幕空间(0..1, 0..1)
        Vector3 screenPos = Camera.main.WorldToViewportPoint(player.position);

        // 将坐标传给材质
        mat.SetVector("_PlayerPos", new Vector4(screenPos.x, screenPos.y, 0, 0));

        // 传其他属性
        mat.SetFloat("_Range", range);
        mat.SetFloat("_Softness", softness);
    }
}