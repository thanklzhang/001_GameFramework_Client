//顶点操作 广告牌
Shader "zxy/test_vertex_04_billboard"
{
    Properties
    {
        _MainTex ("MainTex",2D) = "white" {}
        _Color ("Color",Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags{"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "DisableBatching" = "True"}
        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
           
           sampler2D _MainTex;
           float4 _MainTex_ST;
           float4 _Color;
            struct a2v
            {
                float4 vertex : POSITION;
                float2 coord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(a2v v)
            {

                float3 obj_center = float3(0,0,0);
                float3 obj_camera_pos = mul(unity_WorldToObject,_WorldSpaceCameraPos.xyz);
                float3 view = obj_camera_pos - obj_center;
                float3 normal = normalize(-view);

                float3 up = float3(0,1,0);
                float3 right = normalize(cross(up,normal));
                up = normalize(cross(normal,right));

                float3 offset = v.vertex - obj_center;
                float3 newPos = obj_center + offset.x * right + offset.y * up + offset.z * normal;


                v2f f;
                f.pos = UnityObjectToClipPos(float4(newPos,1));
                f.uv = TRANSFORM_TEX(v.coord,_MainTex);
                return f;
            }

            float4 frag(v2f f):SV_Target
            {
                float4 c = tex2D(_MainTex,f.uv) * _Color;
                float4 result = c;
                return result;
            }

            ENDCG
        }
          
    }
    FallBack "Transparent/VertexLit"
}
