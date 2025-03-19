Shader "Unlit/CoolShader"
{
    Properties
    {
        _VolumeTex("Noise Volume", 3D) = "white" {}
        _Zoom("Zoom", Float) = 1.0
        _Speed("Speed", Float) = 10.0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
            sampler3D _VolumeTex;
            float _Zoom;
            float _Speed;
            CBUFFER_END

            Varyings vert (Attributes IN)
            {
                Varyings OUT;

                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;

                return OUT;
            }

            float mod(float x, float y)
            {
                return x - y * floor(x/y);
            }

            half4 frag (Varyings i) : SV_Target
            {
                //const float SPEED = 10.0;
                const float DETAIL = 0.1;
                const int ITERATIONS = 1200;

                float3 pooper = float3(_ScreenParams.x, _ScreenParams.y, 1);
                float3 wobble = cos(_Time.y + pooper);
                float3 position =  float3(mod(_Time.y / (1.0/_Speed), 900.0)-300.0, 0, 0) + wobble;
                float3 direction = float3(_ScreenParams.x * _Zoom, _ScreenParams.xy - round(i.positionHCS) * 2.0);
                
                int depth = _ScreenParams.x /_ScreenParams.y;
                [loop]
                while (depth++ < ITERATIONS) {
                    float3 step = mod(-position, sign(direction)) / (direction) + 3e-8;
                    float3 minStep = min(step, step);
                    position += minStep.x * direction;
                    float3 samplePos = ceil(position) / 300.0;
                    float noiseValue = tex3D(_VolumeTex, samplePos).g;
                    if (noiseValue <= length(samplePos.yz) / DETAIL) {
                        break;
                    }
                }
                
                return float4(fwidth(position), 1);
            }
            ENDHLSL
        }
    }
}
