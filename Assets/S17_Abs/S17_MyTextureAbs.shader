Shader "CG/S17_MyTextureAbs"
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
                // input.uv 대신 uv를 직접 사용하도록 수정
                if (uv.x <= 0.5 && uv.y >= 0.5)
                {
                    float2 newUV = float2(uv.x + 0.5, uv.y - 0.5);
        
                    // sampler_BaseMap을 지우고 2개의 인자만 전달하도록 수정
                    return tex2D(_BaseMap, newUV);
                }
                else
                {
                    return half4(0.5, 0.5, 0.5, 1.0);
                }
            }
            ENDHLSL
        }
    }
}