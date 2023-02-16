//逐像素 渐变
Shader "zxy/diffuse_test_04"
{
	Properties
	{
		//_Diffuse ("diffuseColor",Color) = (1,1,1,1)
		_Color ("Color",Color) = (1,1,1,1)
		_RampTex ("RampTex",2D) = "white"
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
				float2 coord : TEXCOORD0;
			};

			sampler2D _RampTex;
			float4 _RampTex_ST;

			float4 _Color;

			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 c : Color0;
				float2 uv : TEXCOORD0;
			};

			v2f vert(a2v v)
			{
				float3 n = normalize(mul(unity_ObjectToWorld,v.nor));
				
				v2f f;             
				f.c = n;
				f.pos = UnityObjectToClipPos(v.pos);

				f.uv = TRANSFORM_TEX( v.coord,_RampTex);
				return f;
			}

			float4 frag(v2f f) : SV_TARGET
			{
				float3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
				float3 c_light = _LightColor0.rgb;
				//float3 c_dif = _Diffuse.rgb;
				
				float3 n = normalize(f.c);
				float3 l = normalize( _WorldSpaceLightPos0.xyz);

				float ramp = 0.5 * dot(n,l) + 0.5;
				//sample
				float albedo = tex2D(_RampTex,float2(ramp,ramp));

				
				float3 c = c_light * albedo * _Color;


				return float4(ambient + c,1);
			}
			

			ENDCG

		}
	}
	
}