//逐顶点 高洛德着色
Shader "zxy/diffuse_test_01"
{
	Properties
	{
		_Diffuse ("diffuseColor",Color) = (1,1,1,1)
	}

	SubShader
	{
		Pass
		{
			Tags
			{
				"LightMode" = "ForwardBase"
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "Lighting.cginc"
			
			struct a2v
			{
				float4 pos : POSITION;
				float3 nor : NORMAL;
			};

			float3 _Diffuse;

			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 c : Color0;
			};

			v2f vert(a2v v)
			{
				float3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;

				float3 c_light = _LightColor0.rgb;
				float3 c_dif = _Diffuse.rgb;
				float3 n = normalize(mul(unity_ObjectToWorld,v.nor));
				float3 l = normalize( _WorldSpaceLightPos0.xyz);

				float3 c = c_light * c_dif * saturate(dot(n,l));
				
				v2f f;             
				f.c = ambient + c;
				f.pos = UnityObjectToClipPos(v.pos);
				return f;
			}

			float4 frag(v2f f) : SV_TARGET
			{
				return float4(f.c,1);
			}
			

			ENDCG

		}
	}
	
}