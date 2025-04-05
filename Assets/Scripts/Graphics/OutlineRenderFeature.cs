using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OutlineRenderFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class OutlineSettings
    {
        [Tooltip("后处理材质，使用你写好的 Outline Shader（例如 Custom/TilemapOutlineWithMask）")]
        public Material outlineMaterial = null;
        [Tooltip("Tilemap 摄像机渲染的 RenderTexture（确保格式支持透明）")]
        public RenderTexture tilemapRenderTexture = null;
        [Tooltip("Render Pass 事件，建议设置在 AfterRenderingPostProcessing")]
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
    }

    public OutlineSettings settings = new OutlineSettings();

    class OutlineRenderPass : ScriptableRenderPass
    {
        public Material outlineMaterial = null;
        public RenderTexture tilemapRenderTexture = null;
        private RenderTargetHandle temporaryColorTexture;

        public OutlineRenderPass()
        {
            temporaryColorTexture.Init("_TemporaryColorTexture");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (outlineMaterial == null)
                return;

            CommandBuffer cmd = CommandBufferPool.Get("Outline Render Pass");

            // 将 Tilemap 的 RenderTexture 传递给 Outline 材质
            if (tilemapRenderTexture != null)
            {
                outlineMaterial.SetTexture("_MainTex", tilemapRenderTexture);
            }

            // 在 Execute 中获取摄像机的颜色目标
            RenderTargetIdentifier source = renderingData.cameraData.renderer.cameraColorTarget;
            
            // // 创建一个临时 RT，用于保存当前画面
            // RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            // opaqueDesc.depthBufferBits = 0;
            //
            // cmd.GetTemporaryRT(temporaryColorTexture.id, opaqueDesc, FilterMode.Bilinear);
            // // 拷贝当前摄像机画面到临时 RT
            // cmd.Blit(source, temporaryColorTexture.Identifier());
            // // 使用 Outline 材质处理后，再写回到摄像机目标
            // cmd.Blit(temporaryColorTexture.Identifier(), source, outlineMaterial);
            cmd.Blit(tilemapRenderTexture, source, outlineMaterial);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    OutlineRenderPass m_ScriptablePass;

    public override void Create()
    {
        m_ScriptablePass = new OutlineRenderPass();
        m_ScriptablePass.outlineMaterial = settings.outlineMaterial;
        m_ScriptablePass.tilemapRenderTexture = settings.tilemapRenderTexture;
        m_ScriptablePass.renderPassEvent = settings.renderPassEvent;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (renderingData.cameraData.camera.CompareTag("outliner"))
            return;
        
        if (settings.outlineMaterial == null || settings.tilemapRenderTexture == null)
            return;
        
        // 不要在此处调用 cameraColorTarget，直接入队即可
        renderer.EnqueuePass(m_ScriptablePass);
    }
}