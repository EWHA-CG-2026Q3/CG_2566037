Shader "CG/S17_MyTextureAbs2"
{
    Properties
    {
        _BaseMap ("Base Map", 2D) = "white" {}
    }
    SubShader
    {
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            sampler2D _BaseMap;

            void vert(float3 positionOS : POSITION,
                      float2 uv : TEXCOORD0,
                      out float2 uvOut : TEXCOORD0,
                      out float4 positionCS : SV_POSITION)
            {
                uvOut = uv;
                positionCS = TransformObjectToHClip(positionOS);
            }

            float4 frag(float2 uv : TEXCOORD0) : SV_Target
            {
                // 4x4 텍스처를 1/4 크기로 축소하고, D3 위치(X: 0.5, Y: 0.0)로 이동
                float2 newUV = uv * 0.25 + float2(0.5, 0.0);
                
                return tex2D(_BaseMap, newUV);
            }
            ENDHLSL
        }
    }
}