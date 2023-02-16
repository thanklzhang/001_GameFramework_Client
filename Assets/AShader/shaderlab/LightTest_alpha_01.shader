//透明度测试
Shader "zxy/test_alpha_01"
{
	Properties
	{
		_Color ("Color",Color) = (1,1,1,1)
		_MainTex ("MainTex",2D) = "white" {}
		_AlphaCull ("Cull",Range(0,1)) = 0
	}

	SubShader
	{

		Tags {"Queue" = "AlphaTest" "IgnoreProjector" = "true"  "RenderType" = "TransparentCutout"}
		pass
		{
			Tags {"LightMode" = "ForwardBase"}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			float4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _AlphaCull;

			struct a2v
			{
				float4 pos : POSITION;
				float3 normal : NORMAL;
				float4 coord : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 worldNomral : TEXCOORD1;
				float3 worldLightDir :TEXCOORD2;
			};

			v2f vert(a2v v)
			{
				v2f f;
				f.pos = UnityObjectToClipPos(v.pos);
				f.uv = TRANSFORM_TEX(v.coord,_MainTex);
				f.worldNomral = UnityObjectToWorldNormal(v.normal);
				f.worldLightDir = WorldSpaceLightDir(v.pos);
				return f;
			}

			float4 frag(v2f f):SV_Target
			{
				float4 albedo = tex2D(_MainTex,f.uv);

				clip(albedo.a - _AlphaCull);

				float3  worldNomral = normalize(f.worldNomral);
				float3 worldLightDir = normalize(f.worldLightDir);

				float3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;
				float3 diffuse = _LightColor0 * albedo * (0.5 * dot( worldNomral,worldLightDir ) + 0.5);
				float3 result = ambient + diffuse;

				return float4(result,1);
			}

			ENDCG
		}
	}
}