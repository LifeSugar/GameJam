using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Renderer))]
public class MaskFogController : MonoBehaviour
{
    public Transform player;      // 玩家或光源
    public float range = 0.2f;
    public float softness = 0.1f;
    public float alphaOffset = 0.68f;
    
    private Material mat;

    void Start()
    {
        // 获取此对象材质（Assume: using the "Custom/2DMaskFog" shader）
        mat = GetComponent<Renderer>().material;
        ChangeFogMask(0f, 0.14f, 1f, 1f);
    }

    void Update()
    {
        if (player == null || Camera.main == null) return;

        // 将玩家世界坐标转换成 屏幕空间(0..1, 0..1)
        Vector3 screenPos = Camera.main.WorldToViewportPoint(player.position);

        // 将坐标传给材质
        mat.SetVector("_PlayerPos", new Vector4(screenPos.x, screenPos.y, 0, 0));

        // // 传其他属性
        // mat.SetFloat("_Range", range);
        // mat.SetFloat("_Softness", softness);
        // mat.SetFloat("_AlphaOffset", alphaOffset);
    }
    
    public static MaskFogController instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogError("there're more than one instance of MaskFogController");
        }
        instance = this;
    }

    //这个方法用来平滑改变
    public void ChangeFogMask(float targetRange, float targetSoftness, float targetAlphaOffset, float duration)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Join(
            DOTween.To(() => this.range,
                x =>
                {
                    this.range = x;
                    mat.SetFloat("_Range", this.range);
                }, targetRange, duration
            )
        );
        sequence.Join(
            DOTween.To(() => this.softness,
                x =>
                {
                    this.softness = x;
                    mat.SetFloat("_Softness", this.softness);
                }, targetSoftness, duration
            )
        );
        sequence.Join(
            DOTween.To(() => this.alphaOffset,
                x =>
                {
                    this.alphaOffset = x; 
                    mat.SetFloat("_AlphaOffset", this.alphaOffset);
                }, targetAlphaOffset, duration
            )
        );
        
    }
}