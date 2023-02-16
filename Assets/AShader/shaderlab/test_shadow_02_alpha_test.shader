//透明度测试
Shader "zxy/test_shadow_02_alpha_test"
{
	Properties
	{
		_Color ("Color",Color) = (1,1,1,1)
		_MainTex ("MainTex",2D) = "white" {}
		_Cutoff ("Cull",Range(0,1)) = 0
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

			#pragma multi_compile_fwdbase

			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"
			
			

			float4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Cutoff;

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
				float4 albedo = tex2D(_MainTex,f.uv);

				clip(albedo.a - _Cutoff);

				float3  worldNomral = normalize(f.worldNomral);
				float3 worldLightDir = normalize(f.worldLightDir);

				float3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;
				float3 diffuse = _LightColor0 * albedo * (0.5 * dot( worldNomral,worldLightDir ) + 0.5);
				UNITY_LIGHT_ATTENUATION(atten,f,f.pos)
				float3 result = ambient + diffuse * atten;

				return float4(result,1);
			}

			ENDCG
		}
	}

	Fallback "Transparent/Cutout/VertexLit"
}