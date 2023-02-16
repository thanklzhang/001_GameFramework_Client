//后处理 雾 效果
Shader "zxy/post_effect_07_fog"
{
	Properties
	{
		_MainTex ("MainTex",2D) = "white" {}
		_FogDensity ("BlurAmount",Range(0,1)) = 0.9
		_FogColor ("BlurAmount",Color) = (1,1,1,1)
		_FogStart ("BlurAmount",Float) = 0
		_FogEnd ("BlurAmount",Float) = 2
		_FarDepth ("FarDepth",Float) = 1
	}
	
	SubShader
	{
		ZTest Always
		ZWrite Off
		Cull Off

		CGINCLUDE

		#include "UnityCG.cginc"

		sampler2D _MainTex;
		float2 _MainTex_TexelSize;
		sampler2D _CameraDepthTexture;
		
		float _FogDensity;
		float4 _FogColor;
		float _FogStart;
		float _FogEnd;

		float4x4 _Matrix;

		float _FarDepth;

		struct a2v
		{
			float4 vertex : POSITION;
			float2 texcoord : TEXCOORD0;
		};

		struct v2f
		{
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
			float2 uv_depth : TEXCOORD1;
			float3 interpolateVector : TEXCOORD2;
		};

	
		v2f vert(a2v v)
		{
			v2f f;
			f.pos = UnityObjectToClipPos(v.vertex);

			f.uv = v.texcoord;
			f.uv_depth = v.texcoord;

			#if UNITY_UV_STARTS_AT_TOP
			if(_MainTex_TexelSize.y < 0)
			{
				f.uv_depth.y = 1 - f.uv_depth.y;
			}
			#endif

			int index = 0;
			if(f.uv.x < 0.5 && f.uv.y < 0.5)
			{
				index = 0;
			}
			else if(f.uv.x > 0.5 && f.uv.y < 0.5)
			{
				index = 1;
			}
			else if(f.uv.x > 0.5 && f.uv.y > 0.5)
			{
				index = 2;
			}
			else if(f.uv.x < 0.5 && f.uv.y > 0.5)
			{
				index = 3;
			}

			#if UNITY_UV_STARTS_AT_TOP
			if(_MainTex_TexelSize.y < 0)
			{
				index = 3 - index;
			}
			#endif

			f.interpolateVector = _Matrix[index];

			return f;
		}

		float4 frag(v2f f) : SV_Target
		{
			float3 color = tex2D(_MainTex,f.uv);
			
			float d = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture,f.uv_depth);
			float linearD = LinearEyeDepth(d);

			float3 resultColor = color;
			
			//天空盒不生效
			//float linear01D = Linear01Depth(d);
			//if(linear01D < 1)

			if(linearD < _FarDepth - 1)
			{
				float3 worldPos = _WorldSpaceCameraPos + linearD * f.interpolateVector;

				float fog_f = (_FogEnd - worldPos.y) / (_FogEnd - _FogStart);

				float3 density = saturate(fog_f * _FogDensity);

				resultColor = lerp(color,_FogColor,density);
			}

			return float4(resultColor,1);
		}

		ENDCG

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}

	}

	FallBack Off
}
