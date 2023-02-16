// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

//非真实渲染 卡通效果
Shader "zxy/no_real_cartoon_01"
{
	Properties
	{
		_MainTex ("_MainTex",2D) = "white" {}
		_Color ("Color",Color) = (1,1,1,1)
		_OutlineLength ("_OutlineLength",Float) = 1
		_OutlineColor ("OutlineColor",Color) = (1,1,1,1)
		_Ramp ("Ramp",2D) = "white" {}
		_SpecularSize ("SpecularSize",Float) = 0.6
		
		
	}

	SubShader
	{

			CGINCLUDE
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			struct a2v
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 texcoord : TEXCOORD0;
			};


			float4 _Color;
			float _OutlineLength;
			float4 _OutlineColor;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _Ramp;
			float _SpecularSize;

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
				
				float4 pos = mul(UNITY_MATRIX_MV,v.vertex);
				
				float3 normal = mul(UNITY_MATRIX_IT_MV,v.normal);
				normal.z = -0.5;

				pos = pos + float4( normalize( normal ),0)* _OutlineLength;


				f.pos = mul(UNITY_MATRIX_P,pos);

				return f;
			}

			float4 frag(v2f f) : SV_TARGET
			{
				float4 result = float4(_OutlineColor);
				return result;
			}
			

			v2f vert2(a2v v)
			{
				v2f f;
				
				float4 pos = UnityObjectToClipPos(v.vertex);
				f.uv = TRANSFORM_TEX(v.texcoord,_MainTex);
				f.pos = pos;
				f.worldPos = mul(unity_ObjectToWorld,v.vertex);
				f.worldNormal = UnityObjectToWorldNormal(v.normal);

				TRANSFER_SHADOW(f);

				return f;
			}

			float4 frag2(v2f f) : SV_TARGET
			{

				float3 worldNormal = normalize( f.worldNormal);
				float4 worldPos = f.worldPos;
				float3 worldView = normalize(WorldSpaceViewDir(worldPos));
				float3 worldLightDir = normalize(UnityWorldSpaceLightDir(worldPos));
				float3 worldHalf = normalize(worldLightDir + worldView);

				float3 mainColor = tex2D(_MainTex,f.uv);
				float3 albedo = mainColor * _Color;

				//环境光
				float3 ambient = UNITY_LIGHTMODEL_AMBIENT * albedo;

				//漫反射
				float3 c = _LightColor0;
				float3 m = albedo;

				UNITY_LIGHT_ATTENUATION(atten,f,worldPos);

				float _half = (0.5 * dot(worldNormal,worldLightDir) + 0.5) * atten;
				
				float3 diffuse = c * m * tex2D(_Ramp,float2(_half,_half));

				//float _half = (0.5 * dot(worldNormal,worldLightDir) + 0.5);
				
				//float3 diffuse = c * m * _half;

				//镜面高光
				float sp = dot(worldNormal,worldHalf);
				float w = fwidth(sp) * 2;
				float specular = c * lerp(-0,1, smoothstep(-w,w,sp + _SpecularSize - 1)) * step(0.001,_SpecularSize);

				float3 result = ambient + diffuse + specular;

				return float4(result,1);
			}

			ENDCG


		Pass
		{
			NAME "OUTLINE"
			Cull Front

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}

		Pass
		{
			Tags {"LightMode"="ForwardBase"}

			Cull Back

			CGPROGRAM
			#pragma vertex vert2
			#pragma fragment frag2
			#pragma multi_compile_fwdbase
			ENDCG
		}


	}

	FallBack "Diffuse"
	
}