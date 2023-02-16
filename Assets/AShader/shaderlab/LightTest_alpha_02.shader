//混合透明
Shader "zxy/test_alpha_02"
{
	Properties
	{
		_Color ("Color",Color) = (1,1,1,1)
		_MainTex ("MainTex",2D) = "white" {}
		_Alpha ("Alpha",Range(0,1)) = 1
	}

	SubShader
	{

		Tags {"Queue" = "Transparent" "IgnoreProjector" = "true"  "RenderType" = "TransparentCutout"}

		Pass
		{
			Tags {"LightMode" = "ForwardBase"}
			ZWrite On
			ColorMask 0
		}

		pass
		{
			Tags {"LightMode" = "ForwardBase"}

			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			float4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Alpha;

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
				float4 texColor = tex2D(_MainTex,f.uv);
				float4 albedo = texColor * _Color;

				//clip(albedo.a - _AlphaCull);

				float3  worldNomral = normalize(f.worldNomral);
				float3 worldLightDir = normalize(f.worldLightDir);

				float3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;
				float3 diffuse = _LightColor0 * albedo * (0.5 * dot( worldNomral,worldLightDir ) + 0.5);
				//float3 diffuse = _LightColor0 * albedo;
				float3 result = ambient + diffuse;

				return float4(result,texColor.a * _Alpha);
			}

			ENDCG
		}
	}
}