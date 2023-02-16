//后处理 边缘检测
Shader "zxy/post_effect_02_edge_detection"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" { }
        _EdgeColor ("EdgeColor",Color) = (1,1,1,1)
        _BackgroundColor ("BackgroundColor",Color) = (1,1,1,1)
    }
    SubShader
    {
        Pass
        {
            ZTest Always
            ZWrite Off
            Cull Off

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            // _MainTex纹理的缩放和偏移系数
            float4 _MainTex_ST;
            float2 _MainTex_TexelSize;
            float4 _EdgeColor;
            float4 _BackgroundColor;
        
            // 应用传递给顶点着色器的数据
            struct a2v
            {
                float4 vertex: POSITION; // 语义：模型顶点坐标
                float2 texcoord: TEXCOORD0; // 语义：模型第一组纹理坐标
            };

            // 顶点着色器传递给片元着色器的数据
            struct v2f
            {
                float4 pos: SV_POSITION; // 语义：裁剪空间顶点坐标
                float2 uv[9] : TEXCOORD0;
            };

            // 顶点着色器函数
            v2f vert(a2v v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                float2 uv = TRANSFORM_TEX(v.texcoord,_MainTex);

                o.uv[0] = uv + _MainTex_TexelSize.xy * float2(-1,-1);
                o.uv[1] = uv + _MainTex_TexelSize.xy * float2(0,-1);
                o.uv[2] = uv + _MainTex_TexelSize.xy * float2(1,-1);
                o.uv[3] = uv + _MainTex_TexelSize.xy * float2(-1,0);
                o.uv[4] = uv + _MainTex_TexelSize.xy * float2(0,0);
                o.uv[5] = uv + _MainTex_TexelSize.xy * float2(1,0);
                o.uv[6] = uv + _MainTex_TexelSize.xy * float2(-1,1);
                o.uv[7] = uv + _MainTex_TexelSize.xy * float2(0,1);
                o.uv[8] = uv + _MainTex_TexelSize.xy * float2(1,1);
                return o;
            }

            float GetLuminance(float4 color)
            {
                return color.r * 0.2125 + color.g * 0.7154 + color.b * 0.0721;
            }

            // 片元着色器函数
            fixed4 frag(v2f f): SV_TARGET
            {

                float gx[9] = {-1,0,1,
                               -2,0,2,
                               -1,0,1};
               float gy[9] = {1,2,1,
                               -0,0,0,
                               -1,-2,-1};

                //梯度
                float xEdge = 0;
                float yEdge = 0;
                for(int i = 0;i < 9;++i)
                {
                    float luminance = GetLuminance(tex2D(_MainTex, f.uv[i]));

                    xEdge += gx[i] * luminance;
                    yEdge += gy[i] * luminance;
                }
               
                float edgeCheck = 1 - abs(xEdge) - abs(yEdge);
              

                float3 orgColor = tex2D(_MainTex, f.uv[4]); 
                //edgeCheck 越小 越像边缘

                //边颜色 -> 原始颜色
                float3 eToO = lerp(_EdgeColor,orgColor,edgeCheck);
                //边颜色 -> 背景颜色
                float3 eToB = lerp(_EdgeColor,_BackgroundColor,edgeCheck);
                
                //float3 color = eToO;

                float3 color = eToB;
               
                return float4( color,1);
            }

            ENDCG

        }
    }
    FallBack Off
}
