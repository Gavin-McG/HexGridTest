Shader "Custom/VoronoiShader"
{
    Properties{
        _Scale("Scale", Float) = 1.0
        _TimeScale("Time Scale", Float) = 1.0
        _Threshold("Color Threshold", Float) = 1.0
        _K("K",Float) = 1.0
        _Randomness("Randomness", Float) = 1.0
        _Color1("Color 1", Color) = (0,0,0,1)
        _Color2("Color 2", Color) = (1,1,1,1)
    }
    SubShader{
        Tags { "RenderType" = "Opaque" }
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float _Scale;
            float _TimeScale;
            float _Threshold;
            float _Randomness;
            float4 _Color1;
            float4 _Color2;

            float _K;

            float3 hash3(float3 p) {
                const float3 scale1 = float3(127.1, 311.7, 74.7);    // Prime constants
                const float3 scale2 = float3(269.5, 183.3, 246.1);   // More prime constants

                // First hashing step with dot products
                p = frac(sin(dot(p, scale1)) * 43758.5453);

                // Second hashing step for better randomness
                return frac(sin(dot(p, scale2)) * 43758.5453);
            }

            float voronoiF1_3D(float3 pos) {
                float3 ipos = floor(pos);
                float3 fpos = frac(pos);

                float minDist = 8.0;

                float2 b = float2(-2, 2);
                for (int z = b.x; z <= b.y; z++) {
                    for (int y = b.x; y <= b.y; y++) {
                        for (int x = b.x; x <= b.y; x++) {
                            float3 neighbor = float3(x, y, z);
                            float3 pointPos = (hash3(ipos + neighbor)-0.5)*_Randomness;
                            float3 diff = neighbor + pointPos - fpos;
                            float dist = dot(diff, diff);
                            minDist = min(minDist, dist);
                        }
                    }
                }
                return sqrt(minDist);
            }

            float smoothmin_quad(float a, float b, float k) {
                float h = max(k - abs(a - b), 0.0) / k;
                return min(a, b) - h * h * k * (1.0 / 6.0);
            }
            float smoothmin_exp(float a, float b, float k) {
                k *= 1.0;
                float r = exp2(-a / k) + exp2(-b / k);
                return -k * log2(r);
            }


            float smoothVoronoiF1_3D(float3 pos, float k) {
                float3 ipos = floor(pos);
                float3 fpos = frac(pos);

                float weightSum = 0.0;
                bool firstSet = false;

                float2 b = float2(-2, 2);
                for (int z = b.x; z <= b.y; z++) {
                    for (int y = b.x; y <= b.y; y++) {
                        for (int x = b.x; x <= b.y; x++) {
                            float3 neighbor = float3(x, y, z);
                            float3 pointPos = (hash3(ipos + neighbor)-0.5)*_Randomness;
                            float3 diff = neighbor + pointPos - fpos;
                            float dist = dot(diff, diff);

                            // Smoothing factor (you can tweak this value)
                            weightSum = firstSet ? smoothmin_quad(dist, weightSum, k) : dist;
                            firstSet = true;
                        }
                    }
                }
                return weightSum;
            }

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            v2f vert(appdata v) {
                v2f o;
                o.uv = v.uv;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target{
                float3 pos = float3(i.uv * _Scale, _Time.y * _TimeScale);
                float F1 = voronoiF1_3D(pos);
                float SF1 = smoothVoronoiF1_3D(pos, _K);
                
                //return F1 - SF1 < _Threshold ? _Color1 : _Color2;
                //return _Threshold < 0 ? F1 : SF1;

                return F1 - SF1;
            }
            ENDCG
        }
    }
}