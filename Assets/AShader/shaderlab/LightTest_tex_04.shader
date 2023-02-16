//遮罩纹理 (切线空间下计算)
Shader "zxy/tex_test_04"
{
	Properties
	{
		_Color	("Color",Color) = (1,1,1,1)
		_MainTex ("MainTex",2D) = "white" {}
		_BumpTex ("_BumpTex",2D) = "white" {}
		_BumpScale ("BumpScale",float) = 1
		_Specular ("Specular",float) = 1
		_Gloss ("Gloss",float) = 1

		_MaskTex ("MaskTex",2D) = "white"
		_MaskScale ("MaskScale",float) = 1
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

			float4 _Color;
			sampler2D _MainTex;
			float4	_MainTex_ST;
			sampler2D _BumpTex;
			float4	_BumpTex_ST;
			float _BumpScale;
			float _Specular;
			float _Gloss;
			sampler2D _MaskTex;
			float _MaskScale;
			struct a2v
			{
				float4 pos : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 clipPos : SV_POSITION;
				float3 tan_viewDir : TEXCOORD0;
				float3 tan_lightDir : TEXCOORD1;
				float2 mainTex_uv : TEXCOORD2;
				float2 bumpTex_uv : TEXCOORD3;
			};

			v2f vert(a2v v)
			{
				v2f f;
				f.clipPos = UnityObjectToClipPos(v.pos);
				f.mainTex_uv = TRANSFORM_TEX(v.texcoord,_MainTex);
				f.bumpTex_uv = TRANSFORM_TEX(v.texcoord,_BumpTex);

				float3 nor = normalize( v.normal);
				float3 tan = normalize(v.tangent.xyz);
				float3 bi_tan = normalize(cross(nor,tan) * v.tangent.w);


				//float3x3 ObjToTan_Matrix = float3x3(tan,bi_tan,nor);

				
				TANGENT_SPACE_ROTATION;


				f.tan_viewDir = mul(rotation,normalize( ObjSpaceViewDir(v.pos)));
				f.tan_lightDir = mul(rotation ,normalize( ObjSpaceLightDir(v.pos)));

				return f;
				
			}

			float4 frag(v2f f) : SV_TARGET
			{
				float3 tan_viewDir = normalize( f.tan_viewDir);
				float3 tan_lightDir = normalize( f.tan_lightDir);

				float4 unpackNor = tex2D(_BumpTex,f.bumpTex_uv);
				//float3 tan_nor;
				//tan_nor.xy = unpackNor.xy * _BumpScale;
				//tan_nor.z = saturate(1 - sqrt(dot( tan_nor.xy , tan_nor.xy)));

				float3 tan_nor = UnpackNormal( unpackNor );
				
				tan_nor.xy = tan_nor.xy * _BumpScale;
				tan_nor.z = sqrt(1 - saturate(dot( tan_nor.xy , tan_nor.xy)));

				tan_nor = normalize(tan_nor);
				

				float3 albedo = tex2D(_MainTex,f.mainTex_uv) * _Color;
				

				float3 _ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;
				float3 _diffuse = _LightColor0.rgb * albedo * saturate(dot( tan_nor , tan_lightDir));
				float3 _half = normalize(tan_viewDir + tan_lightDir);
				float mask = tex2D(_MaskTex,f.mainTex_uv).b * _MaskScale;
			
				float3 _specular = _LightColor0.rgb * _Specular * pow( dot(tan_nor , _half ) ,_Gloss)
					* mask;

				float3 resultColor = _ambient + _diffuse + _specular;

				return float4( resultColor , 1);

			}

			ENDCG
		}
	}
}