//后处理 边缘检测(利用深度纹理重建世界坐标)
Shader "zxy/post_effect_08_edge_detection"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" { }
        _EdgeColor ("EdgeColor",Color) = (1,1,1,1)
        _BackgroundColor ("BackgroundColor",Color) = (1,1,1,1)

        _SampleDistance ("SampleDistance",Float) = 1
        _NormalSensitivity ("NormalSensitivity",Float) = 1
        _DepthSensitivity ("DepthSensitivity",Float) = 1

        _EdgeOnly ("EdgeOnly",Float) = 0
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
            float _SampleDistance;
            float _NormalSensitivity;
            float _DepthSensitivity;

            float _EdgeOnly;

            sampler2D _CameraDepthNormalsTexture;
        
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
                float2 uv[5] : TEXCOORD0;
            };

            // 顶点着色器函数
            v2f vert(a2v v)
            {
                v2f f;
                f.pos = UnityObjectToClipPos(v.vertex);

                float2 uv = v.vertex;

                f.uv[0] = uv;

                #if UNITY_UV_STARTS_AT_TOP
			    if(_MainTex_TexelSize.y < 0)
			    {
			    	f.uv[0].y = 1 - f.uv[0].y;
			    }
			    #endif

                f.uv[1] = uv + _MainTex_TexelSize.xy * float2(1,1) * _SampleDistance;
                f.uv[2] = uv + _MainTex_TexelSize.xy * float2(-1,-1) * _SampleDistance;
                f.uv[3] = uv + _MainTex_TexelSize.xy * float2(-1,1) * _SampleDistance;
                f.uv[4] = uv + _MainTex_TexelSize.xy * float2(1,-1) * _SampleDistance;
              
                return f;
            }

            float CheckSame(float4 a,float4 b)
            {
                //法线不用解码 因为只是比较
                float2 aNormal = a.xy;
                float2 bNormal = b.xy;

                float aDepth = DecodeFloatRG(a.zw);
                float bDepth = DecodeFloatRG(b.zw);

                float2 diffNormal = abs(aNormal - bNormal) * _NormalSensitivity;
                float isSameNormal = (diffNormal.x + diffNormal.y) < 0.1;

                float diffDepth = abs(aDepth - bDepth) * _DepthSensitivity;
                float isSameDepth = diffDepth < 0.1 * aDepth;

                return isSameNormal * isSameDepth ? 1 : 0;
            }

            // 片元着色器函数
            fixed4 frag(v2f f): SV_TARGET
            {
                float4 s1 = tex2D(_CameraDepthNormalsTexture,f.uv[1]);
                float4 s2 = tex2D(_CameraDepthNormalsTexture,f.uv[2]);
                float4 s3 = tex2D(_CameraDepthNormalsTexture,f.uv[3]);
                float4 s4 = tex2D(_CameraDepthNormalsTexture,f.uv[4]);

                float edge = 1;

                edge *= CheckSame(s1,s2);
                edge *= CheckSame(s3,s4);
               

                float3 orgColor = tex2D(_MainTex, f.uv[0]); 

                //边颜色 -> 原始颜色(edge 为 1 代表相同 也就是非边缘 , 0 则是边缘)
                float3 eToO = lerp(_EdgeColor,orgColor,edge);

                //边颜色 -> 背景颜色
                float3 eToB = lerp(_EdgeColor,_BackgroundColor,edge);

                float3 result = lerp(eToO,eToB,_EdgeOnly);

                return float4(result,1);
            }

            ENDCG

        }
    }
    FallBack Off
}
