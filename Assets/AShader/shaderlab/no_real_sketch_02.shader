// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

//非真实渲染 素描效果
Shader "zxy/no_real_sketch_02"
{
	Properties
	{
		_Color ("Color",Color) = (1,1,1,1)
		_TileFactor ("TileFactor",Float) = 1
		_OutlineLength ("_OutlineLength",Float) = 1
		
		_T_0 ("T0",2D) = "white" {}
		_T_1 ("T1",2D) = "white" {}
		_T_2 ("T2",2D) = "white" {}
		_T_3 ("T3",2D) = "white" {}
		_T_4 ("T4",2D) = "white" {}
		_T_5 ("T5",2D) = "white" {}
		
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" "Queue" = "Geometry"}

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
		float _TileFactor;
		sampler2D _T_0;
		sampler2D _T_1;
		sampler2D _T_2;
		sampler2D _T_3;
		sampler2D _T_4;
		sampler2D _T_5; 
	

		struct v2f
		{
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
			float3 weight_0 : TEXCOORD1;
			float3 weight_1 : TEXCOORD2;
			float3 worldPos : TEXCOORD3;
			SHADOW_COORDS(4)
		};

		v2f vert(a2v v)
		{
			v2f f;
			
			f.uv = v.texcoord * _TileFactor;

			f.pos = UnityObjectToClipPos(v.vertex);
			

			float3 normal = normalize(UnityObjectToWorldNormal( v.normal));
			float3 lightDir = normalize(WorldSpaceLightDir(v.vertex));
			float diff = max(0, dot( normal,lightDir));
			float factor = diff * 7.0;

			if(factor > 6)
			{

			}
			else if(factor > 5)
			{
				f.weight_0.x = factor - 5;
			}
			else if(factor > 4)
			{
				f.weight_0.x = factor - 4;
				f.weight_0.y = 1 - f.weight_0.x;
			}
			else if(factor > 3)
			{
				f.weight_0.y = factor - 3;
				f.weight_0.z = 1 - f.weight_0.y;
			}
			else if(factor > 2)
			{
				f.weight_0.z = factor - 2;
				f.weight_1.x = 1 - f.weight_0.z;
			}
			else if(factor > 1)
			{
				f.weight_1.x = factor - 1;
				f.weight_1.y = 1 - f.weight_1.x;
			}
			else
			{
				f.weight_1.y = factor - 0;
				f.weight_1.z = 1 - f.weight_1.y;
			}

			
			f.worldPos = mul(unity_ObjectToWorld,v.vertex);
			return f;
		}

		float4 frag(v2f f) : SV_TARGET
		{
			float4 h_0 = tex2D(_T_0,f.uv) * f.weight_0.x;
			float4 h_1 = tex2D(_T_1,f.uv) * f.weight_0.y;
			float4 h_2 = tex2D(_T_2,f.uv) * f.weight_0.z;
			float4 h_3 = tex2D(_T_3,f.uv) * f.weight_1.x;
			float4 h_4 = tex2D(_T_4,f.uv) * f.weight_1.y;
			float4 h_5 = tex2D(_T_5,f.uv) * f.weight_1.z;

			float3 whiteColor = 1 - f.weight_0.x - f.weight_0.y - f.weight_0.z - f.weight_1.x - f.weight_1.y - f.weight_1.z;
			float3 hatchColor = h_0 + h_1 + h_2 + h_3 + h_4 + h_5 + whiteColor;

			UNITY_LIGHT_ATTENUATION(atten,f,f.worldPos)

			return float4(hatchColor * _Color * atten,1);
		}
		

		
		ENDCG


		UsePass "zxy/no_real_cartoon_01/OUTLINE"

		Pass
		{
			Tags {"LightMode"="ForwardBase"}

			

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			ENDCG
		}


	}

	FallBack "Diffuse"
	
}