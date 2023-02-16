//后处理 亮度 饱和度 对比度 
Shader "zxy/post_effect_01_BSC"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" { }
        _Bright ("Bright",float) = 1
        _Saturation ("Saturation",float) = 1
        _Contrast ("Contrast",float) = 1
    }
    SubShader
    {
        // "DisableBatching" = "True" 取消对该Shader的批处理操作
        // 因为此Shader包含了模型空间的顶点动画，而批处理会合并所有相关的模型，导致这些模型各自的模型空间丢失
        //Tags { "RenderType" = "Transparent" "IgnoreProjector" = "True" "Queue" = "Transparent" "DisableBatching" = "True" }

        Pass
        {
            //Tags { "LightMode" = "ForwardBase" }

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
        
            float _Bright;
            float _Saturation;
            float _Contrast;

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
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord,_MainTex);
                return o;
            }

            // 片元着色器函数
            fixed4 frag(v2f f): SV_TARGET
            {
                fixed3 color = tex2D(_MainTex, f.uv);

                //亮度
                color = color * _Bright;

                //饱和度
                float luminance = color.r * 0.2125 + color.g * 0.7154 + color.b * 0.0721;
                color = lerp(float3(luminance , luminance ,luminance ),color,_Saturation);

                //对比度
                float avgColor = float3(0.5,0.5,0.5);
                color = lerp(avgColor , color ,_Contrast);

                return float4( color,1);
            }

            ENDCG

        }
    }
    FallBack Off
}
