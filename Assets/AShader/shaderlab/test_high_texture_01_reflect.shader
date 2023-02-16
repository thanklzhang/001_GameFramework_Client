//高级纹理 cubemap 反射
Shader "zxy/test_high_texture_01_reflect"
{
	Properties
	{
		_Color ("Color",Color) = (1,1,1,1)
		_ReflectColor ("ReflectColor",Color) = (1,1,1,1)
		_ReflectAmount ("ReflectAmount",Range(0,1)) = 1
		_CubeMap ("CubeMap",Cube) = "_Skybox" {}
	}

	SubShader
	{
		Pass
		{
			Tags {"LightMode" = "ForwardBase"}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase

			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			float4 _Color;
			float4 _ReflectColor;
			float _ReflectAmount;
			samplerCUBE _CubeMap;

			struct a2v
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 worldPos : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
				SHADOW_COORDS(2)
			};

			v2f vert(a2v v)
			{
				v2f f;
				f.pos = UnityObjectToClipPos(v.vertex);
				f.worldPos = mul(unity_ObjectToWorld,v.vertex);
				f.worldNormal = UnityObjectToWorldNormal(v.normal);
				TRANSFER_SHADOW(f)
				return f;
			}

			float4 frag(v2f f) : SV_Target
			{
				
				float3 worldNormal = normalize(f.worldNormal);
				float3 worldLightDir = normalize(UnityWorldSpaceLightDir(f.worldPos));
				float3 worldViewDir = normalize(UnityWorldSpaceViewDir(f.worldPos));
				float3 worldRelectDir = normalize(reflect(-worldViewDir,worldNormal));

				float3 reflectColor = texCUBE(_CubeMap,worldRelectDir) * _ReflectColor;

				UNITY_LIGHT_ATTENUATION(atten,f,f.worldPos);

				float3 ambient = UNITY_LIGHTMODEL_AMBIENT;
				float3 diffuse = _LightColor0 * _Color * saturate(dot(worldNormal , worldLightDir));
				float3 result = ambient + lerp(diffuse,reflectColor,_ReflectAmount) * atten;

				return float4(result,1);
			}

			ENDCG
		}
	}


	FallBack "VertexLit"
}