//高级纹理 渲染纹理 玻璃
Shader "zxy/test_high_texture_05_glass"
{
	Properties
	{
		_MainTex ("_MainTex",2D) = "white" {}
		_BumpTex ("_BumpTex",2D) = "white" {}
		_CubeMap ("_CubeMap",Cube) = "_skyBox" {}
		_Distortion ("_Distortion",Range(0,100)) = 10
		_ReflectionAmount ("_ReflectionColor",Range(0,1)) = 1
	}

	SubShader
	{
		GrabPass{}

		Pass
		{
			Tags{"Queue" = "Transparent" "RenderType" = "Opaque"}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _BumpTex;
			float4 _BumpTex_ST;
			samplerCUBE _CubeMap;
			float _Distortion;
			float _ReflectionAmount;
			sampler2D _GrabTexture;
			float4 _GrabTexture_TexelSize;
		
			struct a2v
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float2 coord : TEXCOORD0;
				
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;

				float4 worldPos : TEXCOORD1;

				//tangent to world
				float3 m1 : TEXCOORD2;
				float3 m2 : TEXCOORD3;
				float3 m3 : TEXCOORD4;

				float3 worldNormal : TEXCOORD5;

				float4 screenPos : TEXCOORD6;
			};

			v2f vert(a2v v)
			{
				v2f f;
				f.pos = UnityObjectToClipPos(v.vertex);
				f.uv = TRANSFORM_TEX(v.coord,_MainTex);
				f.worldPos = mul(unity_ObjectToWorld,v.vertex);
				f.screenPos = ComputeGrabScreenPos(f.pos);

				float3 tanXCoord = normalize( UnityObjectToWorldDir( v.tangent));
				float3 tanZCoord = normalize( UnityObjectToWorldNormal(v.normal));
				float3 tanYCoord = normalize( cross( tanZCoord , tanXCoord) * v.tangent.w);
				
				f.m1 = float3(tanXCoord.x,tanYCoord.x,tanZCoord.x);
				f.m2 = float3(tanXCoord.y,tanYCoord.y,tanZCoord.y);
				f.m3 = float3(tanXCoord.z,tanYCoord.z,tanZCoord.z);

				return f;
			}

			float4 frag(v2f f) : SV_Target
			{
				//折射
				float3 tangentBumpVector = UnpackNormal( tex2D(_BumpTex,f.uv));
				float4 screenPos = f.screenPos;
				float2 offset = tangentBumpVector.xy * _Distortion * _GrabTexture_TexelSize.xy;
				float3 refractColor = tex2D(_GrabTexture,((screenPos.xy + offset) / screenPos.w));
				//float3 refractColor = tex2D(_GrabTexture,screenPos.xy / screenPos.w);

				refractColor = (1 - refractColor);

				//反射
				float3 viewDir = normalize( UnityWorldSpaceViewDir(f.worldPos));
				float3 worldBumpVector = normalize( (dot(f.m1,tangentBumpVector) , dot(f.m2,tangentBumpVector) , dot(f.m3,tangentBumpVector)) );
				float3 reflectDir = reflect(-viewDir,normalize(worldBumpVector));
				float3 reflectColor = texCUBE(_CubeMap,reflectDir);

				//mainTex
				float3 diffColor = tex2D(_MainTex,f.uv);
				reflectColor = reflectColor * diffColor;

				//折射和反射融合
				float3 result = refractColor * (1 - _ReflectionAmount) + reflectColor * _ReflectionAmount;
				return float4(result,1);
			}


			ENDCG
		}
	}

	FallBack "VertexLit"
}