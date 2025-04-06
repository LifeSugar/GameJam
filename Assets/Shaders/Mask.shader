Shader "Custom/2DMaskFog"
{
    Properties
    {
        _PlayerPos("Player Screen Pos", Vector) = (0.5, 0.5, 0, 0)
        _Range("Range", Range(0,1)) = 0.2
        _Softness("Softness", Range(0,1)) = 0.1
        _MainTex("MainTex", 2D) = "white" {}
        _ColorTint("ColorTint", Color) = (1, 1, 1, 1)
        _AlphaOffset("AlphaOffset", Range(0,1)) = 0.0
        _MaskScale("Mask Scale (X,Y)", Vector) = (1, 1, 0, 0)

    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "PreviewMode" = "Plane"
        }

        LOD 100

        Pass
        {
            Name "FogMaskPass"
            Tags
            {
                "LightMode"="UniversalForward"
            }

            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            float4 _PlayerPos;
            float _Range;
            float _Softness;
            float4 _ColorTint;
            float _AlphaOffset;
            float4 _MaskScale;


            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 screenPos : TEXCOORD1; // 新增：屏幕空间坐标
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                float4 clipPos = TransformObjectToHClip(IN.positionOS);
                OUT.positionCS = clipPos;
                OUT.uv = IN.uv;

                // 将 clipPos.xyzw 转换为 NDC，再映射到 [0,1] 范围
                OUT.screenPos = (clipPos.xy / clipPos.w) * 0.5 + 0.5;

                return OUT;
            }

            float4 frag(Varyings IN) : SV_TARGET
            {



                float2 center = _PlayerPos.xy; // 假设传入的 _PlayerPos 已是 NDC [0,1] 区间
                float2 offset = (IN.screenPos - center) * _MaskScale.xy;
                float dist = length(offset);
                float alpha = clamp((dist - _Range) / _Softness, 0, _AlphaOffset);

                // float dist = distance(screenUV, center);
                // float alpha = clamp((dist - _Range) / _Softness, 0, _AlphaOffset);

                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                color = float4(color.rgb * _ColorTint.rgb, 1);

                return float4(color.rgb, alpha);
            }
            ENDHLSL
        }
    }
}