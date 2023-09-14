Shader "Custom/NineSliceShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _Length ("Length", Range(0, 10)) = 1
        _Width ("Width", Range(0, 10)) = 1
    }
 
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 200
        
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            CGPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            uniform sampler2D _MainTex;
            uniform float4 _Color;
            uniform float _SliceDistance;
            uniform float _Length;
            uniform float _Width;

           

            struct vertexInput
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct vertexOutput
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            vertexOutput vert(vertexInput input)
            {
                vertexOutput output;
                float3 pos = input.vertex;

                float w = _Width * (step(0.5, pos.x) - 0.5);
                float l = _Length * step(0.001, pos.y) - 0.5;
                
                pos = float3(w, l, 0);


                output.pos = UnityObjectToClipPos(pos);
                output.uv = input.uv;
                return output;
            }

            fixed4 frag(vertexOutput input) : SV_Target
            {
                float2 uv = input.uv;

                fixed4 color = tex2D(_MainTex, uv) * _Color;
                
                

                return color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}