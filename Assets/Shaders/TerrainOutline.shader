Shader "Custom/TilemapOutlineWithMask"
{
    Properties
    {
        _MainTex("Tilemap Render Texture", 2D) = "white" {}
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _OutlineThickness("Outline Thickness", Range(0,10)) = 1
        _Threshold("Threshold", Range(0,1)) = 0.1
        _MaskCenter("Mask Center (UV)", Vector) = (0.5, 0.5, 0, 0)
        _MaskRadius("Mask Radius", Range(0,1)) = 0.5
        _MaskSoftness("Mask Softness", Range(0,0.5)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Overlay" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            
            float4 _OutlineColor;
            float _OutlineThickness;
            float _Threshold;
            float4 _MaskCenter; // 只使用 xy 分量，归一化屏幕坐标（0~1）
            float _MaskRadius;
            float _MaskSoftness;

            fixed4 frag(v2f_img i) : SV_Target
            {
                float2 uv = i.uv;
                // 采样中心 alpha 值
                float aCenter = tex2D(_MainTex, uv).a;
                
                // 根据 _OutlineThickness 计算 UV 偏移（乘上每个像素的 UV 大小）
                float2 offset = _OutlineThickness * _MainTex_TexelSize.xy;
                
                // 采样上下左右的 alpha 值
                float aUp    = tex2D(_MainTex, uv + float2(0, offset.y)).a;
                float aDown  = tex2D(_MainTex, uv - float2(0, offset.y)).a;
                float aLeft  = tex2D(_MainTex, uv - float2(offset.x, 0)).a;
                float aRight = tex2D(_MainTex, uv + float2(offset.x, 0)).a;

                // 计算各方向的差值（只考虑正向差异）
                float diffUp    = saturate(aUp    - aCenter);
                float diffDown  = saturate(aDown  - aCenter);
                float diffLeft  = saturate(aLeft  - aCenter);
                float diffRight = saturate(aRight - aCenter);

                // 取各方向中的最大差值
                float outlineFactor = max(max(diffUp, diffDown), max(diffLeft, diffRight));
                // 如果超过阈值，则认为是轮廓（step 返回 0或1）
                outlineFactor = step(_Threshold, outlineFactor);

                // 计算遮罩：以 _MaskCenter 为中心的圆形区域，内部为1，边缘平滑过渡到0,可以用贴图自定遮罩形状，
                float d = distance(uv, _MaskCenter.xy);
                float mask = 1.0 - smoothstep(_MaskRadius - _MaskSoftness, _MaskRadius, d);

                // 最终描边颜色（只在轮廓位置显示）乘上遮罩
                float4 outline = _OutlineColor * outlineFactor * mask;
                float4 c = tex2D(_MainTex, uv);
                return outline;
                // return float4(0,0,0,1);
            }
            ENDCG
        }
    }
    FallBack "Hidden/InternalErrorShader"
}