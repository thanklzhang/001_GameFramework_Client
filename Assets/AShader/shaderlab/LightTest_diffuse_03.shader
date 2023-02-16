//逐像素 半兰伯特 着色
Shader "zxy/diffuse_test_03"
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
				

				float3 n = normalize(mul(unity_ObjectToWorld,v.nor));
				
				v2f f;             
				f.c = n;
				f.pos = UnityObjectToClipPos(v.pos);
				return f;
			}

			float4 frag(v2f f) : SV_TARGET
			{
				float3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
				float3 c_light = _LightColor0.rgb;
				float3 c_dif = _Diffuse.rgb;
				
				float3 n = normalize(f.c);
				float3 l = normalize( _WorldSpaceLightPos0.xyz);

				float3 c = c_light * c_dif * (0.5 * dot(n,l) + 0.5);


				return float4(ambient + c,1);
			}
			

			ENDCG

		}
	}
	
}