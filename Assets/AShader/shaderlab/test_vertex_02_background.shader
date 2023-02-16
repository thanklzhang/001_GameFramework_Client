//顶点操作 滚动的背景
Shader "zxy/test_vertex_02_background"
{
	Properties
	{
		_MainTex ("MainTex",2D) = "white" {}
		_MainTex2 ("MainTex2",2D) = "white" {}
		_Multiplier ("Multiplier",float) = 1
		_X_Speed ("X_Speed",float) = 2
		_X_Speed2 ("X_Speed2",float) = 2
	}

	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _MainTex2;
			float4 _MainTex2_ST;

			float _X_Speed;
			float _X_Speed2;

			float _Multiplier;

			struct a2v
			{
				float4 vertex : POSITION;
				float2 coord : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
			};

			v2f vert(a2v v)
			{
				v2f f;
				f.pos = UnityObjectToClipPos(v.vertex);
				f.uv = TRANSFORM_TEX(v.coord,_MainTex) + frac(float2(_Time.y * _X_Speed,0));
				f.uv2 = TRANSFORM_TEX(v.coord,_MainTex2) + frac(float2(_Time.y * _X_Speed2,0));

				return f;
			}

			float4 frag(v2f f) : SV_Target
			{
				float4 c1 = tex2D(_MainTex,f.uv);
				float4 c2 = tex2D(_MainTex2,f.uv2);
				 
				float4 result = lerp(c1,c2,c2.a);
				result.rgb *= _Multiplier;

				return result;
			}

			ENDCG

		}

	}

	FallBack "VertexLit"
}