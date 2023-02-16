// Upgrade NOTE: replaced '_LightMatrix0' with 'unity_WorldToLight'

//阴影
Shader "zxy/test_shadow_01"
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

			Tags {"LightMode" = "ForwardBase" "RendType" = "Opaque"}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			#pragma multi_compile_fwdbase

			float4 _V;
			float3 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Specular;
			float _Gloss;

			struct a2v
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 worldPos : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
				SHADOW_COORDS(3)
			};

			v2f vert(a2v v)
			{
				v2f f;
				f.pos = UnityObjectToClipPos(v.vertex);
				f.worldPos = mul(unity_ObjectToWorld,v.vertex);
				f.worldNormal = UnityObjectToWorldNormal(v.normal);
				f.uv = TRANSFORM_TEX(v.texcoord,_MainTex);

				TRANSFER_SHADOW(f)

				return f;
			}

			float4 frag(v2f f) : SV_Target
			{

				float3 worldNormal = normalize(f.worldNormal);
				float3 worldLightDir = _WorldSpaceLightPos0.xyz;
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


				//float atten = 1;
				//float shadow = SHADOW_ATTENUATION(f);

				UNITY_LIGHT_ATTENUATION(atten,f,f.worldPos);

				float3 selfLight = float3(0.2,0.2,0.2);
				float3 result =  _ambient + (_diffuse + _specular) * atten;

				//float dd = (0.5 * dot(worldNormal,worldLightDir) + 0.5);
				//float3 dd = worldNormal;

				//float3 result = float3(_WorldSpaceLightPos0.x + 1,0,0);


				//float3 result =  selfLight + _ambient + (float3(dd,dd,dd) + _specular);
			
				
				
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

			#pragma multi_compile_fwdadd_fullshadows

			float3 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Specular;
			float _Gloss;

			struct a2v
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 worldPos : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
				SHADOW_COORDS(3)
			};

			v2f vert(a2v v)
			{
				v2f f;
				f.pos = UnityObjectToClipPos(v.vertex);
				f.worldPos = mul(unity_ObjectToWorld,v.vertex);
				f.worldNormal = UnityObjectToWorldNormal(v.normal);
				f.uv = TRANSFORM_TEX(v.texcoord,_MainTex);

				TRANSFER_SHADOW(f)

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
				float3 _ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;
				//diffuse 半兰伯特光照模型
				float3 _diffuse = _LightColor0 * albedo * (0.5 * dot(worldNormal,worldLightDir) + 0.5);

				//specular
				float3 _half = normalize(worldViewDir + worldLightDir);
				float3 _specular = _LightColor0 * _Specular * pow(max(0,dot(worldNormal,_half)),_Gloss);

				//result = ambient + diffuse + specular
			

				//func 1
				//float shadow = SHADOW_ATTENUATION(f);
				//#ifdef USING_DIRECTIONAL_LIGHT
				//	float atten = 1 * shadow;
				//#else
				//	//world to light matrix
				//	#if defined (POINT)
				//		float3 lightSpacePos = mul(unity_WorldToLight,f.worldPos).xyz;
				//		float atten = tex2D(_LightTexture0,(dot(lightSpacePos,lightSpacePos).rr)).UNITY_ATTEN_CHANNEL * shadow;
				//	#elif defined (SPOT)
				//		 float4 lightCoord = mul(unity_WorldToLight, float4(f.worldPos));
				//        //然后使用该坐标对衰减纹理进行采样得到衰减值
				//        float atten = (lightCoord.z > 0) * tex2D(_LightTexture0, lightCoord.xy / lightCoord.w + 0.5).w * 
				//			tex2D(_LightTextureB0, dot(lightCoord, lightCoord).rr).UNITY_ATTEN_CHANNEL * shadow;
				//	#else
				//		float atten = 1 * shadow;
				//	#endif
				//#endif

				//func 2
				UNITY_LIGHT_ATTENUATION(atten,f,f.worldPos.xyz);



				float3 result = (_diffuse + _specular) * atten;

				return float4(result,1);

			}

		

			ENDCG

		
		}

		
	}
	Fallback "Specular"
}