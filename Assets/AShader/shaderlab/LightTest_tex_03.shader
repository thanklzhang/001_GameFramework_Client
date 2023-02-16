//法线纹理 (世界空间下计算)
Shader "zxy/tex_test_03"
{
	Properties
	{
		_Color	("Color",Color) = (1,1,1,1)
		_MainTex ("MainTex",2D) = "white" {}
		_BumpTex ("_BumpTex",2D) = "white" {}
		_BumpScale ("BumpScale",float) = 1
		_Specular ("Specular",float) = 1
		_Gloss ("Gloss",float) = 1
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

				//float3 world_viewDir : TEXCOORD0;
				//float3 world_lightDir : TEXCOORD1;

				float3 tan_tan : TEXCOORD2;
				float3 tan_binormal : TEXCOORD3;
				float3 tan_normal : TEXCOORD4;

				float2 mainTex_uv : TEXCOORD5;
				float2 bumpTex_uv : TEXCOORD6;

				float3 worldPos : TEXCOORD7;
			};

			v2f vert(a2v v)
			{
				v2f f;
				f.clipPos = UnityObjectToClipPos(v.pos);

				f.mainTex_uv = TRANSFORM_TEX(v.texcoord,_MainTex);
				f.bumpTex_uv = TRANSFORM_TEX(v.texcoord,_BumpTex);

				//to world
				//float3 tan = normalize(   UnityObjectToWorldDir(v.tangent.xyz)   );
				//float3 nor = normalize( UnityObjectToWorldNormal( v.normal)    );
				//float3 bi_tan = normalize(cross(nor,tan) * v.tangent.w);

				float3 tan =    UnityObjectToWorldDir(v.tangent.xyz) ;  
				float3 nor =  UnityObjectToWorldNormal( v.normal)    ;
				float3 bi_tan = cross(nor,tan) * v.tangent.w;
				

				f.tan_tan =			float3( tan.x, bi_tan.x, nor.x);
				f.tan_binormal =	float3( tan.y, bi_tan.y, nor.y); 
				f.tan_normal =		float3( tan.z,  bi_tan.z, nor.z);

				//float3x3 ObjToTan_Matrix = float3x3(tan,bi_tan,nor);

				
				//TANGENT_SPACE_ROTATION;


				//f.world_viewDir = normalize( WorldSpaceViewDir(v.pos));
				//f.world_lightDir = normalize( WorldSpaceLightDir(v.pos));

				f.worldPos = mul(unity_ObjectToWorld,v.pos);

				return f;
				
			}

			float4 frag(v2f f) : SV_TARGET
			{
				//float3 world_viewDir = normalize( f.world_viewDir);
				//float3 world_lightDir = normalize( f.world_lightDir);


				float3 world_viewDir = normalize( UnityWorldSpaceViewDir(f.worldPos));
				float3 world_lightDir = normalize(UnityWorldSpaceLightDir(f.worldPos));

				float4 unpackNor = tex2D(_BumpTex,f.bumpTex_uv);
				//float3 tan_nor;
				//tan_nor.xy = unpackNor.xy * _BumpScale;
				//tan_nor.z = saturate(1 - sqrt(dot( tan_nor.xy , tan_nor.xy)));

				float3 tan_nor = UnpackNormal( unpackNor );
				tan_nor.xy = tan_nor.xy * _BumpScale;
				tan_nor.z = sqrt(1 - saturate(dot( tan_nor.xy , tan_nor.xy)));

				//to world
				tan_nor = float3(dot(f.tan_tan,tan_nor),dot(f.tan_binormal,tan_nor),dot(f.tan_normal,tan_nor));
				tan_nor = normalize(tan_nor);
				

				float3 albedo = tex2D(_MainTex,f.mainTex_uv) * _Color;
				

				float3 _ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;
				float3 _diffuse = _LightColor0.rgb * albedo * saturate(dot( tan_nor , world_lightDir));
				float3 _half = normalize(world_viewDir + world_lightDir);
				float3 _specular = _LightColor0.rgb * _Specular * pow( dot(tan_nor , _half ) ,_Gloss);

				float3 resultColor = _ambient + _diffuse + _specular;

				return float4( resultColor , 1);

			}

			ENDCG
		}
	}
}