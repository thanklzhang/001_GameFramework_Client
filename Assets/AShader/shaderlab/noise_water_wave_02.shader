//噪声 水波
Shader "zxy/noise_water_wave_02"
{
	Properties
	{
		_Color ("_Color",Color) = (0,0.15,0.115,1)
		_MainTex ("_MainTex",2D) = "white" {}
		_WaveMap ("_WaveMap",2D) = "bump" {}
		//_BumpTex ("_BumpTex",2D) = "white" {}
		_CubeMap ("_CubeMap",Cube) = "_skyBox" {}
		_Distortion ("_Distortion",Range(0,100)) = 10
		//_ReflectionAmount ("_ReflectionAmount",Range(0,1)) = 1

		_WaveXSpeed ("_WaveXSpeed",float) = 0.01
		_WaveYSpeed ("_WaveYSpeed",float) = 0.01
	}

	SubShader
	{
		GrabPass{"_RefractionTex"}

		Pass
		{
			Tags{"Queue" = "Transparent" "RenderType" = "Opaque"}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			float4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _WaveMap;
			float4 _WaveMap_ST;
			//sampler2D _BumpTex;
			//float4 _BumpTex_ST;
			samplerCUBE _CubeMap;
			float _Distortion;
			//float _ReflectionAmount;
			sampler2D _RefractionTex;
			float4 _RefractionTex_TexelSize;

			float _WaveXSpeed;
			float _WaveYSpeed;
		
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
				float2 uv_wave : TEXCOORD1;

				float4 worldPos : TEXCOORD2;

				//tangent to world
				float3 m1 : TEXCOORD3;
				float3 m2 : TEXCOORD4;
				float3 m3 : TEXCOORD5;

				//float3 worldNormal : TEXCOORD5;

				float4 screenPos : TEXCOORD6;

				
			};

			v2f vert(a2v v)
			{
				v2f f;
				f.pos = UnityObjectToClipPos(v.vertex);
				f.uv = TRANSFORM_TEX(v.coord,_MainTex);
				f.uv_wave = TRANSFORM_TEX(v.coord,_WaveMap);

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
				float2 speed = _Time.y * float2(_WaveXSpeed,_WaveYSpeed);
				//float2 speed = float2(_WaveXSpeed,_WaveYSpeed);

				//折射
				float3 bump = UnpackNormal( tex2D(_WaveMap,f.uv_wave + speed));
				float3 bump2 = UnpackNormal( tex2D(_WaveMap,f.uv_wave - speed));
				float3 avgBump = normalize(bump + bump2);
				//float3 avgBump = normalize(bump);

				//float3 tangentBumpVector = UnpackNormal( tex2D(_BumpTex,f.uv));
				float4 screenPos = f.screenPos;
				float2 offset = avgBump.xy * _Distortion * _RefractionTex_TexelSize.xy;
				float3 refractColor = tex2D(_RefractionTex,((screenPos.xy + offset) / screenPos.w));// * screenPos.z
				//float3 refractColor = tex2D(_GrabTexture,screenPos.xy / screenPos.w);

				//refractColor = (1 - refractColor);

				//反射
				float3 viewDir = normalize( UnityWorldSpaceViewDir(f.worldPos));
				avgBump = normalize( float3(dot(f.m1,avgBump) , dot(f.m2,avgBump) , dot(f.m3,avgBump)) );
				float3 reflectDir = reflect(-viewDir,normalize(avgBump));
				float3 reflectColor = texCUBE(_CubeMap,reflectDir);

				//mainTex
				float3 diffColor = tex2D(_MainTex,f.uv + speed);
				reflectColor = reflectColor * diffColor * _Color;
				//reflectColor =  diffColor * _Color;

				//菲涅尔
				float fresnel = pow(1 - saturate(dot(viewDir,avgBump)),4);

				//折射和反射融合
				float3 result = reflectColor * fresnel + refractColor * (1 - fresnel);
				//float3 result = reflectColor * fresnel + refractColor * (1 - fresnel);
				//float3 result = reflectColor * 0.5 + refractColor* 0.5;
				//float3 result = reflectColor * 1 + refractColor * (0);
				//float3 result = reflectColor + refractColor;
				//float3 result = (fresnel,fresnel,fresnel);
				//float3 result = bump;
				return float4(result,1);
			}


			ENDCG
		}
	}

	FallBack "VertexLit"
}