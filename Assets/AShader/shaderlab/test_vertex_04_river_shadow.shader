//顶点操作 模拟河流
Shader "zxy/test_vertex_04_river_shadow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" { }
        _Color ("Color Tint", Color) = (1, 1, 1, 1)
        _Magnitude ("Magnitude", Float) = 1 // 波动幅度
        _Frequency ("Frequency", Float) = 1 // 频率
        _InvWaveLength ("Inverse Wave Length", Float) = 10 // 波长
        _Speed ("Speed", Float) = 0.5
    }
    SubShader
    {
        // "DisableBatching" = "True" 取消对该Shader的批处理操作
        // 因为此Shader包含了模型空间的顶点动画，而批处理会合并所有相关的模型，导致这些模型各自的模型空间丢失
        Tags { "RenderType" = "Transparent" "IgnoreProjector" = "True" "Queue" = "Transparent" "DisableBatching" = "True" }

        Pass
        {
            Tags { "LightMode" = "ForwardBase" }

            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            // _MainTex纹理的缩放和偏移系数
            float4 _MainTex_ST;
            fixed4 _Color;
            float _Magnitude;
            float _Frequency;
            float _InvWaveLength;
            float _Speed;

            // 应用传递给顶点着色器的数据
            struct a2v
            {
                float4 vertex: POSITION; // 语义：模型顶点坐标
                float4 texcoord: TEXCOORD0; // 语义：模型第一组纹理坐标
            };

            // 顶点着色器传递给片元着色器的数据
            struct v2f
            {
                float4 pos: SV_POSITION; // 语义：裁剪空间顶点坐标
                float2 uv: TEXCOORD0;
            };

            // 顶点着色器函数
            v2f vert(a2v v)
            {
                v2f o;

                // 计算顶点位移量，只对顶点的x方向进行位移
                float4 offset;
                offset.xyw = float3(0.0, 0.0, 0.0);
                // 使用_Frequency * _Time.y 控制正弦函数的频率
                // 再加上(v.vertex.x + v.vertex.y + v.vertex.z) * _InvWaveLength 让不同的位置具有不同的位移
                // 最后乘以_Magnitude 控制波动幅度
                //offset.x = sin(_Frequency * _Time.y + (v.vertex.x + v.vertex.y + v.vertex.z) * _InvWaveLength) * _Magnitude;

                offset.z = sin(_Frequency * _Time.y + (v.vertex.x + v.vertex.y + v.vertex.z) * _InvWaveLength) * _Magnitude;

                // 将顶点坐标从模型空间变换到裁剪空间
                o.pos = UnityObjectToClipPos(v.vertex + offset);
                
                // 纹理动画
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.uv += float2( _Time.y * _Speed, 0);

                return o;
            }

            // 片元着色器函数
            fixed4 frag(v2f i): SV_TARGET
            {
                fixed4 color = tex2D(_MainTex, i.uv);
                color.rgb *= _Color.rgb;

                return color;
            }

            ENDCG

        }


        Pass
        {
            Tags { "LightMode" = "ShadowCaster" }

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile_shadowcaster
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            // _MainTex纹理的缩放和偏移系数
            //float4 _MainTex_ST;
            //fixed4 _Color;
            float _Magnitude;
            float _Frequency;
            float _InvWaveLength;
            float _Speed;

            // 应用传递给顶点着色器的数据
            struct a2v
            {
                float4 vertex: POSITION; // 语义：模型顶点坐标
                float4 texcoord: TEXCOORD0; // 语义：模型第一组纹理坐标
                float3 normal : NORMAL;
            };

            // 顶点着色器传递给片元着色器的数据
            struct v2f
            {
                //float4 pos: SV_POSITION; // 语义：裁剪空间顶点坐标
                //float2 uv: TEXCOORD0;
                V2F_SHADOW_CASTER;
            };

            // 顶点着色器函数
            v2f vert(a2v v)
            {
                v2f o;

                // 计算顶点位移量，只对顶点的x方向进行位移
                float4 offset;
                offset.xyw = float3(0.0, 0.0, 0.0);
                // 使用_Frequency * _Time.y 控制正弦函数的频率
                // 再加上(v.vertex.x + v.vertex.y + v.vertex.z) * _InvWaveLength 让不同的位置具有不同的位移
                // 最后乘以_Magnitude 控制波动幅度
                //offset.x = sin(_Frequency * _Time.y + (v.vertex.x + v.vertex.y + v.vertex.z) * _InvWaveLength) * _Magnitude;

                offset.z = sin(_Frequency * _Time.y + (v.vertex.x + v.vertex.y + v.vertex.z) * _InvWaveLength) * _Magnitude;

                // 将顶点坐标从模型空间变换到裁剪空间

                v.vertex = v.vertex + offset;
                //o.pos = UnityObjectToClipPos(v.vertex + offset);

                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                
                //// 纹理动画
                //o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                //o.uv += float2( _Time.y * _Speed, 0);

                return o;
            }

            // 片元着色器函数
            fixed4 frag(v2f i): SV_TARGET
            {
                //fixed4 color = tex2D(_MainTex, i.uv);
                //color.rgb *= _Color.rgb;

                //return color;

                SHADOW_CASTER_FRAGMENT(i)

            }

            ENDCG

        }

    }
    FallBack "VertexLit"
}
