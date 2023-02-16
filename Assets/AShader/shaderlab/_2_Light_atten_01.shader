// Upgrade NOTE: replaced '_LightMatrix0' with 'unity_WorldToLight'

//光照衰减
Shader "zxy/atten/_2_Light_atten_01"
{
	Properties
	{
		_Color ("Color",Color) = (1,1,1,1)
		_MainTex ("MainTex",2D) = "white" {}
		_Specular ("Specular",float) = 1
		_Gloss ("Gloss",float) = 1
		_V ("V",Vector) = (1,1,1,1)
	}

	SubShader
	{
		Pass
		{
			//Base Pass
			//只计算平行光

			Tags {"LightMode" = "ForwardBase"}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			#pragma multi_compile_fwdbase

			float4 _V;
			float3 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Specular;
			float _Gloss;

			struct a2v
			{
				float4 pos : POSITION;
				float3 normal : NORMAL;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 worldPos : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
			};

			v2f vert(a2v v)
			{
				v2f f;
				f.pos = UnityObjectToClipPos(v.pos);
				f.worldPos = mul(unity_ObjectToWorld,v.pos);
				f.worldNormal = UnityObjectToWorldNormal(v.normal);
				f.uv = TRANSFORM_TEX(v.texcoord,_MainTex);
				return f;
			}

			float4 frag(v2f f) : SV_Target
			{

				float3 worldNormal = normalize(f.worldNormal);
				float3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
				float3 worldViewDir = normalize(UnityWorldSpaceViewDir(f.worldPos));

				//sample
				float3 albedo = tex2D(_MainTex,f.uv) * _Color;

				//ambient
				float3 _ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;
				//diffuse 半兰伯特光照模型
				float3 _diffuse = _LightColor0 * albedo * (0.5 * dot(worldNormal,worldLightDir) + 0.5);

				//specular
				float3 _half = normalize(worldViewDir + worldLightDir);
				float3 _specular = _LightColor0 * _Specular * pow(max(0,dot(worldNormal,_half)),_Gloss);

				//result = ambient + diffuse + specular
				float atten = 1;
				float3 result = _ambient + (_diffuse + _specular) * atten;
			

				
				return float4(result,1);

				//float2 ii = dot(_V , _V).rr;
				//return float4(ii,0,1);

			}

			ENDCG
		}

		Pass
		{
			//Add Pass
			//其他非平行光

			Tags {"LightMode" = "ForwardAdd"}
			Blend One One

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			#pragma multi_compile_fwdadd

			float3 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Specular;
			float _Gloss;

			struct a2v
			{
				float4 pos : POSITION;
				float3 normal : NORMAL;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 worldPos : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
			};

			v2f vert(a2v v)
			{
				v2f f;
				f.pos = UnityObjectToClipPos(v.pos);
				f.worldPos = mul(unity_ObjectToWorld,v.pos);
				f.worldNormal = UnityObjectToWorldNormal(v.normal);
				f.uv = TRANSFORM_TEX(v.texcoord,_MainTex);
				return f;
			}

			float4 frag(v2f f) : SV_Target
			{

				float3 worldNormal = normalize(f.worldNormal);
				
			
				float3 worldViewDir = normalize(UnityWorldSpaceViewDir(f.worldPos));
				#ifdef USING_DIRECTIONAL_LIGHT
					float3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
				#else
					float3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz - f.worldPos.xyz);
				#endif
			

				//sample
				float3 albedo = tex2D(_MainTex,f.uv) * _Color;

				//ambient
				//float3 _ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;
				//diffuse 半兰伯特光照模型
				float3 _diffuse = _LightColor0 * albedo * (0.5 * dot(worldNormal,worldLightDir) + 0.5);

				//specular
				float3 _half = normalize(worldViewDir + worldLightDir);
				float3 _specular = _LightColor0 * _Specular * pow(max(0,dot(worldNormal,_half)),_Gloss);

				//result = ambient + diffuse + specular

				#ifdef USING_DIRECTIONAL_LIGHT
					float atten = 1;
				#else
					//world to light matrix
					float3 lightSpacePos = mul(unity_WorldToLight,f.worldPos).xyz;
					float atten = tex2D(_LightTexture0,(dot(lightSpacePos,lightSpacePos).rr)).UNITY_ATTEN_CHANNEL;
				#endif

				//float3 result = _ambient + (_diffuse + _specular) * atten;
				float3 result = (_diffuse + _specular) * atten;


				return float4(result,1);

			}

			ENDCG


		}
	}
}