//混合透明
Shader "zxy/test_shadow_03_alpha_blend"
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
			Tags {"LightMode" = "ForwardBase" }

			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase

			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			float4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Alpha;

			struct a2v
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 coord : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 worldNomral : TEXCOORD1;
				float3 worldLightDir :TEXCOORD2;
				float4 worldPos : TEXCOORD3;
				SHADOW_COORDS(4)

			};

			v2f vert(a2v v)
			{
				v2f f;
				f.pos = UnityObjectToClipPos(v.vertex);
				f.uv = TRANSFORM_TEX(v.coord,_MainTex);
				f.worldNomral = UnityObjectToWorldNormal(v.normal);
				f.worldLightDir = WorldSpaceLightDir(v.vertex);
				f.worldPos = mul(unity_ObjectToWorld,v.vertex);
				TRANSFER_SHADOW(f)
				return f;
			}

			float4 frag(v2f f):SV_Target
			{
				float4 texColor = tex2D(_MainTex,f.uv);
				float4 albedo = texColor * _Color;

				//clip(albedo.a - _AlphaCull);

				float3  worldNomral = normalize(f.worldNomral);
				//float3 worldLightDir = normalize(f.worldLightDir);

				float3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);


				float3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;
				float3 diffuse = _LightColor0 * albedo * (0.5 * dot( worldNomral,worldLightDir ) + 0.5);
				//float3 diffuse = _LightColor0 * albedo;

				//float shadow = SHADOW_ATTENUATION(f);


				UNITY_LIGHT_ATTENUATION(atten,f,f.worldPos.xyz)
				float3 result = ambient + diffuse * atten;


				return float4(result,texColor.a * _Alpha);
			}

			ENDCG
		}
	}

	FallBack "VertexLit"
}