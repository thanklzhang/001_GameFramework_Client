//高级纹理 渲染纹理 镜子
Shader "zxy/test_high_texture_04_mirror"
{
	Properties
	{
		_Main_Tex ("_Main_Tex",2D) = "white" {}
	}

	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _Main_Tex;
			float4 _Main_Tex_ST;
			

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
				v2f f;
				f.pos = UnityObjectToClipPos(v.vertex);
				f.uv = v.coord;
				//f.uv.x = 1 - f.uv.x;
				return f;
			}

			float4 frag(v2f f) : SV_Target
			{
				f.uv.x = 1 - f.uv.x;
				float3 albedo = tex2D(_Main_Tex,f.uv);
				return float4(albedo,1);
			}

			ENDCG
		}
	}


	FallBack "VertexLit"
}