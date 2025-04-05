using UnityEngine;

[ExecuteInEditMode]
public class OutlinePostProcessing : MonoBehaviour
{
    [Header("请将 Tilemap 摄像机渲染的 RenderTexture拖到此处")]
    public RenderTexture tilemapRenderTexture;

    [Header("Outline Material")]
    public Material outlineMaterial;

    void Start()
    {

        
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (outlineMaterial != null && tilemapRenderTexture != null)
        {
            // 将 Tilemap 的 RenderTexture 赋值给 Shader 的 _MainTex 属性
            outlineMaterial.SetTexture("_MainTex", tilemapRenderTexture);
            // 将主摄像机画面通过 outlineMaterial 进行处理后输出到 destination
            Graphics.Blit(source, destination, outlineMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}