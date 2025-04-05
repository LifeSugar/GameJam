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
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = IN.uv;
                return OUT;
            }

            float4 frag(Varyings IN) : SV_TARGET
            {
                float2 uv = IN.uv;
                float2 center = _PlayerPos.xy;

                float dist = distance(uv, center);
                float alpha = clamp((dist - _Range) / _Softness, 0, _AlphaOffset);

                // 用 URP 自带的采样宏
                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
                color = float4 (color.x * _ColorTint.x, color.y * _ColorTint.y, color.z * _ColorTint.z, 1);

                return float4(color.x, color.y, color.z, alpha);
            }
            ENDHLSL
        }
    }
}