//后处理 bloom 效果
Shader "zxy/post_effect_04_bloom"
{
	Properties
	{
		_MainTex ("MainTex",2D) = "white" {}
		_BloomTex ("BloomTex",2D) = "white" {}
		_BlurSize ("BlurSize",Float) = 0.6
		_LuminanceThreshord ("LuminanceThreshord",float) = 0.6
	}
	
	SubShader
	{
		ZTest Always
		ZWrite Off
		Cull Off

		CGINCLUDE

		sampler2D _MainTex;
		float4 _MainTex_TexelSize;
		sampler2D _BloomTex;
		float _BlurSize;
		float _LuminanceThreshord;

		//--得到明亮部分的区域图像--------------
		struct a2v
		{
			float4 vertex : POSITION;
			float2 texcoord : TEXCOORD0;
		};

		struct v2f
		{
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
		};

		float luminance(float3 color)
		{
			return color.r * 0.2125 + color.g * 0.7154 + color.b * 0.0721;
		}
		
		v2f vert_bright(a2v v)
		{
			v2f f;
			f.pos = UnityObjectToClipPos(v.vertex);

			f.uv = v.texcoord;
			return f;
		}

		float4 frag_bright(v2f f) : SV_Target
		{
			float3 color = tex2D(_MainTex,f.uv);
			float lum = luminance(color);
			float bright = clamp(lum - _LuminanceThreshord,0,1);
			return float4(color * bright,1);
		}
		//--------------------------

		//--得到 ‘明亮，模糊之后的图像’ 和 ‘原图像’ 混合之后 的图像--------------
		struct v2f_blend
		{
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
			float2 uv2 : TEXCOORD1;
		};

		v2f_blend vert_blend(a2v v)
		{
			v2f_blend f;
			f.pos = UnityObjectToClipPos(v.vertex);

			f.uv = v.texcoord;
			f.uv2 = v.texcoord;
			#if UNITY_UV_STARTS_AT_TOP
			if(_MainTex_TexelSize.y < 0)
			{
				f.uv2.y = 1 - f.uv2.y;
			}
			#endif
			

			return f;
		}

		float4 frag_blend(v2f_blend f) : SV_Target
		{
			float3 color = tex2D(_MainTex,f.uv);
			float3 color2 = tex2D(_BloomTex,f.uv2);
			return float4((color + color2),1);
		}

		//------------------------

		ENDCG

		Pass
		{
			CGPROGRAM
			#pragma vertex vert_bright
			#pragma fragment frag_bright
			ENDCG
		}

		UsePass "zxy/post_effect_03_gassia_blur/GAUSSIA_BLUR_VERTICAL"

		UsePass "zxy/post_effect_03_gassia_blur/GAUSSIA_BLUR_HORIZONTAL"

		Pass
		{
			
			CGPROGRAM
			#pragma vertex vert_blend
			#pragma fragment frag_blend
			ENDCG
		}

		

	}

	FallBack Off
}
