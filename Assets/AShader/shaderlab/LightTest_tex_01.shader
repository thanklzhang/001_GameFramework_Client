//单张纹理
Shader "zxy/tex_test_01"
{
	Properties
	{
		_Color ("Color",Color) = (1,1,1,1)
		_MainTex ("MainTex",2D) = "white" {}
		_Gloss ("Gloss",float) = 30
		_Specular ("specular",Color) = (1,1,1,1)
	}

	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			float3 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Gloss;
			float4 _Specular;

			struct a2v
			{
				float4 pos : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 normal : TEXCOORD0;
				float3 worldPos : TEXCOORD1;
				float2 uv : TEXCOORD2;
			};

			v2f vert(a2v v)
			{
				v2f f;
				f.pos = UnityObjectToClipPos(v.pos);
				f.normal = normalize( UnityObjectToWorldNormal(v.normal));
				f.worldPos = mul(unity_ObjectToWorld,v.pos);
				f.uv = TRANSFORM_TEX( v.texcoord,_MainTex);
				return f;
			}

			float4 frag(v2f f) : SV_TARGET
			{
				float3 albebo = tex2D(_MainTex,f.uv);

				float3 n = normalize( f.normal);
				float3 l = normalize(UnityWorldSpaceLightDir(f.worldPos));

				float3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * _Color;
				//float3 diffuse = _LightColor0.rgb * albebo * (0.5 * (dot(n,l)) + 0.5);

				float3 diffuse = _LightColor0.rgb * albebo * saturate (dot(n,l));

				float3 v = normalize( UnityWorldSpaceViewDir(f.worldPos));
				float3 h = normalize(l + v);
				float3 specular = _LightColor0.rgb * _Specular * pow( saturate(dot(n,h)) , _Gloss);

				float3 result = ambient + diffuse + specular;
				return float4(result,1.0);
			}

			ENDCG
		}
	}

	
}